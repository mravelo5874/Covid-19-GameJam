using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermCountControl : MonoBehaviour
{
    public int germThreshold;

    // Update is called once per frame
    void Update()
    {
        int germCount = transform.childCount;
        if (germCount >= germThreshold)
        {
            DeleteGerms(germCount - germThreshold);
        }
    }

    private void DeleteGerms(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
