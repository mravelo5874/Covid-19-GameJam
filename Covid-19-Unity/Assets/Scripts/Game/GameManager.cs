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
    public GameObject pauseScreen;
    public GameObject winScreen;

    private int width, height;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Fade.Play("Black");
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);

        // un-mute npcs
        GameData.instance.muteFX = false;

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

        if (GameData.instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetAxis("Fire1") > 0f && Input.GetAxis("Fire2") > 0f)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.RestartGame();
                }
                else
                {
                    TutorialManager.instance.RestartGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.M) || Input.GetKey("joystick button 4") && Input.GetKey("joystick button 5"))
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.ReturnToMenu();
                }
                else
                {
                    TutorialManager.instance.ReturnToMenu();
                }
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

    public void PauseGame()
    {
        GameData.instance.PauseGame();
        pauseScreen.SetActive(GameData.instance.isPaused);
    }

    private IEnumerator GameInit()
    {
        Fade.Play("FadeIn");
        GameData.instance.isPaused = true;
        yield return new WaitForSeconds(1f);
        Fade.Play("Clear");
        yield return new WaitForSeconds(1f);
        GameTimer.instance.StartTimer();
        GameData.instance.isPaused = false;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameAnimation());
    }

    private IEnumerator RestartGameAnimation()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        GameData.instance.isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        StartCoroutine(ReturnToMenuAnimation());
    }

    private IEnumerator ReturnToMenuAnimation()
    {
        Fade.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        GameData.instance.isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void WinGame()
    {
        GameData.instance.PauseGame();
        GameObject.Find("Player").GetComponent<PlayerController>().isControlable = false;
        winScreen.SetActive(true);
        WinManager.instance.SetWinTime(GameTimer.instance.GetText(), GameTimer.instance.GetTime());
    }
}
