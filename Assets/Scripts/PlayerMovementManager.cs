using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public PlayerController playerController;
    public AStarPathfinding pathfinding;
    private List<Vector3> path;
    private int targetIndex;
    private EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = FindObjectOfType<EnemyAI>();
    }

    void Update()
    {
        if (!playerController.IsMoving() && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GridCubeInfo cubeInfo = hit.transform.GetComponent<GridCubeInfo>();
                if (cubeInfo != null)
                {
                    Vector3 targetPosition = new Vector3(cubeInfo.x, 0.5f, cubeInfo.y);
                    path = pathfinding.FindPath(playerController.transform.position, targetPosition);
                    if (path != null && path.Count > 0)
                    {
                        targetIndex = 0;
                        MoveToNextTile();
                        if (enemyAI != null)
                        {
                            enemyAI.MoveTowards(targetPosition);
                        }
                    }
                }
            }
        }
    }

    void MoveToNextTile()
    {
        if (targetIndex < path.Count)
        {
            playerController.SetTargetPosition(path[targetIndex]);
            targetIndex++;
        }
    }

    void LateUpdate()
    {
        if (!playerController.IsMoving() && path != null && targetIndex < path.Count)
        {
            MoveToNextTile();
        }
    }
}
