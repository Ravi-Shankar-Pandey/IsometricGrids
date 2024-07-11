using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleData))]
public class GridObstacleEditor : Editor
{
    private ObstacleData obstacleData;
    private const int gridSize = 10;

    private void OnEnable()
    {
        obstacleData = (ObstacleData)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Obstacle Grid", EditorStyles.boldLabel);

        for (int y = 0; y < gridSize; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                obstacleData.obstacleArray[index] = GUILayout.Toggle(obstacleData.obstacleArray[index], "");
            }
            GUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(obstacleData);
        }
    }
}
