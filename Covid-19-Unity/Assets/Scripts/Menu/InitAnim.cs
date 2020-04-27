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
        yield return new WaitForSeconds(0.2f);
        CtrlZ.Play("ctrlz-NEW");
        yield return new WaitForSeconds(0.6f);
        Fade.Play("Clear");
        yield return new WaitForSeconds(4f);
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
