using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject Goal;
    
    private GameObject ballInstance;
    private Grid grid;


    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void StartGame()
    {
        Reset();
        ballInstance = Instantiate(ball,grid.GridArray[0,0].transform.position + Vector3.up*3f, Quaternion.identity);
        ballInstance.transform.parent = grid.transform;
    }

    public void Reset()
    {
        if (ballInstance != null)
        {
            Destroy(ballInstance);
        }
    }
}
