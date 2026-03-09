using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthbarOwner : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] float yOffset = 108f;

    // NOTE(Gerald 2025 07 31) this is supposed to be a read-only inspector field for convenience,
    // so developers can easily find a unit's healthbar.
    [SerializeField] Healthbar healthbar;

    public Health Health => health;
    public float YOffset => yOffset;

    bool started = false; // Start() was called

    void Awake()
    {
        health = GetComponent<Health>();

    }
    
    void Start()
    {
        HealthbarsHUD healthbarsHUD = HUD.Instance.HealthbarsHUD;
        healthbar = healthbarsHUD.Register(this);
        started = true;
    }

    void OnEnable()
    {
        //NOTE(Gerald 2025 07 31):
        // in the middle of the game, we want to not draw healthbars of disabled object.
        // however, at startup, we might Awake() before the HUD and so we don't Register() until we Start()'ed
        if (started)
        {
            HealthbarsHUD healthbarsHUD = HUD.Instance.HealthbarsHUD;
            healthbarsHUD.Register(this);
        }
    }

    void OnDisable()
    {
        if (started && HUD.Instance)
        {
            HealthbarsHUD healthbarsHUD = HUD.Instance.HealthbarsHUD;
            healthbarsHUD.Unregister(this);
        }
    }
}