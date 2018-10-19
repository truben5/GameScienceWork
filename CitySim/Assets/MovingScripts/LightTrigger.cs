using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {

    public Collider FBlane;
    public Collider RLlane;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name);
        Debug.Log(other.name);
    }
}
