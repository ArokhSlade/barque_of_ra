using UnityEngine;
using BarqueOfRa.Units;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using BarqueOfRa.Game.UI.States.Combat;

namespace BarqueOfRa.Game.UI
{
    public class PlayerCombatInput : MonoBehaviour
    {
        bool frozen = false; //NOTE(Gerald 2025 08 04): used to coordinate with higher-priority ui state, i.e. modal windows (pause menu).

        [SerializeField] Neutral neutralState;
        [SerializeField] DraggingSoul draggingSoulState;
        [SerializeField] DraggingGuardian draggingGuardianState;
        [SerializeField] SelectingUnit selectingUnitState;

        public Neutral NeutralState => neutralState;
        public DraggingSoul DraggingSoulState => draggingSoulState;
        public DraggingGuardian DraggingGuardianState => draggingGuardianState;
        public SelectingUnit SelectingUnitState => selectingUnitState;

        public Soul SelectedSoul { get; set; }
        public Guardian SelectedGuardian { get; set; }
        public DefenseSlot SelectedSlot { get; set; }
        public DefenseSlot LastSelectedSlot { get; set; }
        public GameObject HighlighterInstance { get; set; }

        [SerializeField] GameObject highlighterPrefab;
        [SerializeField] Transform unitSelectUI; //TODO(Gerald 2025 08 04): make it a class for modular protection

        public GameObject HighlighterPrefab => highlighterPrefab;
        public Barque Barque => InGame.Instance.Barque;
        public Transform UnitSelectUI => unitSelectUI; //TODO(Gerald 2025 08 04): make it a class for modular protection

        [SerializeField] InputActionReference positiveClickActionRef;
        [SerializeField] InputActionReference negativeClickActionRef;
        [SerializeField] InputActionReference pointerPositionActionRef;
        [SerializeField] InputActionReference pauseMenuActionRef;
        
        InputAction PositiveClickAction;
        InputAction NegativeClickAction;
        InputAction PointerPositionAction;
        Vector2 pointerPosition;
        InputAction PauseMenuAction;

        public bool PositiveClickDown => PositiveClickAction.WasPressedThisFrame();
        public bool PositiveClickReleased => PositiveClickAction.WasReleasedThisFrame();
        public bool NegativeClickDown => NegativeClickAction.WasPressedThisFrame();
        public bool NegativeClickReleased => NegativeClickAction.WasReleasedThisFrame();
        public Vector2 PointerPosition => pointerPosition;

        [SerializeField]  LayerMask soulsLayer;
        [SerializeField]  LayerMask slotsLayer;
        [SerializeField]  LayerMask guardiansLayer;
        [SerializeField]  LayerMask enemiesLayer;
        [SerializeField]  LayerMask highlightablesLayer;

        public LayerMask SoulsLayer => soulsLayer;
        public LayerMask SlotsLayer => slotsLayer;
        public LayerMask GuardiansLayer => guardiansLayer;
        public LayerMask EnemiesLayer => enemiesLayer;
        public LayerMask HighlightablesLayer => highlightablesLayer;


        [SerializeField] GameObject soulDummyPrefab;
        public GameObject SoulDummyPrefab => soulDummyPrefab;
        public GameObject SoulDummyInstance { get; set; }
        public GameObject GuardianDummyInstance { get; set; }
        public Guardian GuardianDummySource { get; set; }


        [SerializeField] Vector3 dummyFloatOffset; //TODO(Gerald 2025 08 04): replace this with a float, it's only vertical after all.
        public Vector3 DummyFloatOffset => dummyFloatOffset; //TODO(Gerald 2025 08 04): replace this with a float, it's only vertical after all.
        public Vector3 highlighterVFXOffsetDuringMove; //TODO(Gerald 2025 08 04): this is about the highlighter not being centered on dummy because of its pivot. could be solved more elgantly.
        public Vector3 HighlighterVFXOffsetDuringMove => highlighterVFXOffsetDuringMove; //TODO(Gerald 2025 08 04): this is about the highlighter not being centered on dummy because of its pivot. could be solved more elgantly.

        [SerializeField] UnitFactory unitsCatalog;
        [SerializeField] public UnityEvent UnitSummoned;

        [SerializeField] PauseMenu pauseMenu;


        CombatUIState currentState;

        void OnPointerMoved(InputAction.CallbackContext context)
        {
            pointerPosition = context.ReadValue<Vector2>();
           
        }

        void Awake()
        {
            PositiveClickAction = positiveClickActionRef;
            NegativeClickAction = negativeClickActionRef;
            PointerPositionAction = pointerPositionActionRef;
            PauseMenuAction = pauseMenuActionRef;

            neutralState.Initialize(this);
            draggingSoulState.Initialize(this);
            draggingGuardianState.Initialize(this);
            selectingUnitState.Initialize(this);

            currentState = neutralState;
        }

        void OnEnable()
        {
            PointerPositionAction.performed += OnPointerMoved;
            PauseMenuAction.performed += TogglePauseMenu;
        }

        void OnDisable()
        {
            Debug.Log("input disabled");
            PointerPositionAction.performed -= OnPointerMoved;
            PauseMenuAction.performed -= TogglePauseMenu;
        }

        void TogglePauseMenu(InputAction.CallbackContext context)
        {
            UI.Instance.TogglePauseMenu();
        }

        void Update()
        {
            if (!frozen)
            {
                CombatUIState nextState = currentState.StateUpdate();
                if (nextState != currentState)
                {
                    currentState.Exit();
                    nextState.Enter();
                    currentState = nextState;
                }
            }
        }


        //TODO(Gerald 2025 08 03): refactor original to parameterless version and without typo
        public void EnableGuardianDummy()
        {
            for (int i = 0; i < SelectedGuardian.transform.childCount; i++)
            {
                SelectedGuardian.transform.GetChild(i).gameObject.SetActive(false); // disables the visual and children of the models 
            }
            GuardianDummyInstance.GetComponent<CapsuleCollider>().enabled = false;
        }

        public void DisableGuardianDummy(Guardian selectedGuardian)
        {
            Destroy(GuardianDummyInstance);

            for (int i = 0; i < selectedGuardian.transform.childCount; i++)
            {
                selectedGuardian.transform.GetChild(i).gameObject.SetActive(true); // disables the visual and children of the models 
            }
        }

        public void DenySummoningLastSoul()
        {
            Debug.Log("Attempt to Summon Last Soul Denied!");
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }

        public void PrepareSummon()
        {
            if (HighlighterInstance == null)
            {
                HighlighterInstance = Instantiate(highlighterPrefab);
                HighlighterInstance.transform.position = SoulDummyInstance.transform.position;
                HighlighterInstance.transform.SetParent(SoulDummyInstance.transform);
            }
            Debug.Log("Prepare Summon");
            AudioManager.Instance.PlaySound("soul_summoned");
            // selectedSoul.gameObject.SetActive(false); // TODO: Clean up
        }

        public void SummonSelectedUnit(int unitTypeIndex)
        {
            if (currentState != SelectingUnitState)
            {
                Debug.LogError($"unexpected state ({currentState}) while summon selected unit requested.");
                return;
            }

            UnitType unitType = mapIndexToUnitType(unitTypeIndex);
            var unitObject = SummonUnit(unitType);
            if (unitObject != null)
            {
                Unit unit = unitObject.GetComponent<Unit>();
                if (unit == null)
                {
                    Debug.LogError("unit object missing unit component when trying to summon");
                    return;
                }
                unit.transform.position = SelectedSlot.transform.position + unit.Offset;
                unit.transform.parent = SelectedSlot.transform;

                Debug.Log($"Summon successful at {unit.transform.position}");
                AudioManager.Instance.PlaySound("soul_placed");
            }
            HideUnitSelectUI();
        }

        UnitType mapIndexToUnitType(int unitTypeIndex)
        {
            switch (unitTypeIndex)
            {
                case 1:
                    return UnitType.GuardianMelee_Brawler;
                case 2:
                    return UnitType.GuardianMelee_Assassin;
                case 3:
                    return UnitType.GuardianMelee_Tank;
                default:
                    Debug.LogError("Unknown UnitType index requested");
                    return UnitType.GuardianMelee;
            }
        }

        GameObject SummonUnit(UnitType unitType)
        {
            Debug.Log("Summon Unit");
            if (unitsCatalog == null)
            {
                return null;
            }
            GameObject newUnit = unitsCatalog.CreateUnit(unitType);
            UnitSummoned.Invoke();
            return newUnit;
        }

        public void PrepareMove()
        {
            GuardianDummySource = SelectedGuardian;

            if (HighlighterInstance == null)
            {
                GuardianDummyInstance = GameObject.Instantiate(SelectedGuardian.gameObject);

                EnableGuardianDummy();


                HighlighterInstance = GameObject.Instantiate(HighlighterPrefab);
                HighlighterInstance.transform.position = GuardianDummyInstance.transform.position + HighlighterVFXOffsetDuringMove;
                HighlighterInstance.transform.SetParent(GuardianDummyInstance.transform);
            }
            Debug.Log("Prepare Move");
        }

        public void ShowUnitSelectUI()
        {
            unitSelectUI.gameObject.SetActive(true);
            MoveUIToMouseCursor(unitSelectUI);
        }

        public void HideUnitSelectUI()
        {
            unitSelectUI.gameObject.SetActive(false);
        }

        public void MoveUIToMouseCursor(Transform unitSelectUI)
        {
            Camera cam = Camera.main;
            var mousePosition = pointerPosition;
            unitSelectUI.position = mousePosition;
        }

        public void CancelSummon()
        {
            Debug.Log("Cancel Summon");
            SelectedSoul.gameObject.SetActive(true);
            SelectedSoul = null;
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }

        public void SwapUnits(Unit first, Unit second)
        {

            Debug.Log("Swap Positions");
            if (first == null || second == null)
            {
                Debug.LogError("swap units: at least one was null");
                return;
            }

            Vector3 tempPosition = first.transform.position;
            first.transform.position = second.transform.position;
            second.transform.position = tempPosition;

            DisableGuardianDummy(SelectedGuardian);
            AudioManager.Instance.PlaySound("soul_placed");
        }

        public void CancelMove()
        {
            if (GuardianDummyInstance && SelectedGuardian)
            {

                Destroy(GuardianDummyInstance);

                for (int i = 0; i < SelectedGuardian.transform.childCount; i++)
                {
                    SelectedGuardian.transform.GetChild(i).gameObject.SetActive(true); // enables the visual and children of the models 
                }
            }
            Debug.Log("Cancel Move");
            SelectedGuardian = null;
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }

        void OnDestroy()
        {
            neutralState.CleanUp();
            draggingSoulState.CleanUp();
            draggingGuardianState.CleanUp();
            selectingUnitState.CleanUp();
        }
    }
}
