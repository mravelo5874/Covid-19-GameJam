using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameManager : MonoBehaviour
{
    public static MenuGameManager instance { get; private set; }
    public Animator Fade;

    [SerializeField] private PathfindingVisual pathfindingVisual;
    private Pathfinding pathfinding;
    private myGrid<PathNode> grid;
    private Camera worldCamera;

    private int width, height;

    // Start is called before the first frame update
    void Start()
    {
        // intro fade animation
        Fade.Play("Black");
        StartCoroutine(Init());

        instance = this;

        // load data from GameData
        width = GameData.instance.menuGridWidth;
        height = GameData.instance.menuGridHeight;
        Vector3 origin = GameData.instance.menuGridOrigin;

        worldCamera = Camera.main;
        pathfinding = new Pathfinding(width, height, origin);
        grid = pathfinding.GetGrid();
        GameData.instance.LoadMenuWalkableTiles();

        if (GameData.instance.devTools)
        {
            grid.DrawGrid();
            pathfindingVisual.SetGrid(grid);
        }
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
                List<PathNode> path = pathfinding.FindPath(new Vector2Int(1, 1), XY);

                if (path != null)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].x - 15, path[i].y - 9) * 1f + Vector3.one * 0.5f, new Vector3(path[i+1].x - 15, path[i+1].y - 9) * 1f + Vector3.one * 0.5f, Color.red, 5f);
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

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(1f);
        Fade.Play("FadeIn");
        yield return new WaitForSeconds(1f);
        Fade.Play("Clear");
    }

    public bool isWalkablePos(Vector2Int pos)
    {
        return grid.GetObject(pos.x, pos.y).isWalkable;
    }

    public Vector3 GetWorldPos(Vector2Int pos)
    {
        return grid.GetWorldPos(pos.x, pos.y);
    }

    public Vector2Int GetGridXY(Vector3 worldPos)
    {
        return grid.GetXY(worldPos);
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

    public void PauseGame()
    {
        GameData.instance.isPaused = !GameData.instance.isPaused;
    }

    public void LoadGameScene()
    {
        // outro fade animation
        Fade.Play("Clear");
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        Fade.Play("Black");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGameScene()
    {
        // outro fade animation
        Fade.Play("Clear");
        StartCoroutine(ExitGame());
    }

    private IEnumerator ExitGame()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        Fade.Play("Black");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
