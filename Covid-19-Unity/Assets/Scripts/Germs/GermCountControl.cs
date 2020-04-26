using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermCountControl : MonoBehaviour
{
    public int germThreshold;
    public int totalGerms;
    public bool isDestroying;

    // Update is called once per frame
    void Update()
    {
        if(isDestroying)
        {
            return;
        }

        totalGerms = transform.childCount;
        if (totalGerms >= germThreshold)
        {
            DeleteGerms(totalGerms - germThreshold);
        }
    }

    private void DeleteGerms(int amount)
    {
        isDestroying = true;
        for (int i = 0; i < amount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        isDestroying = false;
    }
}
