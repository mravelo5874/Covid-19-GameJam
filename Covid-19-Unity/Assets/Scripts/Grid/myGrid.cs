using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used Code-Monkey tutorial: https://www.youtube.com/watch?v=waEsGu--9P8
public class myGrid <Type>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private Type[,] gridArray;

    public myGrid(int width, int height, float cellSize, Vector3 origin, Func<myGrid<Type>, int, int, Type> createGridObj)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new Type[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridObj(this, x, y);
            }
        }
    }

    public void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.blue, 1000f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.blue, 1000f);
            }
        }

        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.blue, 1000f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.blue, 1000f);
    }

    public Type[,] GetGridData()
    {
        return gridArray;
    }

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public float GetCellSize() { return cellSize; }

    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    public Vector2Int GetXY(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        int y = Mathf.FloorToInt((worldPos - origin).y / cellSize);
        return new Vector2Int(x, y);
    }

    public void SetObject(int x, int y, Type obj)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = obj;
            if (OnGridObjectChanged != null) 
            {
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) 
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetObject(Vector3 worldPos, Type obj)
    {
        Vector2Int pos = GetXY(worldPos);
        SetObject(pos.x, pos.y, obj);
    }

    public Type GetObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else 
        {
            return default(Type);
        }
    }

    public Type GetObject(Vector3 worldPos)
    {
        Vector2Int pos = GetXY(worldPos);
        return GetObject(pos.x, pos.y);
    }

    public Vector3 GetGridOrigin()
    {
        return origin;
    }

}
