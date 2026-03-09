using System.Collections.Generic;
using UnityEngine;

namespace BarqueOfRa.Units
{
    public class UnitFactory : MonoBehaviour
    {
        [SerializeField] List<GameObject> unitPrefabs = new();

        public GameObject CreateUnit(UnitType desiredUnitType)
        {
            Debug.Log("Create Unit");
            GameObject result = null;
            bool unitTypeFound = false;
            foreach (var unitPrefab in unitPrefabs)
            {
                Unit unit = unitPrefab.GetComponent<Unit>();
                if (unit == null)
                {
                    Debug.LogError("UnitPrefab missing Unit component!");
                    return null;
                }
                if (unit.Type == desiredUnitType)
                {
                    result = Instantiate(unitPrefab);
                    unitTypeFound = true;
                    break;
                }
            }
            if (!unitTypeFound)
            {
                Debug.LogError($"Invalid UnitType requested: {desiredUnitType} !");
            }

            return result;
        }
    }

    
}