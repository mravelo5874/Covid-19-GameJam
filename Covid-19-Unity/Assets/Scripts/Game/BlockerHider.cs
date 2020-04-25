using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerHider : MonoBehaviour
{
    void Start()
    {
        // delete child trees
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
