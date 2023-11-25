using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    private GameObject ballInstance;
    private Grid grid;


    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void StartGame()
    {
        Reset();
        ballInstance = Instantiate(ball,grid.GridArray[0,0].transform.position + Vector3.up*0.5f, Quaternion.identity);
    }

    public void Reset()
    {
        if (ballInstance != null)
        {
            Destroy(ballInstance);
        }
    }
}
