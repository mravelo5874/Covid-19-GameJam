using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class GameData : Singleton<GameData> 
{
    // grid data:
    [Header("Grid Data")]
    public int gridWidth;
    public int gridHeight;
    public bool[] walkableTiles;

    // player data:
    [Header("Player Data")]
    public float playerSpeed;

    // npc data:
    [Header("NPC Data")]
    public float npcSpeedHealthy;
    public float npcSpeedInfected;
    public float npcSpreadChance;

    // ability data:
    [Header("Ability Data")]
    public int coughProjectileCount;
    public float coughCooldown;
    public float coughGermSpeed;

    public int sneezeProjectileCount;
    public float sneezeCooldown;
    public float sneezeGermSpeed;
    public float sneezeSpread;

    // germ data:
    [Header("Germ Data")]
    public float germdespawnProbability;
    public float germSpreadChance;



    public void Start()
    {
        // init SaveSystem
        SaveSystem.Init();

        // load data from file (json format)
        string json = SaveSystem.Load();
        if (json != null)
        {
            //print("load: " + json);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            walkableTiles = saveObject.dataArray;
            GameObject.Find("GameManager").GetComponent<GameManager>().SetGridWalkableData(walkableTiles);
        }
        else
        {
            walkableTiles = new bool[gridWidth * gridHeight];
        }
        
    }
    
    public void OnGUI()
    {
        // reload scene
        if (GUI.Button(new Rect(10, 10, 80, 30), "Restart")) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // save grid layout
        if (GUI.Button(new Rect(10, 50, 80, 30), "Save Grid"))
        {
            walkableTiles = GameManager.instance.GetGridWalkableData();
            SaveObject saveObject = new SaveObject();
            saveObject.dataArray = walkableTiles;
            string json = JsonUtility.ToJson(saveObject);
            //print ("save: " + json);
            SaveSystem.Save(json);
        }
    }
}

[System.Serializable]
public class SaveObject
{
    public bool[] dataArray;
}