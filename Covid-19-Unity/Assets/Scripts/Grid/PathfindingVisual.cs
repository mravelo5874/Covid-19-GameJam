using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used Code-Monkey tutorial: https://www.youtube.com/watch?v=alU04hvz6L4&t=297s
public class PathfindingVisual : MonoBehaviour
{
    private myGrid<PathNode> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    public void SetGrid(myGrid<PathNode> grid)
    {
        this.grid = grid;
        UpdateVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, myGrid<PathNode>.OnGridObjectChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate() 
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        int quadCount = grid.GetWidth() * grid.GetHeight();
        MeshUtils.CreateEmptyMeshArrays(quadCount, out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++) 
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1,1) * grid.GetCellSize();

                PathNode pathNode = grid.GetObject(x, y);
                if (pathNode.isWalkable)
                {
                    quadSize = Vector3.zero;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPos(x, y) + quadSize * 0.5f, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
