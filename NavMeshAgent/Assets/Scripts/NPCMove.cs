using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour {

    /*
     * Original code to get to specific point on map
     * 
     **/

    //   [SerializeField]
    //   Transform _destination;

    //   NavMeshAgent _navMeshAgent;

    //// Use this for initialization
    //void Start () {
    //       _navMeshAgent = this.GetComponent<NavMeshAgent>();

    //       if (_navMeshAgent == null)
    //       {
    //           Debug.LogError("navMesh agent is not attached to " + gameObject.name);
    //       }
    //       else
    //       {
    //           SetDestination();
    //       }

    //}

    //   private void SetDestination()
    //   {
    //       if (_destination != null)
    //       {
    //           Vector3 targetVector = _destination.transform.position;
    //           //Debug.Log(targetVector);
    //           _navMeshAgent.SetDestination(targetVector);
    //       }
    //   }

    /*
     * On click casts a ray from mouse position 
     * and sets the location it hit to be the new destination for the agent
     **/
    public Camera cam;
    public NavMeshAgent agent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
