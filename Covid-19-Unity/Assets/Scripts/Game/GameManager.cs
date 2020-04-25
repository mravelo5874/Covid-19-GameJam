using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private PathfindingVisual pathfindingVisual;
    private Pathfinding pathfinding;
    private myGrid<PathNode> grid;
    private Camera worldCamera;

    public Animator Fade;

    private int width, height;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Fade.Play("Black");

        // load data from GameData
        width = GameData.instance.gridWidth;
        height = GameData.instance.gridHeight;
        Vector3 origin = GameData.instance.gridOrigin;

        worldCamera = Camera.main;
        pathfinding = new Pathfinding(width, height, origin);
        grid = pathfinding.GetGrid();
        GameData.instance.LoadGameWalkableTiles();

        if (GameData.instance.devTools)
        {
            grid.DrawGrid();
            pathfindingVisual.SetGrid(grid);
        }

        // start game
        StartCoroutine(GameInit());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.instance.devTools)
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

    public bool isWalkablePos(Vector2Int pos)
    {
        return grid.GetObject(pos.x, pos.y).isWalkable;
    }

    public Vector3 GetWorldPos(Vector2Int pos)
    {
        return grid.GetWorldPos(pos.x, pos.y);
    }

    public bool[] GetGridWalkableData()
    {
        bool[] walkableData = new bool[width * height];
        PathNode[,] gridData = grid.GetGridData();

        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                walkableData[i] = gridData[x, y].isWalkable;
                i++;
            }
        }

        return walkableData;
    }

    public void SetGridWalkableData(bool[] data)
    {
        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pathfinding.GetNode(x, y).SetIsWalkable(data[i]);
                i++;
            }
        }
    }

    private IEnumerator GameInit()
    {
        Fade.Play("FadeIn");
        GameData.instance.PauseGame();
        yield return new WaitForSeconds(1f);
        Fade.Play("Clear");
        yield return new WaitForSeconds(1f);
        GameTimer.instance.StartTimer();
        GameData.instance.PauseGame();
    }
}
