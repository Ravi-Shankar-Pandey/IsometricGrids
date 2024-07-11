using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IAI
{
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving;
    private PlayerController playerController;
    public AStarPathfinding pathfinding;
    private List<Vector3> path;
    private int targetIndex;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pathfinding = FindObjectOfType<AStarPathfinding>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3 playerPosition = playerController.transform.position;
            List<Vector3> adjacentTiles = GetAdjacentTiles(playerPosition);
            Vector3 closestTile = GetClosestTile(adjacentTiles, transform.position);

            if (closestTile != transform.position)
            {
                path = pathfinding.FindPath(transform.position, closestTile);
                if (path != null && path.Count > 0)
                {
                    targetIndex = 0;
                    MoveToNextTile();
                }
            }
        }
        else
        {
            Move();
        }
    }

    public void MoveTowards(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        isMoving = true;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            if (targetIndex < path.Count)
            {
                MoveToNextTile();
            }
        }
    }

    private void MoveToNextTile()
    {
        if (targetIndex < path.Count)
        {
            targetPosition = path[targetIndex];
            isMoving = true;
            targetIndex++;
        }
    }

    private List<Vector3> GetAdjacentTiles(Vector3 position)
    {
        List<Vector3> adjacentTiles = new List<Vector3>
        {
            new Vector3(position.x + 1, 0.5f, position.z),
            new Vector3(position.x - 1, 0.5f, position.z),
            new Vector3(position.x, 0.5f, position.z + 1),
            new Vector3(position.x, 0.5f, position.z - 1)
        };

        adjacentTiles.RemoveAll(tile => tile.x < 0 || tile.x >= 10 || tile.z < 0 || tile.z >= 10 || pathfinding.obstacleData.obstacleArray[(int)tile.z * 10 + (int)tile.x]);
        return adjacentTiles;
    }

    private Vector3 GetClosestTile(List<Vector3> tiles, Vector3 fromPosition)
    {
        Vector3 closestTile = fromPosition;
        float minDistance = float.MaxValue;

        foreach (Vector3 tile in tiles)
        {
            float distance = Vector3.Distance(fromPosition, tile);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTile = tile;
            }
        }

        return closestTile;
    }
}
