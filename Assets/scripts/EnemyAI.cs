using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IAI
{
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Stack<Vector3> path;

    void Update()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
    }

    public void MoveTowardsTarget(Vector3 targetPosition)
    {
        this.targetPosition = GetNearestAdjacentPosition(targetPosition);
        if (this.targetPosition == transform.position)
        {
            // No valid move available (adjacent positions are blocked), do not move
            return;
        }

        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        path = FindPath(transform.position, this.targetPosition, obstacleManager.obstacleData);
        if (path != null && path.Count > 0)
        {
            isMoving = true;
        }
    }

    void MoveAlongPath()
    {
        if (path.Count > 0)
        {
            Vector3 nextPosition = path.Peek();
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
            {
                transform.position = nextPosition;
                path.Pop();
            }
        }
        else
        {
            isMoving = false;
        }
    }

    Stack<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, ObstacleData obstacleData)
    {
        try
        {
            Vector2Int start = new Vector2Int(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.z));
            Vector2Int target = new Vector2Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.z));

            List<Vector2Int> openList = new List<Vector2Int>();
            HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
            Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

            openList.Add(start);
            gScore[start] = 0;
            fScore[start] = Vector2Int.Distance(start, target);

            while (openList.Count > 0)
            {
                Vector2Int current = GetLowestFScoreNode(openList, fScore);

                if (current == target)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openList.Remove(current);
                closedList.Add(current);

                foreach (Vector2Int neighbor in GetNeighbors(current))
                {
                    if (closedList.Contains(neighbor) || IsObstacle(neighbor, obstacleData))
                    {
                        continue;
                    }

                    float tentativeGScore = gScore[current] + Vector2Int.Distance(current, neighbor);

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Vector2Int.Distance(neighbor, target);
                }
            }

            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error finding path: " + ex.Message);
            return null;
        }
    }

    Vector2Int GetLowestFScoreNode(List<Vector2Int> openList, Dictionary<Vector2Int, float> fScore)
    {
        Vector2Int lowest = openList[0];
        float lowestScore = fScore.ContainsKey(lowest) ? fScore[lowest] : float.MaxValue;

        foreach (var node in openList)
        {
            float score = fScore.ContainsKey(node) ? fScore[node] : float.MaxValue;
            if (score < lowestScore)
            {
                lowest = node;
                lowestScore = score;
            }
        }

        return lowest;
    }

    List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(node.x + 1, node.y),
            new Vector2Int(node.x - 1, node.y),
            new Vector2Int(node.x, node.y + 1),
            new Vector2Int(node.x, node.y - 1)
        };

        return neighbors;
    }

    bool IsObstacle(Vector2Int position, ObstacleData obstacleData)
    {
        int index = position.y * 10 + position.x;
        return index >= 0 && index < obstacleData.obstacleArray.Length && obstacleData.obstacleArray[index];
    }

    Stack<Vector3> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        Stack<Vector3> totalPath = new Stack<Vector3>();
        totalPath.Push(new Vector3(current.x, 0.5f, current.y));

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Push(new Vector3(current.x, 0.5f, current.y));
        }

        return totalPath;
    }

    Vector3 GetNearestAdjacentPosition(Vector3 playerPosition)
    {
        Vector3[] adjacentPositions = new Vector3[]
        {
            playerPosition + Vector3.forward,
            playerPosition + Vector3.back,
            playerPosition + Vector3.left,
            playerPosition + Vector3.right
        };

        foreach (var pos in adjacentPositions)
        {
            if (!IsObstacle(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z)), FindObjectOfType<ObstacleManager>().obstacleData) && pos != transform.position)
            {
                return pos;
            }
        }

        return transform.position; // No valid adjacent position found
    }
}
