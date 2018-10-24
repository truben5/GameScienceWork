using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider lane;

    // Current state for collider to interact with cars 
    public int state;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(state);
        //if (color.Equals("Stop"))
        //{
        //    Debug.Log("Red Light");
        //    other.transform.GetComponentInParent<CarMove>().isBraking = true;
        //}
        //Debug.Log(this.gameObject.name);
        //Debug.Log(other.name);
        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("leaving");
    }
}
