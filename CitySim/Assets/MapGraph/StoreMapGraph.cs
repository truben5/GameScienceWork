using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(RoadMaker))]
public class StoreGraph : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    private List<Vector3> wayPoints;

    void Awake()
    {
        wayPoints = GetComponent<RoadMaker>().wayPoints;
        Debug.Log(wayPoints.Count);
    }

}
