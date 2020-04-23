using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string GAME_FILE_NAME = "gameGridSave.txt";
    public static readonly string MENU_FILE_NAME = "menuGridSave.txt";


    public static void Init()
    {   
        // create save folder if it does not already exist
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
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
