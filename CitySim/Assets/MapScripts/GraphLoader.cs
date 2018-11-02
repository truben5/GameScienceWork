using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GraphLoader : InfrastructureBehaviour
{

    [System.NonSerialized]
    public MapGraph graph;
    public  List<Vector3> wayPoints = new List<Vector3>();

    public bool IsReady { get; private set; }

    private int OSMCount = 0;

    IEnumerator Start()
    {
        graph = new MapGraph();

        while (!map.IsReady)
        {
            yield return null;
        }
        // Create game object under map to store streets
        //GameObject allStreets = new GameObject();
        //allStreets.transform.parent = map.transform;
        //allStreets.name = "Roads";
        // TODO: Add Lanes
        foreach (var way in map.ways.FindAll((w) => { return w.IsRoad && !w.IsRailway; }))
        {
            Vector3 localOrigin = GetCenter(way);

            for (int i = 1; i < way.NodeIDs.Count; i++)
            {
                OSMNode p1 = map.nodes[way.NodeIDs[i - 1]];
                OSMNode p2 = map.nodes[way.NodeIDs[i]];


                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;
                //Vector3 s3 = Vector3.zero;

                //int avg = 1;
                Vector3 diff = Vector3.zero;
                diff = (s2 - s1).normalized;

                //Vector3 diff = ((s2 - s1) + (s3 - s2) / avg).normalized;
                var cross = Vector3.Cross(diff, Vector3.up) * 4.0f * way.Lanes; // Add lanes here

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;

                // calculates current and next position vector
                Vector3 currPos = p1 - map.bounds.Center;
                Vector3 nextPos = p2 - map.bounds.Center;

                if (way.Lanes ==2)
                {
                    //Debug.Log(i);
                    //GraphNode[] paths = new GraphNode[way.Lanes];
                    Vector3 lane1C = currPos - (cross / 2);
                    Vector3 lane1N = nextPos - (cross / 2);
                    UpdateGraph(lane1C, lane1N, i, way.NodeIDs.Count, 2);
                    wayPoints.Add(lane1C);

                    Vector3 lane2C = currPos + (cross / 2);
                    Vector3 lane2N = nextPos + (cross / 2);
                    UpdateGraph(lane2C, lane2N, i, way.NodeIDs.Count, 2);
                    wayPoints.Add(lane2C);
                }
                else
                {
                    UpdateGraph(currPos, nextPos, i, way.NodeIDs.Count, 2);
                    wayPoints.Add(currPos);
                }

                yield return null;

            }

        }
        IsReady = true;
        Debug.Log("Completed Graph Loading");
        //Debug.Log(graph.nodes.Count);
    }
    private void UpdateGraph(Vector3 currPos, Vector3 nextPos, int currI, int maxI, int direction)
    {
        GraphNode currNode;
        GraphNode nextNode;
        // Add next node to current node neighbors and current to next node neighbors
        if (direction == 2)
        {
            // Add current and next node to graph. Connect from curr -> next and cur <- next
            if (currI == 1)
            {
                graph.AddNode(currPos);
                graph.AddNode(nextPos);

                //Get graphNode reference
                currNode = graph.nodes[currPos];
                nextNode = graph.nodes[nextPos];

                graph.nodes[currPos].AddNeighbor(nextNode);
                graph.nodes[nextPos].AddNeighbor(currNode);
            }
            else if (currI > 1 && currI < maxI)
            {
                graph.AddNode(nextPos);
                if (!graph.nodes.ContainsKey(currPos))
                {
                    graph.AddNode(currPos);
                }
                currNode = graph.nodes[currPos];
                nextNode = graph.nodes[nextPos];

                graph.nodes[currPos].AddNeighbor(nextNode);
                graph.nodes[nextPos].AddNeighbor(currNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (IsReady == true)
        {
            //Debug.Log("making gizmos");
            Gizmos.color = Color.red;
            foreach (var point in graph.nodes)
            {
                Gizmos.DrawWireSphere(point.Key, 1f);
                foreach (var neighbor in graph.nodes[point.Key].neighbors)
                {
                    Gizmos.DrawLine(point.Key, neighbor.position);
                }
            }

        }
    }
}
