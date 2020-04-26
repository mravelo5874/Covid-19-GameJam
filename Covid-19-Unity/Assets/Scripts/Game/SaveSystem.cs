using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveAdvanceMode
{
    public bool isUnlocked;
}

public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string GAME_FILE_NAME = "gameGridSave.txt";
    public static readonly string MENU_FILE_NAME = "menuGridSave.txt";
    public static readonly string ADVANCED_MODE = "advMode.txt";


    public static void Init()
    {   
        // create save folder if it does not already exist
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static bool LoadAdvancedMode()
    {
        if (File.Exists(SAVE_FOLDER + ADVANCED_MODE))
        {
            string json = File.ReadAllText(SAVE_FOLDER + ADVANCED_MODE);
            SaveAdvanceMode saveObject = JsonUtility.FromJson<SaveAdvanceMode>(json);
            return saveObject.isUnlocked;
        }
        else
        {
            return false;
        }
    }

    public static void UnlockAdvancedMode()
    {
        SaveAdvanceMode saveObject = new SaveAdvanceMode();
        saveObject.isUnlocked = true;
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(SAVE_FOLDER + ADVANCED_MODE, json);
    }

    public static void SaveGameTiles(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + GAME_FILE_NAME, saveString);
    }

    public static void SaveMenuTiles(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + MENU_FILE_NAME, saveString);
    }

    public static string LoadGameTiles()
    {
        if (File.Exists(SAVE_FOLDER + GAME_FILE_NAME))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + GAME_FILE_NAME);
            return saveString;
        }
        else
        {
            return null;
        }
    }

    public static string LoadMenuTiles()
    {
        if (File.Exists(SAVE_FOLDER + MENU_FILE_NAME))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + MENU_FILE_NAME);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
