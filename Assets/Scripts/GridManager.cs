using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCubePrefab;
    private int gridSize = 10;

    void Start()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject cube = Instantiate(gridCubePrefab, new Vector3(x, 0, y), Quaternion.identity);
                cube.GetComponent<GridCubeInfo>().SetPosition(x, y);
            }
        }
    }
}
