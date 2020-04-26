using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusceptibleObject : MonoBehaviour
{
    public Status status;
    public float chanceToSpread;
    private npcSpreadAbilities spreadAbilities;
    private SimpleAI simpleAI;

    void Start() 
    {
        // load data from GameData:
        chanceToSpread = GameData.instance.npcSpreadChance;

        simpleAI = GetComponent<SimpleAI>();
        spreadAbilities = GetComponent<npcSpreadAbilities>();
        float rand = Random.Range(5f, 7f);
        InvokeRepeating("AttemptToSpread", 1f, rand);  //1s delay, repeat every x seconds
    }

    public void Infect()
    {
        // return if game is paused
        if (GameData.instance.isPaused)
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
        if (GameData.instance.isPaused || !simpleAI.inRange)
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
