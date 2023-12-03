using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridTilter : MonoBehaviour
{
    [SerializeField] private float maxAngle;

    [SerializeField] private float rotationSpeedFactor = 0.01f;
    
    
    [Header("Input")]
    [SerializeField] private InputActionReference tiltMouse;
    [SerializeField] private InputActionReference tiltGamepad;

    private Camera cam;
    private Quaternion rotationZ;
    private Quaternion rotationX;

    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = transform.rotation;
        Quaternion.Normalize(startRotation);
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        var targetRotation = transform.rotation * rotationZ;
        targetRotation = ClampAngleOnAxis(targetRotation, 2, -maxAngle, maxAngle);
        targetRotation = ClampAngleOnAxis(targetRotation, 1, 0, 0);

        transform.rotation = targetRotation;

        targetRotation = rotationX * transform.rotation;
        targetRotation = ClampAngleOnAxis(targetRotation, 0, -maxAngle, maxAngle);
        targetRotation = ClampAngleOnAxis(targetRotation, 1, 0, 0);

        transform.rotation = targetRotation;
    }
    private void OnEnable()
    {
        tiltMouse.action.performed += TiltMouse;
    }

    private void OnDisable()
    {
        tiltMouse.action.performed -= TiltMouse;
    }

    private void TiltMouse(InputAction.CallbackContext context)
    {
        var mousePos = context.ReadValue<Vector2>();
        Tilt(cam.ScreenToWorldPoint(mousePos));
    }

    private void TiltGamepad(InputAction.CallbackContext context)
    {
        var lookDirection = context.ReadValue<Vector2>() * 20;
    }


    private void Tilt(Vector3 worldPoint)
    {
        // Debug.Log(worldPoint);
        worldPoint = worldPoint.normalized;
        worldPoint *= rotationSpeedFactor;



        rotationZ = Quaternion.Euler(0f,0f,-worldPoint.x);
        rotationX = Quaternion.Euler(worldPoint.z, 0f, 0f);
    }
    
    
    //copied from https://forum.unity.com/threads/object-rotation-clamping.922145/
    //the best method i ever stole from the internet. This will make things so much easier down the line
    //Quaternions are voodoo magic
    private Quaternion ClampAngleOnAxis(Quaternion q, int axis, float minAngle, float maxAngle)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
 
        var angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q[axis]);
 
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
 
        q[axis] = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
 
        return q;
    }
    
    
    
    
    
}
