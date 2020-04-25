using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // sorting layer = y-pos;
        int y = -1 * (int)transform.position.y;
        spriteRenderer.sortingOrder = y;
    }
}
