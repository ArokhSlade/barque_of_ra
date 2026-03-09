using UnityEngine;

namespace BarqueOfRa.Game
{
    public class AttackPoint2 : MonoBehaviour
    {
        public enum AttackPointID
        {
            LeftFront, LeftMid, LeftBack,
            RightFront, RightMid, RightBack
        }
        public AttackPointID ID;
    }
}
