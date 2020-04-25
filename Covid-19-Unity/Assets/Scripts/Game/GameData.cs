using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class GameData : Singleton<GameData> 
{
    // dev:
    public bool devTools;
    public bool isPaused;

    // game grid data:
    [Header("Game Grid Data")]
    public int gridWidth;
    public int gridHeight;
    public bool[] walkableTiles;
    public Vector3 gridOrigin;

    // menu grid data:
    [Header("Menu Grid Data")]
    public int menuGridWidth;
    public int menuGridHeight;
    public bool[] menuWalkableTiles;
    public Vector3 menuGridOrigin;

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
    public float germImmuneTime;

    // fluid capsule data:
    [Header("Fluid Capsule Data")]
    public float fluidCapsuleMaxCapacity;
    public float coughFluidCost;
    public float sneezeFluidCost;

    void Start()
    {
        // init SaveSystem
        SaveSystem.Init();
    }

    public void LoadMenuWalkableTiles()
    {
         // load data from file (json format)
        string json = SaveSystem.LoadMenuTiles();
        if (json != null)
        {
            //print("load: " + json);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            walkableTiles = saveObject.dataArray;
            MenuGameManager.instance.SetGridWalkableData(walkableTiles);
        }
        else
        {
            walkableTiles = new bool[gridWidth * gridHeight];
        }
    }

    public void LoadGameWalkableTiles()
    {
         // load data from file (json format)
        string json = SaveSystem.LoadGameTiles();
        if (json != null)
        {
            //print("load: " + json);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            walkableTiles = saveObject.dataArray;
            MenuGameManager.instance.SetGridWalkableData(walkableTiles);
        }
        else
        {
            walkableTiles = new bool[gridWidth * gridHeight];
        }
    }
    
    public void OnGUI()
    {
        if (devTools)
        {
            // reload scene
            if (GUI.Button(new Rect(10, 10, 80, 30), "Restart")) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            // save game grid layout
            if (GUI.Button(new Rect(10, 50, 120, 30), "Save Game Grid"))
            {
                walkableTiles = GameManager.instance.GetGridWalkableData();
                SaveObject saveObject = new SaveObject();
                saveObject.dataArray = walkableTiles;
                string json = JsonUtility.ToJson(saveObject);
                //print ("save: " + json);
                SaveSystem.SaveGameTiles(json);
            }

            // save menu grid layout
            if (GUI.Button(new Rect(10, 90, 120, 30), "Save Menu Grid"))
            {
                walkableTiles = MenuGameManager.instance.GetGridWalkableData();
                SaveObject saveObject = new SaveObject();
                saveObject.dataArray = walkableTiles;
                string json = JsonUtility.ToJson(saveObject);
                //print ("save: " + json);
                SaveSystem.SaveMenuTiles(json);
            }
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        
    }
}

[System.Serializable]
public class SaveObject
{
    public bool[] dataArray;
}