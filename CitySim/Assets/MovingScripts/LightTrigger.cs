using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider lane;

    // Current state for collider to interact with cars 
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        //bool inIntersection = other.GetComponent<CarMove>().inIntersection;
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        // If not in intersection already
        if (!inIntersection)
        {
            inIntersection = true;
            
        }
        // If already in intersection then hitting other collider should have no effect
        else
        {
            inIntersection = false;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leaving");
    }
}
