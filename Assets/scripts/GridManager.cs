using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public int gridSize = 10;
    private GameObject[,] grid;

    void Start()
    {
        grid = new GameObject[gridSize, gridSize];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.name = $"Cube_{x}_{y}";
                cube.AddComponent<TileInfo>().SetPosition(x, y);
                grid[x, y] = cube;
            }
        }
    }
}
