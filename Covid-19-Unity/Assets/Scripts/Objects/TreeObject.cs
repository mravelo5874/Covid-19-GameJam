using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObject : MonoBehaviour
{
    public GameObject[] Trees;

    void Start()
    {
        // delete child trees
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        int index = Random.Range(0, 4);
        GameObject tree = Instantiate(Trees[index], transform.position, Quaternion.identity, transform);
    }
}
