using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PathfindingVisual pathfindingVisual;
    private Pathfinding pathfinding;
    private Camera worldCamera;

    // Start is called before the first frame update
    void Start()
    {
        worldCamera = Camera.main;
        pathfinding = new Pathfinding(13, 11);
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = worldCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector2Int XY = pathfinding.GetGrid().GetXY(mouseWorldPos);
            List<PathNode> path = pathfinding.FindPath(Vector2Int.zero, XY);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * 0.5f, new Vector3(path[i+1].x, path[i+1].y) * 1f + Vector3.one * 0.5f, Color.red, 5f);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = worldCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector2Int XY = pathfinding.GetGrid().GetXY(mouseWorldPos);
            pathfinding.GetNode(XY.x, XY.y).ToggleIsWalkable();
        }
    }
}
