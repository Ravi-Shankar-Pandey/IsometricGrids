using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private const int gridSize = 10;
    public ObstacleData obstacleData;

    private class Node
    {
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public int fCost;
        public Node parent;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        Node startNode = new Node((int)start.x, (int)start.z);
        Node endNode = new Node((int)end.x, (int)end.z);

        List<Node> openList = new List<Node> { startNode };
        HashSet<Node> closedList = new HashSet<Node>();

        Node[,] grid = new Node[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = new Node(x, y);
            }
        }

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode, grid))
            {
                if (closedList.Contains(neighbor) || obstacleData.obstacleArray[neighbor.y * gridSize + neighbor.x])
                {
                    continue;
                }

                int tentativeGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (tentativeGCost < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.CalculateFCost();
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> GetNeighbors(Node node, Node[,] grid)
    {
        List<Node> neighbors = new List<Node>();

        if (node.x - 1 >= 0) neighbors.Add(grid[node.x - 1, node.y]);
        if (node.x + 1 < gridSize) neighbors.Add(grid[node.x + 1, node.y]);
        if (node.y - 1 >= 0) neighbors.Add(grid[node.x, node.y - 1]);
        if (node.y + 1 < gridSize) neighbors.Add(grid[node.x, node.y + 1]);

        return neighbors;
    }

    private int GetDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        List<Vector3> waypoints = new List<Vector3>();
        foreach (Node node in path)
        {
            waypoints.Add(new Vector3(node.x, 0.5f, node.y)); // Adjust height as needed
        }
        return waypoints;
    }
}
