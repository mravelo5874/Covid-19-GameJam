using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagiousObject : MonoBehaviour
{
    [Range(0, 1)] private float spreadChance;

    void Awake()
    {
        // Load data from GameData
        spreadChance = GameData.instance.germSpreadChance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SusceptibleObject>() != null)
        {
            float num = Random.value;
            if (num <= spreadChance)
            {
                other.GetComponent<SusceptibleObject>().Infect();
                
                GermDespawn germDespawn = GetComponent<GermDespawn>();
                if (germDespawn != null)
                {
                    germDespawn.Despawn();
                }
            }
        }
    }   
}
