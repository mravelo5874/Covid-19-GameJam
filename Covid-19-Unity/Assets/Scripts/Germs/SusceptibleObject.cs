using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusceptibleObject : MonoBehaviour
{
    public Status status;
    public float chanceToSpread;
    private npcSpreadAbilities spreadAbilities;

    void Start() 
    {
        // load data from GameData:
        chanceToSpread = GameData.instance.npcSpreadChance;

        spreadAbilities = GetComponent<npcSpreadAbilities>();
        float rand = Random.Range(8f, 12f);
        InvokeRepeating("AttemptToSpread", 1f, rand);  //1s delay, repeat every x seconds
    }

    public void Infect()
    {
        // return if game is paused
        if (GameManager.instance.isPaused)
        {
            return;
        }
        
        if (status == Status.healthy)
        {
            status = Status.infected;

            npcController npc = GetComponent<npcController>();
            if (npc != null)
            {
                npc.BecomeInfected();
            }
        }
    }

    private void AttemptToSpread()
    {
        // return if game is paused
        if (GameManager.instance.isPaused)
        {
            return;
        }

        // spread if infected
        if (status == Status.infected)
        {
            float num = Random.value;
            if (num <= chanceToSpread)
            {
                int type = Random.Range(0, 2);
                if (type == 0)
                {
                    spreadAbilities.CoughAbility();
                }
                else
                {
                    spreadAbilities.SneezeAbility();
                }
            }
        }
    }
}
