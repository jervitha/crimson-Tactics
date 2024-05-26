using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;
    private int gridSize = 10;

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                if (obstacleData.obstacleArray[index])
                {
                    Vector3 position = new Vector3(x, 0.5f, y);
                    Instantiate(obstaclePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
