using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{   
    Health health;
    [SerializeField] float yOffset;
    
    public void Initialize(HealthbarOwner healthbarOwner, Color healthbarEmptyColor, Color healthbarFillColor)
    {
        health = healthbarOwner.Health;
        healthbarEmptyImage.color = healthbarEmptyColor;
        healthbarFillImage.color = healthbarFillColor;
        yOffset = healthbarOwner.YOffset;
    }
    
    [SerializeField] Image healthbarEmptyImage;
    [SerializeField] Image healthbarFillImage;
    
    void Awake()
    {
        if (healthbarFillImage == null)
        {
            Debug.LogError("no health bar image linked");
            return;
        }
    }

    void Start()
    {
        if (health == null)
        {
            Debug.LogError("no health component linked");
            return;
        }
    }

    void Update()
    {
        int healthNow = health.CurrentHealth;
        int healthMax = health.MaxHealth;
        healthbarFillImage.fillAmount = (float)healthNow / healthMax;
        
        UpdateScreenPosition();
    }
         
    void UpdateScreenPosition()
    {
        Vector3 newPosition = worldToScreenPos(health.transform.position);
        newPosition.y += yOffset;
        transform.position = newPosition;
    }
    
    Vector3 worldToScreenPos(Vector3 worldPos)
    {
        Camera cam = Camera.main;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        return screenPos;
    }
}