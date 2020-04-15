using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New SpritePool", menuName = "ScriptableObjects/SpritePool", order = 10)]
public class SpritePool : ScriptableObject
{
    public Sprite[] sprites;
}
