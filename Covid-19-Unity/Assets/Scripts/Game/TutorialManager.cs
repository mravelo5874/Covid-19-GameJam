using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance { get; private set; }

    private Camera worldCamera;

    public Animator Fade;
    public GameObject pauseScreen;

    [Header("Intro")]
    public GameObject Intro;

    [Header("Movement")]
    public GameObject Movement;

    [Header("Cough Ability")]
    public GameObject CoughAbility;
    public GameObject coughUI;

    [Header("Sneeze Ability")]
    public GameObject SneezeAbility;
    public GameObject sneezeUI;

    [Header("NPC Infect")]
    public GameObject NPCInfect;
    public GameObject npcs;
    public GameObject popBarUI;

    [Header("Fluid Capsule")]
    public GameObject FluidCapsule;
    public GameObject capsuleUI;
    public GameObject fluidOrbs;

    [Header("Items")]
    public GameObject Items;
    public GameObject items;

    [Header("Timer and Minimap")]
    public GameObject Timer_Minimap;
    public GameObject timerUI;
    public GameObject minimapUI;

    [Header("End")]
    public GameObject End;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Fade.Play("Black");
        pauseScreen.SetActive(false);

        // un-mute npcs
        GameData.instance.muteFX = false;

        // start game
        StartCoroutine(GameInit());

        // start tutorial
        StartCoroutine(TutorialInit());
    }

    void Update() 
    {
        if (GameData.instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetAxis("Fire1") > 0f && Input.GetAxis("Fire2") > 0f)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.RestartGame();
                }
                else
                {
                    TutorialManager.instance.RestartGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.M) || Input.GetKey("joystick button 4") && Input.GetKey("joystick button 5"))
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.ReturnToMenu();
                }
                else
                {
                    TutorialManager.instance.ReturnToMenu();
                }
            }
        }
    }

    private IEnumerator TutorialInit()
    {
        yield return new WaitForSeconds(1f);
        Intro.SetActive(true);
        yield return new WaitForSeconds(5f);
        Intro.SetActive(false);

        StartCoroutine(MovementTutorial());
    }

    private IEnumerator MovementTutorial()
    {
        Movement.SetActive(true);
        GameData.instance.isPaused = false;
        yield return new WaitForSeconds(10f);
        Movement.SetActive(false);

        StartCoroutine(CoughTutorial());
    }

    private IEnumerator CoughTutorial()
    {
        CoughAbility.SetActive(true);
        coughUI.SetActive(true);
        playerSpreadAbilitiesTutorial.instance.canUseCough = true;
        yield return new WaitForSeconds(12f);
        CoughAbility.SetActive(false);

        StartCoroutine(SneezeTutorial());
    }

    private IEnumerator SneezeTutorial()
    {
        SneezeAbility.SetActive(true);
        sneezeUI.SetActive(true);
        playerSpreadAbilitiesTutorial.instance.canUseSneeze = true;
        yield return new WaitForSeconds(12f);
        SneezeAbility.SetActive(false);

        StartCoroutine(NPCInfectTutorial());
    }

    private IEnumerator NPCInfectTutorial()
    {
        NPCInfect.SetActive(true);
        npcs.SetActive(true);
        popBarUI.SetActive(true);
        yield return new WaitForSeconds(15f);
        NPCInfect.SetActive(false);

        StartCoroutine(FluidCapsuleTutorial());
    }

    private IEnumerator FluidCapsuleTutorial()
    {
        FluidCapsule.SetActive(true);
        playerSpreadAbilitiesTutorial.instance.useFluid = true;
        capsuleUI.SetActive(true);
        fluidOrbs.SetActive(true);
        yield return new WaitForSeconds(20f);
        FluidCapsule.SetActive(false);

        StartCoroutine(ItemsTutorial());
    }

    private IEnumerator ItemsTutorial()
    {
        Items.SetActive(true);
        items.SetActive(true);
        yield return new WaitForSeconds(20f);
        Items.SetActive(false);

        StartCoroutine(TimerAndMinimapTutorial());
    }

    private IEnumerator TimerAndMinimapTutorial()
    {
        Timer_Minimap.SetActive(true);
        timerUI.SetActive(true);
        GameTimer.instance.StartTimer();
        minimapUI.SetActive(true);
        yield return new WaitForSeconds(20f);
        Timer_Minimap.SetActive(false);

        StartCoroutine(EndTutorial());
    }

    private IEnumerator EndTutorial()
    {
        End.SetActive(true);
        yield return new WaitForSeconds(15f);
        ReturnToMenu();
    }









    public void PauseGame()
    {
        GameData.instance.PauseGame();
        pauseScreen.SetActive(GameData.instance.isPaused);
    }

    private IEnumerator GameInit()
    {
        Fade.Play("FadeIn");
        GameData.instance.isPaused = true;
        yield return new WaitForSeconds(1f);
        Fade.Play("Clear");
        yield return new WaitForSeconds(1f);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameAnimation());
    }

    private IEnumerator RestartGameAnimation()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        GameData.instance.isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        StartCoroutine(ReturnToMenuAnimation());
    }

    private IEnumerator ReturnToMenuAnimation()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        GameData.instance.isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
