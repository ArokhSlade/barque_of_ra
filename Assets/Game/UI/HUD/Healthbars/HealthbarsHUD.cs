using BarqueOfRa.Game.Units.Enemies;
using System.Collections.Generic;
using UnityEngine;
public class HealthbarsHUD : MonoBehaviour
{
    [SerializeField] GameObject healthbarPrefab;
    [SerializeField] Color enemyEmptyColor = new(0.3137255f, 0.0842549f, 0.05931826f);
    [SerializeField] Color enemyFillColor = new(0.6039216f, 0.1498605f, 0.1137255f); 
      
    [SerializeField] Color guardianEmptyColor = new(0.007843138f, 0.3529412f, 0.5568628f);
    [SerializeField] Color guardianFillColor = new(0.02352941f, 0.6f, 0.9411765f);

    Dictionary<HealthbarOwner, Healthbar> healthbarOwners = new();

    public Healthbar Register(HealthbarOwner healthbarOwner)
    {
        Color healthbarEmptyColor = Color.gray;
        Color healthbarFillColor = Color.white;
        if (healthbarOwner.GetComponent<Enemy>())
        {
            healthbarEmptyColor = enemyEmptyColor;
            healthbarFillColor = enemyFillColor;
        }
        if (healthbarOwner.GetComponent<Guardian>())
        {
            healthbarEmptyColor = guardianEmptyColor;
            healthbarFillColor = guardianFillColor;
        }

        GameObject healthbarGO = Instantiate(healthbarPrefab, transform);
        Healthbar healthbar = healthbarGO.GetComponent<Healthbar>();
        healthbar.Initialize(healthbarOwner, healthbarEmptyColor, healthbarFillColor);
        healthbarOwners[healthbarOwner] = healthbar;
        return healthbar;
    }

    public void Unregister(HealthbarOwner healthbarOwner)
    {
        Healthbar healthbar;
        healthbarOwners.Remove(healthbarOwner, out healthbar);
        GameObject healthbarGO = healthbar.gameObject;
        Destroy(healthbarGO);
    }
}