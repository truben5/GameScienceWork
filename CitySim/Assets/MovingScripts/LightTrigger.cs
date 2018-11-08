using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider lane;
    public GameObject streetLight;
    private int lightState;

    // Current state for collider to interact with cars 
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        /* Should only do this if trigger is 0 */


        //Debug.Log("hit trigger");
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        //Debug.Log(inIntersection);
        //Debug.Log(other.GetComponent)

        // Determine light state
        CheckLightState();

        // If not in intersection already 
        if (!inIntersection && lightState == 0)
        {
            Debug.Log("see red light");
            other.GetComponentInParent<CarMove>().triggerCount++;
            other.GetComponentInParent<CarMove>().inIntersection = true;
        }
        // If already in intersection then hitting other collider should have no effect
        else 
        {
            other.GetComponentInParent<CarMove>().triggerCount++;
            other.GetComponentInParent<CarMove>().inIntersection = false;
        }
        Debug.Log("local var is: " + inIntersection);
        Debug.Log("inIntersection is: " + other.GetComponentInParent<CarMove>().inIntersection);
        
    }

    private void OnTriggerStay(Collider other)
    {
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        CheckLightState();
        //string lightColor = lights.transform.GetChild(0).GetComponent<Renderer>().material.name;
        //Debug.Log("staying in");
        //Debug.Log(lightState);
        if (inIntersection && lightState == 2)
        {
            Debug.Log("see green");
            other.GetComponentInParent<CarMove>().inIntersection = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("leaving");

    }

    private void CheckLightState()
    {
        // Determine light state based on collider name
        if (this.name[0].Equals("L") || this.name[0].Equals('R'))
        {
            lightState = streetLight.GetComponent<LightChange>().RLmode;
        }
        else
        {
            lightState = streetLight.GetComponent<LightChange>().FBMode;
        }
    }
}
