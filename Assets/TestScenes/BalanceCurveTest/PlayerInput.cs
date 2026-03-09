using UnityEngine;
using BarqueOfRa.Units;
using BarqueOfRa.Game.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BarqueOfRa.Test.BalanceCurveTest
{
    enum MouseButton
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }
    

    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] GameObject highlighterPrefab;
        GameObject highlighterInstance;

        [SerializeField] LayerMask playerInputLayer;
        [SerializeField] LayerMask highlighterLayer;
        [SerializeField] Barque_BalanceCurveTest barque;
        [SerializeField] Transform unitSelectUI;
        Soul selectedSoul = null;

        [Tooltip("offset for position of dragging gameobject")]
        [SerializeField] Vector3 OffsetSelectionPostion;
        [SerializeField] Vector3 highlighterMoveOffset = new Vector3(0, 2.25f, 0);

        [SerializeField]
        GameObject soulDummyPrefab;
        GameObject soulDummy = null;
        GameObject GuardianDummy = null;
        Guardian guardianDummySource;
        UIState uiState = UIState.Neutral;
        DefenseSlot selectedSlot = null;
        DefenseSlot lastSelectedSlot = null;
        [SerializeField] UnitFactory unitsCatalog;
        Guardian selectedGuardian = null;


        [SerializeField] public UnityEvent unitSummoned;
        [SerializeField] HUD hud;

        bool MouseActionPressed => Input.GetMouseButtonDown((int)MouseButton.Left);
        bool MouseActionReleased => Input.GetMouseButtonUp((int)MouseButton.Left);

        [SerializeField] InputActionReference cancelActionRef;
        InputAction cancelAction;

        bool PressedCancel => cancelAction.WasPerformedThisFrame();

        //TODO : state pattern makes sense here, break up into separate scripts.

        enum UIState
        {
            Neutral,
            DraggingSoul,
            DraggingGuardian,
            WaitingForUnitSelect
        }

        private void Awake()
        {
            cancelAction = cancelActionRef;
        }

        void Update()
        {
            Camera camera = Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);

            RaycastHit raycastData;
            bool raycastSuccessful = Physics.Raycast(ray, out raycastData, Mathf.Infinity, playerInputLayer, QueryTriggerInteraction.Collide);
            Debug.DrawRay(ray.origin, ray.direction);
            Collider hitCollider = raycastData.collider;

            RaycastHit raycastHit;
            bool raycast = Physics.Raycast(ray, out raycastHit, Mathf.Infinity, highlighterLayer);
            raycast = raycastSuccessful;
            raycastHit = raycastData;
            switch (uiState)
            {
                case UIState.Neutral:
                    if (PressedCancel)
                    {
                        hud?.OpenPauseMenu();
                    }
                    else if(MouseActionPressed && raycastSuccessful)
                    {
                        Debug.Log($"Mouse pressed while UI at neutral: mouse points at: {raycastData.collider}");

                        if (selectedSoul = hitCollider.GetComponent<Soul>())
                        {
                            Debug.Log($"souls left on board: {barque.SoulCabin.SoulsLeft}");
                            if (barque.SoulCabin.SoulsLeft <= 1)
                            {
                                DenySummoningLastSoul();
                            }
                            else
                            {
                                soulDummy = Instantiate(soulDummyPrefab, raycastData.point, Quaternion.identity);
                                PrepareSummon();
                                uiState = UIState.DraggingSoul;
                            }
                        }
                        else if (selectedGuardian = hitCollider.GetComponent<Guardian>())
                        {

                            PrepareMove();
                            uiState = UIState.DraggingGuardian;
                        }
                    }

                    break;
                case UIState.DraggingSoul:
                    if (selectedSoul != null && soulDummy != null)
                    {

                        if (raycast)
                        {
                            soulDummy.transform.position = raycastHit.point + OffsetSelectionPostion;

                            if (raycastData.collider != null)
                            {
                                selectedSlot = raycastData.collider.GetComponent<DefenseSlot>();
                            }
                            
                            if (selectedSlot != null)
                            {
                                selectedSlot.SetHoverOverState();
                                lastSelectedSlot = selectedSlot;
                            }

                            else if (selectedSlot == null && lastSelectedSlot != null)
                            {
                                lastSelectedSlot.ResetMaterial();
                                lastSelectedSlot = null;
                            }
                        }
                    }

                    if (MouseActionReleased)
                    {
                        Debug.Log($"Mouse released while UI at dragging soul, pointing at: {raycastData.collider}");

                        if (soulDummy != null)
                        {
                            Destroy(highlighterInstance);
                            Destroy(soulDummy);
                        }

                        if (selectedSoul == null)
                        {
                            Debug.LogError($"UIState is DraggingSoul ({uiState}), but selected Soul is {selectedSoul}");
                            uiState = UIState.Neutral;
                        }
                        else
                        {
                            bool freeSlotSelected = false;
                            if (raycastSuccessful)
                            {                            
                                freeSlotSelected = selectedSlot != null && selectedSlot.IsFree;
                                if (freeSlotSelected)
                                {
                                    ShowUnitSelectUI();
                                    uiState = UIState.WaitingForUnitSelect;
                                    freeSlotSelected = true;
                                }
                            }
                            if (!freeSlotSelected)
                            {
                                CancelSummon();
                                uiState = UIState.Neutral;
                            }
                        }
                    }

                    break;
                case UIState.DraggingGuardian:
                    if(GuardianDummy)
                    {
                        GuardianDummy.transform.position = raycastHit.point + OffsetSelectionPostion;

                        GuardianDummy.GetComponent<Health>().RemoveHealth(GuardianDummy.GetComponent<Health>().CurrentHealth - guardianDummySource.GetComponent<Health>().CurrentHealth);

                        Debug.Log(guardianDummySource.GetComponent<Health>().CurrentHealth + "CurrentHealth us s " );

                        if (raycast)
                        {
                            GuardianDummy.transform.position = raycastHit.point + OffsetSelectionPostion;

                            if (raycastData.collider != null)
                            {
                                selectedSlot = raycastData.collider.GetComponent<DefenseSlot>();
                            }

                            if (selectedSlot != null)
                            {
                                selectedSlot.SetHoverOverState();
                                lastSelectedSlot = selectedSlot;
                            }

                            else if (selectedSlot == null && lastSelectedSlot != null)
                            {
                                lastSelectedSlot.ResetMaterial();
                                lastSelectedSlot = null;
                            }
                        }
                    }
                   
                    if (MouseActionReleased)
                    {
                        Debug.Log($"Mouse released while UI at dragging Guardian, pointing at: {raycastData.collider}");

                        if (highlighterInstance) { Destroy(highlighterInstance); }

                        bool moveSuccessful = false;

                        if (selectedGuardian == null)
                        {
                            Debug.LogWarning("No Guardian selected in state Dragging Guardian");
                        }
                        else if (raycastSuccessful)
                        {
                            if (selectedSlot = hitCollider.GetComponent<DefenseSlot>())
                            {
                                if (selectedSlot.IsFree)
                                {
                                    Unit unit = selectedGuardian.GetComponent<Unit>();

                                    if (unit == null)
                                    {
                                        Debug.LogError("guardian object missing unit component when trying to place");
                                    }
                                    else
                                    {
                                        unit.transform.position = selectedSlot.transform.position + unit.Offset;
                                        unit.transform.parent = selectedSlot.transform;

                                        DisableGuardianDummy(selectedGuardian);


                                        Debug.Log($"Move successful at {unit.transform.position}");

                                        moveSuccessful = true;
                                    }
                                }
                                else
                                {
                                    Guardian oldOccupantGuardian = selectedSlot.TryGetOccupantGuardian();
                                    if (oldOccupantGuardian != null)
                                    {
                                        SwapUnits(oldOccupantGuardian.GetComponent<Unit>(), selectedGuardian.GetComponent<Unit>());
                                        moveSuccessful = true;
                                    }
                                    else
                                    {
                                        CancelMove();
                                    }
                                }
                            }
                            else
                            {
                                Guardian oldOccupantGuardian = hitCollider.GetComponent<Guardian>();
                                if (oldOccupantGuardian != null)
                                {
                                    SwapUnits(oldOccupantGuardian.GetComponent<Unit>(), selectedGuardian.GetComponent<Unit>());
                                    moveSuccessful = true;
                                }
                                else
                                {
                                    CancelMove();
                                }

                            }
                        }
                        if (!moveSuccessful)
                        {
                            CancelMove();
                        }
                        uiState = UIState.Neutral;
                    }
                    break;
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
            unitSummoned.Invoke();
            return newUnit;
        }

        void PrepareSummon()
        {
            if (highlighterInstance == null)
            {
                highlighterInstance = Instantiate(highlighterPrefab);
                highlighterInstance.transform.position = soulDummy.transform.position;
                highlighterInstance.transform.SetParent(soulDummy.transform);
            }
            Debug.Log("Prepare Summon");
            AudioManager.Instance.PlaySound("soul_summoned");
            // selectedSoul.gameObject.SetActive(false); // TODO: Clean up
        }

        public void ShowUnitSelectUI()
        {
            unitSelectUI.gameObject.SetActive(true);
            MoveUIToMouseCursor(unitSelectUI);
        }

        void MoveUIToMouseCursor(Transform unitSelectUI)
        {
            Camera cam = Camera.main;
            var mousePosition = Input.mousePosition;
            unitSelectUI.position = mousePosition;
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

        public void summonSelectedUnit(int unitTypeIndex)
        {
            if (uiState != UIState.WaitingForUnitSelect)
            {
                Debug.LogError($"unexpected state ({uiState})while summon selected unit requested: ");
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
                unit.transform.position = selectedSlot.transform.position + unit.Offset;
                unit.transform.parent = selectedSlot.transform;

                Debug.Log($"Summon successful at {unit.transform.position}");
                AudioManager.Instance.PlaySound("soul_placed");
            }
            uiState = UIState.Neutral;
            HideUnitSelectUI();

        }

        public void HideUnitSelectUI()
        {
            unitSelectUI.gameObject.SetActive(false);
        }

        void DisableGuardianDummy(Guardian selectedGuardian)
        {
            Destroy(GuardianDummy);

            for (int i = 0; i < selectedGuardian.transform.childCount; i++)
            {
                selectedGuardian.transform.GetChild(i).gameObject.SetActive(true); // disables the visual and children of the models 
            }

        }
        void EnableGuadianDummy(Guardian selectedGuardian)
        {

            for (int i = 0; i < selectedGuardian.transform.childCount; i++)
            {
                selectedGuardian.transform.GetChild(i).gameObject.SetActive(false); // disables the visual and children of the models 
            }
            GuardianDummy.GetComponent<CapsuleCollider>().enabled = false;

        }
        void PrepareMove()
        {
            guardianDummySource = selectedGuardian;

            if (highlighterInstance == null)
            {
                GuardianDummy = Instantiate(selectedGuardian.gameObject);

                EnableGuadianDummy(selectedGuardian);


                highlighterInstance = Instantiate(highlighterPrefab);
                highlighterInstance.transform.position = GuardianDummy.transform.position + highlighterMoveOffset;
                highlighterInstance.transform.SetParent(GuardianDummy.transform);


            }
            Debug.Log("Prepare Move");
        }

        void CancelSummon()
        {
            Debug.Log("Cancel Summon");
            selectedSoul.gameObject.SetActive(true);
            selectedSoul = null;
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }

        void CancelMove()
        {
            if(GuardianDummy && selectedGuardian)
            {

                Destroy(GuardianDummy);

                for (int i = 0; i < selectedGuardian.transform.childCount; i++)
                {
                    selectedGuardian.transform.GetChild(i).gameObject.SetActive(true); // enables the visual and children of the models 
                }
            }

            Debug.Log("Cancel Move");
            selectedGuardian = null;
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }

        void SwapUnits(Unit first, Unit second)
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

            DisableGuardianDummy(selectedGuardian);
            AudioManager.Instance.PlaySound("soul_placed");
        }

        void DenySummoningLastSoul()
        {
            Debug.Log("Attempt to Summon Last Soul Denied!");
            AudioManager.Instance.PlaySound("soul_pickup_denied");
        }
    }
}
