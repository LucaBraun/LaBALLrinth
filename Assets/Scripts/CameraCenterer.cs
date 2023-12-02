using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraCenterer : MonoBehaviour
{
    private Camera orthoCam;
    private Grid grid;

    private void Awake()
    {
        orthoCam = GetComponent<Camera>();
        grid = FindFirstObjectByType<Grid>();
    }

    private void Start()
    {
        CenterCamera();
    }


    public void CenterCamera()
    {
        transform.position = CalculateCameraPosition();
        orthoCam.orthographicSize = grid.NodeExtents * grid.GridSizeY * 2f;
    }
    
    public Vector3 CalculateCameraPosition()
    {
        var x = (grid.GridSizeX - 1) * grid.NodeExtents;
        var z = (grid.GridSizeY - 1) * grid.NodeExtents;
        return new Vector3(x, 100f, z)- new Vector3(grid.GridSizeX*grid.NodeExtents, 0, grid.GridSizeY * grid.NodeExtents);
    }
}
