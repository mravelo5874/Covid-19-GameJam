using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used Code-Monkey tutorial: https://www.youtube.com/watch?v=alU04hvz6L4&t=297s
public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private myGrid<PathNode> grid;
    private List<PathNode> openList; // what nodes to search
    private List<PathNode> closedList; // nodes that have already been searched

    public Pathfinding(int width, int height)
    {
        grid = new myGrid<PathNode>(width, height, 1f, Vector3.zero, (myGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public myGrid<PathNode> GetGrid()
    {
        return grid;
    }

    // finds a path between two positions in the graph
    public List<PathNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        // get the first Node (startPos) and add it to open list
        PathNode startNode = GetNode(startPos.x, startPos.y);
        PathNode endNode = GetNode(endPos.x, endPos.y);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = GetNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();


        // CYCLE:
        while (openList.Count > 0)
        {
            PathNode currNode = GetLowestFCostNode(openList);
            if (currNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currNode);
            closedList.Add(currNode);

            foreach (PathNode neighborNode in GetNeighbors(currNode))
            {
                if (closedList.Contains(neighborNode))
                {
                    continue;
                }
                // ignores nodes that are not walkable
                if (!neighborNode.isWalkable)
                {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentCost = currNode.gCost + CalculateDistanceCost(currNode, neighborNode);
                if (tentCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currNode;
                    neighborNode.gCost = tentCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) 
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes in open list!
        return null;
    }

    private List<PathNode> GetNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>();

        if (node.x - 1 >= 0) 
        {
            // Left:
            neighbors.Add(GetNode(node.x - 1, node.y));
            // Left-Down:
            if (node.y - 1 >= 0)
            {
                neighbors.Add(GetNode(node.x - 1, node.y - 1));
            }
            // Left-Up:
            if (node.y + 1 < grid.GetHeight())
            {
                neighbors.Add(GetNode(node.x - 1, node.y + 1));
            }
        }
        if (node.x + 1 < grid.GetWidth())
        {
            // Right:
            neighbors.Add(GetNode(node.x + 1, node.y));
            // Right Down:
            if (node.y - 1 >= 0)
            {
                neighbors.Add(GetNode(node.x + 1, node.y - 1));
            }
            // Right-Up:
            if (node.y + 1 < grid.GetHeight())
            {
                neighbors.Add(GetNode(node.x + 1, node.y + 1));
            }
        }
        // Down:
        if (node.y - 1 >= 0)
        {
            neighbors.Add(GetNode(node.x, node.y - 1));
        }
        // Up:
        if (node.y + 1 < grid.GetHeight())
        {
            neighbors.Add(GetNode(node.x, node.y + 1));
        }

        return neighbors;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currNode = endNode;
        while (currNode.cameFromNode != null)
        {
            path.Add(currNode.cameFromNode);
            currNode = currNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int rem = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * rem;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode min = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < min.fCost)
            {
                min = nodes[i];
            }
        }
        
        return min;
    }
}
