using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public bool useAI = true;
    [Range(0,1)] public float moveProb; // probability of moving if idle

    private npcController npc;
    private float timer = 0f;
    public bool inRange = false;


    void Start()
    {
        npc = GetComponent<npcController>();
    }


    // Update is called once per frame
    void Update()
    {
        // return if game is paused
        if (GameData.instance.isPaused || !inRange || !useAI)
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (!useAI)
        {
            return;
        }
        
        if (other.GetComponent<DummyInRangeScript>() != null)
        {
            inRange = true;
            AttemptToWalk();
            timer = 0f;
        }
        else
        {
            inRange = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.GetComponent<DummyInRangeScript>() != null)
        {
            inRange = false;
            GetComponent<npcController>().StopMoving();
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
                    if (MenuGameManager.instance != null)
                    {
                        randPos = new Vector2Int(Random.Range(0, GameData.instance.menuGridWidth), Random.Range(0, GameData.instance.menuGridHeight));
                    }
                    else
                    {
                        randPos = new Vector2Int(Random.Range(0, GameData.instance.gridWidth), Random.Range(0, GameData.instance.gridHeight));
                    }
                    
                    if (MenuGameManager.instance != null)
                    {
                        walkable = MenuGameManager.instance.isWalkablePos(randPos);
                    }
                    else
                    {
                        walkable = GameManager.instance.isWalkablePos(randPos);
                    }
                }
                
                if (MenuGameManager.instance != null)
                {
                    npc.SetTargetPosition(MenuGameManager.instance.GetWorldPos(randPos));
                }
                else
                {
                    npc.SetTargetPosition(GameManager.instance.GetWorldPos(randPos));
                }
            }
        }
    }
}
