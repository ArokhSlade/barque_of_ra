using UnityEngine;

namespace BarqueOfRa.Game.UI
{
    public class DefenseSlot : MonoBehaviour
    {
        [SerializeField] float occupancyRadius;
        [SerializeField] LayerMask unitsLayerMask;
        [SerializeField] LayerMask guardiansLayerMask;
        [SerializeField] LayerMask enemiesLayerMask;

        [SerializeField] Material OccupiedMat, FreeMat, HoverOverMat;

        MeshRenderer slotRenderer;

        public bool IsFree => isFree;
        bool isFree = false;

        private void Awake()
        {
            slotRenderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            isFree = !IsOccupied();

            if (isFree)
            {
                slotRenderer.material = FreeMat; 
            }
            else
            {
                slotRenderer.material = OccupiedMat;
            }
        }

        bool IsOccupied()
        {
            Collider[] colliders = new Collider[1];
            int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, occupancyRadius, colliders, unitsLayerMask, QueryTriggerInteraction.Collide);
            bool result = collidersCount != 0;
            return result;
        }

        public Guardian TryGetOccupantGuardian()
        {
            Guardian result = null;
            Collider[] colliders = new Collider[1];
            int collidersCount = Physics.OverlapSphereNonAlloc(transform.position, occupancyRadius, colliders, guardiansLayerMask, QueryTriggerInteraction.Collide);
            if (collidersCount != 0)
            {
                result = colliders[0].GetComponent<Guardian>();
                if (result == null)
                {
                    Debug.LogError("guardian layer mask not empty but collider lacks guardian component");
                    return null;
                }

            }
            return result;

        }

        public void SetHoverOverState()
        {
            slotRenderer.material = HoverOverMat;
        }

        public void ResetMaterial()
        {
            slotRenderer.material = FreeMat;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, occupancyRadius);
        }
    }
}