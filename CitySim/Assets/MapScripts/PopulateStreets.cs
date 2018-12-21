using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

class PopulateStreets : MonoBehaviour {

    public int carPopulation = 100;
    public GraphLoader loader;
    public GameObject carPrefab;

    public GameObject v1;
    public GameObject v2;

    List<Vector3> path = new List<Vector3>();

    Random random = new Random();
    //List<GraphNode> path;
    //bool x = false;

    // Use this for initialization
    IEnumerator Start()
    {
        loader = GetComponent<GraphLoader>();
        while (!loader.IsReady)
        {
            //Debug.Log("not ready");
            yield return null;
        }

        int totalPoints = loader.wayPoints.Count;

        // Number of cars cannot exceed number of waypoints
        if (carPopulation >= totalPoints)
        {
            carPopulation = totalPoints -  (carPopulation/10);
        }


        Dictionary<Vector3, bool> selectedNodes = new Dictionary<Vector3, bool>();
        //Debug.Log("total cars is: " + carPopulation);
        for (int i = 0; i < carPopulation; i++)
        {
           // Debug.Log("i is: " + i);
            Vector3 start = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
            if (selectedNodes.ContainsKey(start))
            {
                while (selectedNodes.ContainsKey(start))
                {
                    start = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
                }
            }

            Vector3 end = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
            if (start == end)
            {
                while (start == end)
                {
                    end = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
                }
            }
            selectedNodes.Add(start, true);
            //selectedNodes.Add(end, true);

            GameObject Car = Instantiate(carPrefab, start, Quaternion.identity);
            path = loader.graph.ShortestPath(loader.graph.nodes[start], loader.graph.nodes[end]);
            Car.GetComponent<CarMove>().wayPoints = path;
           // Car.GetComponent<CarMove>().destination = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];

        }
    }

    // Draws path
    //void OnDrawGizmos()
    //{

    //    //Debug.Log("making gizmos");
    //    Gizmos.color = Color.yellow;
    //    foreach (var point in path)
    //    {
    //        //Debug.Log("drawing " + point.position);
    //        Gizmos.DrawWireSphere(point, 1f);
    //        //foreach (var neighbor in graph.nodes[point.Key].neighbors)
    //        //{
    //        //    Gizmos.DrawLine(point.Key, neighbor.position);
    //        //}
    //    }

    //}
}
