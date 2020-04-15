using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public Vector2Int movementRange; // min and max distances that npc can move
    [Range(0,1)] public float moveProb; // probability of moving if idle

    private npcController npc;
    private float timer = 0f;

    void Start()
    {
        npc = GetComponent<npcController>();
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            AttemptToWalk();
            timer = 0f;
        }
    }

    private void AttemptToWalk()
    {
        if (!npc.isMoving)
        {
            float probability = Random.Range(0f, 1f);

            if (probability <= moveProb)
            {
                int units = Random.Range(movementRange.x, movementRange.y);
                int direction = Random.Range(0, 7);

                npc.SimpleWalk((Direction)direction, units);
            }
        }
    }
}
