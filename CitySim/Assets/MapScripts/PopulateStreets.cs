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

    Random random = new Random();
    List<GraphNode> path;
    bool x = false;

    // Use this for initialization
    IEnumerator Start()
    {
        loader = GetComponent<GraphLoader>();
        while (!loader.IsReady)
        {
            //Debug.Log("not ready");
            yield return null;
        }

        Vector3 start = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
        Vector3 end = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
        //Debug.Log("start: " + start + " end: " + end);
        v1.transform.position = start;
        v2.transform.position = end;
        path = loader.graph.ShortestPath(loader.graph.nodes[start], loader.graph.nodes[end]);
        //Debug.Log(path.Count);
        x = true;
        //Debug.Log("Population starting");
        //Debug.Log(loader.graph.nodes.Count);
        //for (int i = 0; i < carPopulation; i++)
        //{
        //    Vector3 pos = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
        //pos.y += 3;
        //    GameObject Car = Instantiate(carPrefab, pos, Quaternion.identity);
        //    Car.GetComponent<CarMove>().destination = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];
        //    Car.GetComponent<CarMove>().destination = loader.wayPoints[random.Next(loader.wayPoints.Count - 1)];

        //}
    }

    void OnDrawGizmos()
    {
        if (x)
        {
            //Debug.Log("making gizmos");
            Gizmos.color = Color.yellow;
            foreach (var point in path)
            {
                //Debug.Log("drawing " + point.position);
                Gizmos.DrawWireSphere(point.position, 1f);
                //foreach (var neighbor in graph.nodes[point.Key].neighbors)
                //{
                //    Gizmos.DrawLine(point.Key, neighbor.position);
                //}
            }
        }
    }       
}
