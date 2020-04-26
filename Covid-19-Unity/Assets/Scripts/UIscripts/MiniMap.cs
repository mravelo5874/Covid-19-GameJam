using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public static MiniMap instance { get; private set; }
    public GameObject dotPrefab;
    public RectTransform dotParent;
    private GameObject currDot;

    void Start() 
    {
        instance = this;
    }

    public void UpdateMiniMap(int x, int y)
    {
        if (currDot != null)
        {
            Destroy(currDot); 
        }

        RectTransform location = GameObject.Find(x + ":" + y).GetComponent<RectTransform>();
        currDot = Instantiate(dotPrefab, location.transform.position, Quaternion.identity, location) as GameObject;
    }
}
