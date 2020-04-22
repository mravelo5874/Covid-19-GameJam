using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitAnim : MonoBehaviour
{
    public Animator Fade;
    public Animator CtrlZ;

    void Start()
    {
        Fade.Play("Black");
        CtrlZ.Play("Remove");
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(1f);
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1f);
        Fade.Play("Clear");
        CtrlZ.Play("ctrlz");
        yield return new WaitForSeconds(6f);
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
