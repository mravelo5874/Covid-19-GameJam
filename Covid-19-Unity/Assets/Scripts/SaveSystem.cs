using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static readonly string FILE_NAME = "gridSave.txt";


    public static void Init()
    {   
        // create save folder if it does not already exist
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + FILE_NAME, saveString);
    }

    public static string Load()
    {
        if (File.Exists(SAVE_FOLDER + FILE_NAME))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + FILE_NAME);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
