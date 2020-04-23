using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
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
        // return if game is paused
        if (GameData.instance.isPaused)
        {
            return;
        }
        
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
                bool walkable = false;
                Vector2Int randPos = new Vector2Int();
                while (!walkable)
                {
                    // choose a random x, y from the grid
                    randPos = new Vector2Int(Random.Range(0, GameData.instance.menuGridWidth), Random.Range(0, GameData.instance.menuGridHeight));
                    walkable = MenuGameManager.instance.isWalkablePos(randPos);
                }
                
                npc.SetTargetPosition(MenuGameManager.instance.GetWorldPos(randPos));
            }
        }
    }
}
