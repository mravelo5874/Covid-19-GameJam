using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagiousObject : MonoBehaviour
{
    [Range(0, 1)] private float spreadChance;
    private float immuneTime;
    private bool isImmune;
    private float timer;

    void Awake()
    {
        // Load data from GameData
        spreadChance = GameData.instance.germSpreadChance;
        immuneTime = GameData.instance.germImmuneTime;
        isImmune = true;
        timer = 0f;
    }

    void Update()
    {
        if (isImmune)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= immuneTime)
            {
                isImmune = false;
            }
        }
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

            if (!isImmune)
            {
                GermDespawn germDespawn = GetComponent<GermDespawn>();
                if (germDespawn != null)
                {
                    germDespawn.Despawn();
                }
            }

        }
    }   
}
