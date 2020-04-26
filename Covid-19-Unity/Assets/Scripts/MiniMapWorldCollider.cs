using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapWorldCollider : MonoBehaviour
{
    public Vector2Int worldToMapPos;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            MiniMap.instance.UpdateMiniMap(worldToMapPos.x, worldToMapPos.y);
        }
    }
}
