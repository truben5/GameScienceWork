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

        if (other.name.Equals("Rear"))
        {
            return;
        }
        //Debug.Log("hit trigger");
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        //Debug.Log(inIntersection);
        //Debug.Log(other.GetComponent)

        // Determine light state
        CheckLightState();
        //Debug.Log(lightState);

        // If not in intersection already 
        //Debug.Log("light state is: " + lightState + " and inIntersection is: " + inIntersection);
        if (!inIntersection && lightState == 0)
        {
            Debug.Log("is red light");
            other.GetComponentInParent<CarMove>().triggerCount++;
            other.GetComponentInParent<CarMove>().inIntersection = true;
        }
        // If already in intersection then hitting other collider should have no effect
        else 
        {
            Debug.Log("is yellow or green light");
            other.GetComponentInParent<CarMove>().triggerCount++;
            other.GetComponentInParent<CarMove>().inIntersection = false;
        }
        Debug.Log("inIntersection is: " + other.GetComponentInParent<CarMove>().inIntersection);
        //Debug.Log(other.GetComponentInParent<CarMove>().triggerCount);
        
    }

    private void OnTriggerStay(Collider other)
    {
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        CheckLightState();
        //string lightColor = lights.transform.GetChild(0).GetComponent<Renderer>().material.name;
        //Debug.Log("staying in and lightState is: " + lightState);
        //Debug.Log(lightState);
        if (inIntersection && lightState == 2)
        {
            //Debug.Log("light is green");
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
        if (this.name[0].Equals('L') || this.name[0].Equals('R'))
        {
            
            lightState = streetLight.GetComponent<LightChange>().RLmode;
        }
        else if (this.name[0].Equals('F') || this.name[0].Equals('B'))
        {
            lightState = streetLight.GetComponent<LightChange>().FBMode;
        }
    }
}
