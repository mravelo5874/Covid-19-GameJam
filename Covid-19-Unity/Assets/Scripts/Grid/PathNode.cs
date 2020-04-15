using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used Code-Monkey tutorial: https://www.youtube.com/watch?v=alU04hvz6L4&t=297s
public class PathNode
{
    private myGrid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable = true;
    public PathNode cameFromNode;
    
    public PathNode(myGrid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + y;
    }

    public void ToggleIsWalkable()
    {
        isWalkable = !isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }
}
