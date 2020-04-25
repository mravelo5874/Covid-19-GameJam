using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public SpritePool buildingSprites;

    void Start()
    {
        // assign a random building sprite
        int index = Random.Range(0, buildingSprites.sprites.Length);
        GetComponent<SpriteRenderer>().sprite = buildingSprites.sprites[index];
    }
}
