using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(RoadMaker))]
public class StoreMapGraph : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    //private List<Vector3> wayPoints; 
    private RoadMaker roads;
    private MapGraph graph = new MapGraph();
    
    void Awake()
    {
        roads = GetComponent<RoadMaker>();
    }
    IEnumerator Start()
    {
        while (!roads.IsReady)
        {
            yield return null;
        }
        List<Vector3> wayPoints = GetComponent<RoadMaker>().wayPoints;
        //Debug.Log(wayPoints.Count);
        // Create node for each vector and add it to map
        foreach (Vector3 v in wayPoints)
        {
            GraphNode node = new GraphNode(v);
            //graph.AddNode(node);
        }

    }


}
