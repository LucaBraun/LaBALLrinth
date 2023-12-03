using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder;

public class Node : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    [SerializeField] private GameObject WallNorth;
    [SerializeField] private GameObject WallEast;
    [SerializeField] private GameObject WallSouth;
    [SerializeField] private GameObject WallWest;
    [SerializeField] private GameObject floor;


    private bool hasNorth = true;
    private bool hasEast = true;
    private bool hasSouth = true;
    private bool hasWest = true;

    private Grid grid;

    private void Awake()
    {
        grid = FindFirstObjectByType<Grid>();
    }


    public void DeleteNorth()
    {
        hasNorth = false;
        Destroy(WallNorth);
    }

    public void DeleteEast()
    {
        hasEast = false;
        Destroy(WallEast);
    }

    public void DeleteSouth()
    {
        hasSouth = false;
        Destroy(WallSouth);
    }

    public void DeleteWest()
    {
        hasWest = false;
        Destroy(WallWest);
    }


    public List<Vector2Int> GetWallDirections()
    {
        var wallDirections = new List<Vector2Int>();

        if (hasNorth)
        {
            wallDirections.Add(DirectionConstants.north);
        }

        if (hasEast)
        {
            wallDirections.Add(DirectionConstants.east);
        }

        if (hasSouth)
        {
            wallDirections.Add(DirectionConstants.south);
        }

        if (hasWest)
        {
            wallDirections.Add(DirectionConstants.west);
        }

        return wallDirections;
    }

    public bool IsDeadEnd()
    {
        var wallCount = GetWallDirections().Count;

        if (wallCount == 0)
        {
            return false;
        }

        switch (wallCount)
        {
            case 1 when X == 0 && Y == 0:
            case 1 when X == 0 && Y == grid.GridSizeY - 1:
            case 1 when X == grid.GridSizeX - 1 && Y == 0:
            case 1 when X == grid.GridSizeX - 1 && Y == grid.GridSizeY - 1:
                return true;
        }

        if (wallCount == 2 && (X == 0 || X == grid.GridSizeX - 1))
        {
            return true;
        }

        if (wallCount == 2 && (Y == 0 || Y == grid.GridSizeY - 1))
        {
            return true;
        }
        
        return wallCount == 3;
    }


    public void SetGridPosition(int x, int y)
    {
        X = x;
        Y = y;

        //Handling the EDGE cases hahaha get it?
        if (x == 0)
        {
            hasWest = false;
        }
        
        if (x == grid.GridSizeX - 1)
        {
            hasEast = false;
        }
        
        if (y == 0)
        {
            hasSouth = false;
        }
        
        if (y == grid.GridSizeY - 1)
        {
            hasNorth = false;
        }

        var pos = grid.GetTransformPosition(new Vector2Int(x,y));
        transform.position = pos;
    }

    public float GetNodeExtents()
    {
        return floor.GetComponent<BoxCollider>().bounds.extents.x;
    }
}
