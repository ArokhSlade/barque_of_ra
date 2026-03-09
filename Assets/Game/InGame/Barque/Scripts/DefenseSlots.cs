using System.Collections.Generic;
using UnityEngine;
using BarqueOfRa.Game.UI;
using UnityEngine.ProBuilder.MeshOperations;

//TODO(Gerald 2025 08 04): namespace should be barque. but barque is name of a class already. what do?
namespace BarqueOfRa.Game.Units
{
    public class DefenseSlots : MonoBehaviour
    {
        [SerializeField] public List<DefenseSlot> slots = new();

       
        public int GuardiansCount => countGuardians();


        static Animator animator;

        [SerializeField] GameObject Transitioning;

       
        public bool IsDisabled { get; set; }


        [Tooltip("Higher number reduces the speed of unit slots enabling/disabling transition")][SerializeField][Range(0.5f,3)] float UnitSlotTransitionSpeed;
       

        



        int countGuardians()
        {
            int result = 0;

            foreach (DefenseSlot slot in slots)
            {
                if (slot.GetComponentInChildren<Guardian>() != null)
                {
                    result += 1;
                }
            }

            return result;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetFloat("TransitionSpeed",UnitSlotTransitionSpeed);
            IsDisabled = true;
            HideSlotVisual();

        }

        public  void DisablingUnitSlots()
        {
            FindAnyObjectByType<PlayerCombatInput>().HideUnitSelectUI();

            IsDisabled = true;

            HideSlotVisual();
            animator.SetTrigger("Disable");
        }
        public  List<Guardian> GetAllGuardians()
        {
            List <Guardian> list = new List<Guardian>();
            for(int i = 0; i < 12;i++)
            {
                if (slots[i].TryGetOccupantGuardian())
                {
                    list.Add(slots[i].TryGetOccupantGuardian());
                }
            }
            return list;
        }
        public void EnablingUnitSlot()
        {
            IsDisabled = false;

            AnimatorClipInfo[] currentClip = animator.GetCurrentAnimatorClipInfo(0);
            float ClipLength = currentClip[0].clip.length;

         

            animator.SetTrigger("Enable");
            Invoke("UnhideSlotVisual", ClipLength);
        }
        void HideSlotVisual()
        {
            foreach(DefenseSlot Slot in slots)
            {
                Debug.Log("A " + Slot.gameObject.name);
                Slot.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Slot.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        }
        void UnhideSlotVisual()
        {
            foreach (DefenseSlot Slot in slots)
            {
                Slot.gameObject.GetComponent<MeshRenderer>().enabled = true;
                Slot.gameObject.layer = LayerMask.NameToLayer("DefenseSlot");

            }
        }

        public DefenseSlot ClosestSlotToPosition(RaycastHit slothitpoint , float SnappingThreshold)
        {
            if (IsDisabled)
            {
                Debug.LogWarning("Boat is transition to way point");
                return null;
            }
            Vector3 Pointposition = slothitpoint.point;
            

            Dictionary<float,DefenseSlot> Distances = new Dictionary<float,DefenseSlot>();

            List<float> ResultSlot = new List<float>();

            DefenseSlot SnapTarget = slots[0];

            foreach(DefenseSlot Slot in slots)
            {
                if(Vector3.Distance(Pointposition, Slot.transform.position) < SnappingThreshold)
                {
                    Distances.Add(Vector3.Distance(Pointposition, Slot.transform.position), Slot);
                    ResultSlot.Add(Vector3.Distance(Pointposition, Slot.transform.position));
                }

            }
            ResultSlot.Sort();

            if(ResultSlot.Count > 0)
            {
                SnapTarget = Distances[ResultSlot[0]];
            }
            else{ SnapTarget = null; }
            return SnapTarget;

        }
    }
}

