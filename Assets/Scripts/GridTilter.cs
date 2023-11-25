using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridTilter : MonoBehaviour
{
    [SerializeField] private float maxAngleX;
    [SerializeField] private float maxAngleY;
    
    [Header("Input")]
    [SerializeField] private InputActionReference tiltMouse;
    [SerializeField] private InputActionReference tiltGamepad;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        tiltMouse.action.performed += TiltMouse;
        // tiltGamepad.action.performed += TiltGamepad;
    }

    private void OnDisable()
    {
        tiltMouse.action.performed -= TiltMouse;
        // tiltGamepad.action.performed -= TiltGamepad;
    }

    private void TiltMouse(InputAction.CallbackContext context)
    {
        var mousePos = context.ReadValue<Vector2>();
        Debug.Log(mousePos);
        // var screenCenter = new Vector2(cam.transform.position.x, cam.transform.position.z);
        // var lookDirection = ((Vector2)cam.ScreenToWorldPoint(mousePos) - screenCenter);
        Tilt(cam.ScreenToWorldPoint(mousePos));
    }

    private void TiltGamepad(InputAction.CallbackContext context)
    {
        var lookDirection = context.ReadValue<Vector2>() * 20;
    }


    private void Tilt(Vector3 worldPoint)
    {
        Debug.Log(worldPoint);

        var clampedZ = worldPoint.z > maxAngleX ? maxAngleX : worldPoint.z;
        clampedZ = clampedZ < -maxAngleX ? -maxAngleX : clampedZ;
        var clampedX = worldPoint.x > maxAngleY ? maxAngleY : worldPoint.x;
        clampedX = clampedX < -maxAngleY ? -maxAngleY : clampedX;

        transform.rotation = Quaternion.Euler(clampedZ, 0, -clampedX);
    }
    
    
    
}
