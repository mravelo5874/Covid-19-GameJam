using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationBar : MonoBehaviour
{
    public SpritePool spritePool;
    private Image image;
    private const int SPRITE_NUM = 31;
    public static PopulationBar instance { get; private set; }

    private TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        image = GetComponent<Image>();
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        image.sprite = spritePool.sprites[0];
        UpdatePopulationBar();
    }

    public void UpdatePopulationBar()
    {
        int popTotal = npcPoolData.instance.totalNpcCount;
        int infected = npcPoolData.instance.infectedNpcCount;
        timerText.text = infected.ToString() + "/" + popTotal.ToString();

        float percent = (float)infected / (float)popTotal;
        if (percent <= 0f)
        {
            image.sprite = spritePool.sprites[0];
        }
        else if (percent >= 1f)
        {
            image.sprite = spritePool.sprites[30];
        }
        else
        {
            int index = (int)(SPRITE_NUM * percent);
            image.sprite = spritePool.sprites[index];
        } 
    }
}
