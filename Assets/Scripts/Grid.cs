using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [SerializeField] private int gridSizeX;
    [SerializeField] private int gridSizeY;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private LayerMask wallLayer;
    
    

    public static GameObject[,] GridArray;

    private static bool[,] visited;
    private List<Vector2Int> visitedCellsWithUnvisitedNeighbors = new List<Vector2Int>();


    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
        HuntAndKill();
    }


    private void InitializeGrid()
    {
        GridArray = new GameObject[gridSizeX,gridSizeY];
        visited = new bool[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GridArray[x, y] = Instantiate(nodePrefab, transform);
                GridArray[x,y].GetComponent<Node>().SetGridPosition(x, y);
            }
        }
    }

    private void HuntAndKill()
    {
        var currentPos = new Vector2Int(Random.Range(0, gridSizeX - 1), Random.Range(0, gridSizeY - 1));
        visited[currentPos.x, currentPos.y] = true;

        while (true)
        {
            
            while (true)
            {
                var oldPos = currentPos;
                currentPos = RandomWalk(currentPos);

                if (oldPos.Equals(currentPos))
                {
                    break;
                }

                Visit(currentPos);
                
                BreakWalls(currentPos, oldPos);
            }

            var endPos = currentPos;
            currentPos = FindNewStartPos();
            

            if (currentPos.Equals(new Vector2Int(-1, -1)))
            {
                break;
            }
        }

    }

    private void Visit(Vector2Int cell)
    {
        visited[cell.x, cell.y] = true;
                
        if (GetUnvisitedNeighbors(cell).Count > 0)
        {
            visitedCellsWithUnvisitedNeighbors.Add(cell);
        }
    }

    private Vector2Int RandomWalk(Vector2Int startFrom)
    {
        var neighbors = GetUnvisitedNeighbors(startFrom);

        if (neighbors.Count == 0)
        {
            return startFrom;
        }

        var index = Random.Range(0, neighbors.Count);

        return neighbors[index];
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int position)
    {
        var neighbors = new List<Vector2Int>();

        if (position.x > 0 && !visited[position.x-1, position.y])
        {
            neighbors.Add(new Vector2Int(position.x-1, position.y));
        }

        if (position.x < gridSizeX - 1 && !visited[position.x+1, position.y])
        {
            neighbors.Add(new Vector2Int(position.x+1, position.y));
        }

        if (position.y > 0 && !visited[position.x, position.y-1])
        {
            neighbors.Add(new Vector2Int(position.x, position.y-1));
        }

        if (position.y < gridSizeY - 1 && !visited[position.x, position.y + 1])
        {
            neighbors.Add(new Vector2Int(position.x, position.y+1));
        }

        return neighbors;
    }

    private Vector2Int FindNewStartPos()
    {
        visitedCellsWithUnvisitedNeighbors = PruneCellList();
        var length = visitedCellsWithUnvisitedNeighbors.Count;

        if (length  == 0)
        {
            return new Vector2Int(-1, -1);
        }
        
        var visitedCellIndex = Random.Range(0, length);
        var cell = visitedCellsWithUnvisitedNeighbors[visitedCellIndex];
        var unvisitedNeighbors = GetUnvisitedNeighbors(cell);
        var unvisitedNeighborCount = unvisitedNeighbors.Count;

        var targetCellIndex = Random.Range(0, unvisitedNeighborCount);
        //Debug.Log($"{targetCellIndex} : {unvisitedNeighborCount}");
        var targetCell = new Vector2Int(unvisitedNeighbors[targetCellIndex].x, unvisitedNeighbors[targetCellIndex].y);

        BreakWalls(cell, targetCell);
        if (unvisitedNeighborCount <= 1)
        {
            visitedCellsWithUnvisitedNeighbors.RemoveAt(visitedCellIndex);
        }
        
        Visit(targetCell);

        return targetCell;
    }

    private List<Vector2Int> PruneCellList()
    {
        var prunedList = new List<Vector2Int>();
        foreach (var cell in visitedCellsWithUnvisitedNeighbors)
        {
            if (GetUnvisitedNeighbors(cell).Count != 0)
            {
                prunedList.Add(cell);
            }
        }

        return prunedList;
    }

    private void BreakWalls(Vector2Int between, Vector2Int these)
    {

        var betweenNode = GridArray[between.x, between.y].GetComponent<Node>();
        var theseNode = GridArray[these.x, these.y].GetComponent<Node>();
        var direction = these - between;

        var north = new Vector2Int(0, 1);
        var south = new Vector2Int(0, -1);
        var east = new Vector2Int(1, 0);

        if (direction.Equals(north))
        {
            betweenNode.DeleteNorth();
            theseNode.DeleteSouth();
        }
        else if (direction.Equals(south))
        {
            betweenNode.DeleteSouth();
            theseNode.DeleteNorth();
        }
        else if (direction.Equals(east))
        {
            betweenNode.DeleteEast();
            theseNode.DeleteWest();
        }
        else // west
        {
            betweenNode.DeleteWest();
            theseNode.DeleteEast();
        }
    }

    //does not work for some reason.... raycast has forsaken me
    //i wanted to save time and instead created a lot of weird ass problems
    //my brother in christ i work in unity full time
    //this shit should not happen to me
    private void BreakWallsRetarded(Vector2Int between, Vector2Int these)
    {
        Vector3 fromPos = GetTransformPosition(between);
        Vector3 toPos = GetTransformPosition(these);
        Vector3 direction = toPos - fromPos;
        fromPos += Vector3.up;

        var hits = Physics.RaycastAll(fromPos, direction.normalized, direction.magnitude);

        if (Physics.Linecast(fromPos, fromPos + direction, out var hit1))
        {
            Debug.DrawLine(hit1.point, hit1.point+Vector3.up*10, Color.yellow, 10);
        }
        
        Debug.DrawLine(fromPos, fromPos+direction, Color.red, 10f);
        foreach (var hit in hits)
        {
            Debug.DrawLine(hit.point, hit.point+Vector3.up*10, Color.yellow, 10);
            // Destroy(hit.transform.gameObject);
        }
    }

    public static Vector3 GetTransformPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * 2f, 0, gridPos.y * 2f);
    }
}