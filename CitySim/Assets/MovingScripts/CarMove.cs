using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMove : MonoBehaviour {

    //public Vector3 X = new Vector3();
    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start () {

        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
	}

    private void SetDestination()
    {
        Debug.Log(_destination);
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
