using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorColapser : MonoBehaviour
{

    [SerializeField] private GameObject floor;
    
    public static UnityEvent OnFloorCollapse = new();
    // Start is called before the first frame update


    private void OnTriggerExit(Collider other)
    {
        if (floor != null && other.tag == "Player")
        {
            Destroy(floor);
            OnFloorCollapse.Invoke();
        }
    }
}
