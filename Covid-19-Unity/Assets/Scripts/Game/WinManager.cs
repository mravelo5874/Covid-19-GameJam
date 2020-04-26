using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinManager : MonoBehaviour
{
    public static WinManager instance { get; private set; }
    public TextMeshProUGUI winText;
    public GameObject AdvancedModeUnlocked;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        AdvancedModeUnlocked.SetActive(false);
    }

    public void SetWinTime(string time, float numTime)
    {
        winText.text = "Time: " + time.ToString();

        if (numTime <= 300 && !GameData.instance.isAdvanceModeUnlocked())
        {
            AdvancedModeUnlocked.SetActive(true);
            SaveSystem.UnlockAdvancedMode();
        }
    }
}
