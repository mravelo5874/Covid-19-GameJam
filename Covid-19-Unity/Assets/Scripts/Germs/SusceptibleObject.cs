using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusceptibleObject : MonoBehaviour
{
    public Status status;
    public float chanceToSpred = 0.5f;
    private npcSpreadAbilities spreadAbilities;

    void Start() 
    {
        spreadAbilities = GetComponent<npcSpreadAbilities>();
        float rand = Random.Range(8f, 12f);
        InvokeRepeating("AttemptToSpread", 1f, rand);  //1s delay, repeat every x seconds
    }

    public void Infect()
    {
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
        // spread if infected
        if (status == Status.infected)
        {
            float num = Random.value;
            if (num <= chanceToSpred)
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
