using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider lane;
    public GameObject lights;
    public Material stop;

    // Current state for collider to interact with cars 
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        string lightColor = lights.transform.GetChild(2).GetComponent<Renderer>().material.name;
        Debug.Log("light is:" + lightColor + " and inIntersection is " + inIntersection);
        //Debug.Log(lights.transform.GetChild(2).GetComponent<Renderer>().sharedMaterial);
        // If not in intersection already
        if (!inIntersection && lightColor.Equals("Stop (Instance)"))
        {
            Debug.Log("see red light");
            other.GetComponentInParent<CarMove>().inIntersection = true;
        }
        // If already in intersection then hitting other collider should have no effect
        //else 
        //{
        //    Debug.Log("already in inIntersection");
        //    inIntersection = false;
        //}
        
    }

    private void OnTriggerStay(Collider other)
    {
        bool inIntersection = other.GetComponentInParent<CarMove>().inIntersection;
        string lightColor = lights.transform.GetChild(0).GetComponent<Renderer>().material.name;
        Debug.Log("staying in");
        Debug.Log(inIntersection + " and " + lightColor);
        if (inIntersection && lightColor.Equals("Go (Instance)"))
        {
            Debug.Log("see green");
            other.GetComponentInParent<CarMove>().inIntersection = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leaving");
    }
}
