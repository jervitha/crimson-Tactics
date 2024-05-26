using UnityEditor;
using UnityEngine;

public class ObstacleEditor : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>("Obstacle Editor");
    }

    private void OnGUI()
    {
        if (obstacleData == null)
        {
            obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);
            return;
        }

        if (GUILayout.Button("Clear Obstacles"))
        {
            for (int i = 0; i < obstacleData.obstacleArray.Length; i++)
            {
                obstacleData.obstacleArray[i] = false;
            }
        }

        int gridSize = 10;
        for (int y = 0; y < gridSize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                obstacleData.obstacleArray[index] = EditorGUILayout.Toggle(obstacleData.obstacleArray[index], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Obstacles"))
        {
            EditorUtility.SetDirty(obstacleData);
            AssetDatabase.SaveAssets();
        }
    }
}
