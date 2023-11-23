using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    [SerializeField] private GameObject WallNorth;
    [SerializeField] private GameObject WallEast;
    [SerializeField] private GameObject WallSouth;
    [SerializeField] private GameObject WallWest;



    public void DeleteNorth()
    {
        Destroy(WallNorth);
    }

    public void DeleteEast()
    {
        Destroy(WallEast);
    }

    public void DeleteSouth()
    {
        Destroy(WallSouth);
    }

    public void DeleteWest()
    {
        Destroy(WallWest);
    }


    public void SetGridPosition(int x, int y)
    {
        X = x;
        Y = y;
        var pos = Grid.GetTransformPosition(new Vector2Int(x,y));
        transform.position = pos;
    }
}
