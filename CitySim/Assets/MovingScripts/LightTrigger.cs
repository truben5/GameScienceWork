using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider lane;
    public GameObject trafflight;

    private void OnTriggerEnter(Collider other)
    {

        string color = trafflight.transform.GetChild(2).GetComponent<Renderer>().material.name;
        Debug.Log(color);
        if (color.Equals("Stop"))
        {
            Debug.Log("Red Light");
            other.transform.GetComponentInParent<CarMove>().isBraking = true;
        }
        //Debug.Log(this.gameObject.name);
        //Debug.Log(other.name);
        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leaving");
    }
}
