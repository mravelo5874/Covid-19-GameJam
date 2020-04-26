using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct npcDataNode
{
    private int time;
    private int infectedNpcCount;

    public npcDataNode(int time, int infectedNpcCount)
    {
        this.infectedNpcCount = infectedNpcCount;
        this.time = time;
    }

    public Vector2Int GetData()
    {
        return new Vector2Int(time, infectedNpcCount);
    }
}

public class npcPoolData : MonoBehaviour
{
    public bool collectData = false;
    public bool isTutorial = false;
    public static npcPoolData instance { get; private set; }
    private List<npcDataNode> npcData;
    private List<SusceptibleObject> npcPool;
    public int totalNpcCount = 1;
    public int infectedNpcCount = 0;
    private int currTime = 0;
    private bool isWin = false;

    void Awake()
    {
        instance = this;
        // create lists
        npcData = new List<npcDataNode>();
        npcPool = new List<SusceptibleObject>();
        // get each child in transform and add to list
        foreach(Transform child in transform)
        {
            SusceptibleObject so = child.GetComponent<SusceptibleObject>();
            if (so != null)
            {
                npcPool.Add(so);
            }
        }

        totalNpcCount = npcPool.Count;
        infectedNpcCount = 0;

        // collect data at time 0
        npcDataNode node = new npcDataNode(currTime, infectedNpcCount);
        npcData.Add(node);

        // invoke collect data every 5 mins
        InvokeRepeating("CollectData", 1f, 1f);  //0s delay, repeat every 1s
    }

    public void CollectData()
    {
        if (isWin)
        {
            return;
        }

        int count = 0;
        foreach(SusceptibleObject npc in npcPool)
        {
            if (npc.status == Status.infected)
            {
                count++;
            }
        }
        infectedNpcCount = count;
        currTime++;

        if (infectedNpcCount >= totalNpcCount && !isTutorial)
        {
            GameManager.instance.WinGame();
            isWin = true;
        }

        if (collectData)
        {
            npcDataNode node = new npcDataNode(currTime, infectedNpcCount);
            npcData.Add(node);
        }

        float percent = (float)infectedNpcCount/(float)totalNpcCount;
        PopulationBar.instance.UpdatePopulationBar();
    }
}
