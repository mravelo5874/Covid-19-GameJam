using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance { get; private set; }

    private TextMeshProUGUI timerText;
    private float timer = 0f;
    private bool timerGoing = false;
    private string timerString;


    void Awake()
    {
        instance = this;
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        // return if game is paused
        if (GameData.instance.isPaused)
        {
            return;
        }

        if (timerGoing)
        {
            timer += Time.fixedDeltaTime;
            int sec = (int)timer;

            if (sec >= 60)
            {
                int min = sec / 60;
                sec = sec % 60;

                if (sec <= 9)
                {
                    timerString = min.ToString() + ":0" + sec.ToString();
                    timerText.text = timerString;
                }
                else
                {
                    timerString = min.ToString() + ":" + sec.ToString();
                    timerText.text = timerString;
                }
            }
            else
            {
                if (sec <= 9)
                {
                    timerString = "0:0" + sec.ToString();
                    timerText.text = timerString;
                }
                else
                {
                    timerString = "0:" + sec.ToString();
                    timerText.text = timerString;
                }
            }
        }
    }
    public string GetText()
    {
        return timerString;
    }

    public float GetTime()
    {
        return timer;
    }

    public void StartTimer()
    {
        timerGoing = true;
    }

    public void ResetTimer()
    {
        timerGoing = false;
        timer = 0f;
    }
}
