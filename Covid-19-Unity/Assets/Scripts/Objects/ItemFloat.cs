using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    public float speedUpDown = 1;
    public float distanceUpDown = 1;

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y += Mathf.Sin(speedUpDown * Time.time) * Time.deltaTime * distanceUpDown;
        transform.position = newPos;
    }
}
