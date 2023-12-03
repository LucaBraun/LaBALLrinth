using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillboxController : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeath = new();


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
            onDeath.Invoke();
        }
    }
}
