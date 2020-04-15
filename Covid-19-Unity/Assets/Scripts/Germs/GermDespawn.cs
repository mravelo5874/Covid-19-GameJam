using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermDespawn : MonoBehaviour
{
    [Range(0, 1)] public float despawnProbability = 0.25f; // chance that germ is despawned every second;
    private bool isDespawning = false;
    public float timeToDespawn = 3f;
    private float timer = 0f;

    void Start()
    {
        InvokeRepeating("AttemptToDespawn", 1f, 1f);  //1s delay, repeat every 1s
    }

    void Update()
    {
        if (isDespawning)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= timeToDespawn)
            {
                Destroy(this.gameObject);
            }

            // shrink germ before despawning
            float value =  1 - (timer / timeToDespawn);
            this.transform.localScale = new Vector3(value, value, value);
        }
    }

    public void Despawn()
    {
        isDespawning = true;
        timeToDespawn = 1f;
    }

    private void AttemptToDespawn()
    {
        float num = Random.value;
        if (num <= despawnProbability)
        {
            isDespawning = true;
        }
    }
}
