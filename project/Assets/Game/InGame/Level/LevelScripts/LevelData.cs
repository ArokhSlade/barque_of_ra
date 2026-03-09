using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField]
    private List<Waypoint> barqueWaypoints;
    public List<Waypoint> BarqueWaypoints => barqueWaypoints;
}
