using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusceptibleObject : MonoBehaviour
{
    public Status status;


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
}
