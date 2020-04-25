using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcons : MonoBehaviour
{
    public static AbilityIcons instance { get; private set; }
    public SpritePool AbilityIconSpritePool;

    [HideInInspector] public bool canUseSneeze;
    [HideInInspector] public bool canUseCough;

    public Image CoughImage;
    public Image SneezeImage;

    public Image CoughCurtain;
    public Image SneezeCurtain;

    private float coughTimer = 0f;
    private float sneezeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        CoughCurtain.fillAmount = 0f;
        SneezeCurtain.fillAmount = 0f;
        canUseCough = true;
        canUseSneeze = true;
        UpdateIcons();
    }

    // Update is called once per frame
    void Update()
    {
        // return if game is paused
        if (GameData.instance.isPaused)
        {
            return;
        }
        
        // update cough fill amount
        if (!canUseCough)
        {
            coughTimer += Time.fixedDeltaTime;
            CoughCurtain.fillAmount = 1f - (coughTimer / GameData.instance.coughCooldown);
            if (coughTimer >= GameData.instance.coughCooldown)
            {
                CoughCurtain.fillAmount = 0f;
                canUseCough = true;
                UpdateIcons();
            }
        }
        // update sneeze fill amount
        if (!canUseSneeze)
        {
            sneezeTimer += Time.fixedDeltaTime;
            SneezeCurtain.fillAmount = 1f - (sneezeTimer / GameData.instance.sneezeCooldown);
            if (sneezeTimer >= GameData.instance.sneezeCooldown)
            {
                SneezeCurtain.fillAmount = 0f;
                canUseSneeze = true;
                UpdateIcons();
            }
        }
    }

    private void UpdateIcons()
    {
        // update cough icon
        if (canUseCough)
        {
            CoughImage.sprite = AbilityIconSpritePool.sprites[3];
        }
        else
        {
            CoughImage.sprite = AbilityIconSpritePool.sprites[2];
        }
        // update sneeze icon
        if (canUseSneeze)
        {
            SneezeImage.sprite = AbilityIconSpritePool.sprites[1];
        }
        else
        {
            SneezeImage.sprite = AbilityIconSpritePool.sprites[0];
        }
    }

    public void UseCoughAbility()
    {
        if (canUseCough)
        {
            StartCoroutine(Cough());
        }
    }

    private IEnumerator Cough()
    {
        canUseCough = false;
        coughTimer = 0f;
        UpdateIcons();

        yield return new WaitForSeconds(GameData.instance.coughCooldown);
    }

    public void UseSneezeAbility()
    {
        if (canUseSneeze)
        {
            StartCoroutine(Sneeze());
        }
    }

    private IEnumerator Sneeze()
    {
        canUseSneeze = false;
        sneezeTimer = 0f;
        UpdateIcons();

        yield return new WaitForSeconds(GameData.instance.sneezeCooldown);
    }
}
