using BarqueOfRa.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationTrigger : MonoBehaviour
{
    [SerializeField] Barque Barq; // access to barque and souls 

    [SerializeField] public bool OnlyRa, OnlyGuards, OnlySouls; // controls Where VFX spawns 


    [SerializeField]Transform Ra_Character; //access to Ra transform 

    [SerializeField] GameObject VFX_BattleCelebration,VFX_LevelCelebration; // access to Visual effect prefabs

    public static Action Trigger; // acts as triggers for  OnCelebrations 

    public static Action<List<Guardian>,List<Soul>> OnCelebrations; // main event delegate that exceute VFX spawning 

    [SerializeField] float CelebrationDuration = 2f;

    Queue<GameObject> ActiveVFX = new Queue<GameObject>();

    [Header("General Controls")]
    [SerializeField] Vector3 GlobalScaleOffset, GlobalPositionOffset;

    private void OnEnable()
    {
        Trigger += TriggerCelebrationParty;
        OnCelebrations += Celebrate;
    }
    private void OnDisable()
    {
        Trigger -= TriggerCelebrationParty;

        OnCelebrations -= Celebrate;

    }
    void TriggerCelebrationParty()
    {
        OnCelebrations?.Invoke(Barq.AliveGuardians,Barq.AliveSouls);
    }
    void Celebrate(List <Guardian> AliveGuardian,List<Soul> AliveSouls)
    {
        if (OnlyRa && Ra_Character)
        {
            GameObject NewVFX = Instantiate(VFX_BattleCelebration, Ra_Character.position + GlobalPositionOffset, Quaternion.identity);
            NewVFX.transform.localScale = GlobalScaleOffset;
            NewVFX.transform.SetParent(Ra_Character);
            ActiveVFX.Enqueue(NewVFX);
        }
        if(OnlyGuards)
        {
            foreach (Guardian Guard in AliveGuardian)
            {
                GameObject NewVFXA = Instantiate(VFX_BattleCelebration, Guard.transform.position + GlobalPositionOffset, Quaternion.identity);
                NewVFXA.transform.localScale = GlobalScaleOffset;
                NewVFXA.transform.SetParent(Guard.transform);
                ActiveVFX.Enqueue(NewVFXA);
            }

        }
        if(OnlySouls)
        {
            foreach (Soul Passanger in AliveSouls)
            {
                if (!Passanger.Alive) { continue; }

                GameObject NewVFXB = Instantiate(VFX_BattleCelebration, Passanger.transform.position + GlobalPositionOffset, Quaternion.identity);
                NewVFXB.transform.localScale = GlobalScaleOffset;
                NewVFXB.transform.SetParent(Passanger.transform);
                ActiveVFX.Enqueue(NewVFXB);
            }
        }
     
       

        Invoke("DestroyVFX", CelebrationDuration);
    }
    void DestroyVFX()
    {
        int index = ActiveVFX.Count;   
        for(int A = 0; A < index; A++)
        {
            GameObject Trash = ActiveVFX.Dequeue();
            Destroy(Trash);
        }
    }
}
