using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChaseTarget : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵 참조
    public Transform target; // 타겟 오브젝트

    private List<Vector3Int> path;
    private int currentPathIndex = 0;

    private void Start()
    {
        FindPath(transform.position, target.position);
    }

    private void Update()
    {
        if (path != null && path.Count > 0)
        {
            Vector3 targetPosition = tilemap.GetCellCenterWorld(path[currentPathIndex]);
            float step = Time.deltaTime * 5; // 이동 속도 조절
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                currentPathIndex++;
                if (currentPathIndex >= path.Count)
                {
                    // 도착한 경우 또는 경로를 끝까지 따라갔을 경우
                    path = null;
                    currentPathIndex = 0;
                }
            }
        }
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3Int startCell = tilemap.WorldToCell(startPosition);
        Vector3Int targetCell = tilemap.WorldToCell(targetPosition);

        path = AStar(startCell, targetCell);
    }

    private List<Vector3Int> AStar(Vector3Int start, Vector3Int target)
    {
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        PriorityQueue<Vector3Int> openSet = new PriorityQueue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            if (current == target)
            {
                return ReconstructPath(cameFrom, start, target);
            }

            closedSet.Add(current);

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + HeuristicCostEstimate(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;

                    float fScore = tentativeGScore + HeuristicCostEstimate(neighbor, target);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }
        }

        return null;
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int start, Vector3Int current)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, current);
            current = cameFrom[current];
        }
        path.Insert(0, start);
        return path;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = {
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0)
        };

        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighbor = cell + dir;
            if (IsTileWalkable(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private float HeuristicCostEstimate(Vector3Int start, Vector3Int target)
    {
        return Vector3Int.Distance(start, target);
    }

    private bool IsTileWalkable(Vector3Int cell)
    {
        TileBase tile = tilemap.GetTile(cell);
        return tile != null; // 수정이 필요할 수 있습니다. 타일이 통과 가능한지 여부에 따라 조건을 변경하세요.
    }
}

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Contains(T item)
    {
        return elements.Exists(pair => pair.Key.Equals(item));
    }
}
