using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

class RoadMaker : InfrastructureBehaviour
{ 
    public Material roadMaterial;

    [System.NonSerialized]
    public MapGraph graph;
    //public  List<Vector3> wayPoints = new List<Vector3>();
    [System.NonSerialized]
    public List<Vector3> stopLights;

    public GameObject streetLightPrefab;

    public bool IsReady { get; private set; }

    private int OSMCount = 0;

    IEnumerator Start()
    {
        graph = new MapGraph();
        stopLights = new List<Vector3>();

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

            GameObject go = new GameObject();
            // Name road if name is available
            if (string.IsNullOrEmpty(way.Name))
            {
                go.name = "OSMWay" + OSMCount;
                OSMCount++;
            }
            else
            {
                go.name = way.Name;
            }
            // Set map to parent
            go.transform.parent = map.transform;

            Vector3 localOrigin = GetCenter(way);
            go.transform.position = localOrigin - map.bounds.Center;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            mr.material = roadMaterial;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

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
                // Adds current position to graph
                //graph.AddNode(currPos);
                //GraphNode currNode = graph.nodes[currPos];

                UpdateGraph(currPos, nextPos, i, way.NodeIDs.Count, 2);


                // Add node locations to stoplights
                if (p1.IsStreetLight)
                {
                    //Debug.Log("light");
                    stopLights.Add(currPos);
                }


                vectors.Add(v1);
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                if (i > 1)
                {
                    int idPrev4, idPrev3;
                    idPrev4 = vectors.Count - 5;
                    idPrev3 = vectors.Count - 6;

                    //Triangles to connect breaks
                    indices.Add(idx1);
                    indices.Add(idx2);
                    indices.Add(idPrev4);

                    indices.Add(idPrev3);
                    indices.Add(idx1);
                    indices.Add(idx2);
                }

                // First Triangle
                indices.Add(idx1);
                indices.Add(idx3);
                indices.Add(idx2);

                // Second
                indices.Add(idx3);
                indices.Add(idx4);
                indices.Add(idx2);

            }

            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indices.ToArray();

            

            NavMeshSurface surface = go.AddComponent<NavMeshSurface>();
            //surface.BuildNavMesh();

            yield return null;

        }
        GameObject streets = new GameObject();
        streets.name = "StreetLights";
        streets.transform.parent = map.transform;
        foreach(var light in stopLights)
        {

            GameObject streetLight = Instantiate(streetLightPrefab, new Vector3(light.x, 10, light.z), Quaternion.identity);
            streetLight.transform.parent = streets.transform;
        }
        IsReady = true;
        Debug.Log("Completed Road Rendering");
    }

    private void UpdateGraph(Vector3 currPos, Vector3 nextPos, int currI, int maxI, int direction)
    {
        GraphNode currNode;
        GraphNode nextNode;
        // Add next node to current node neighbors and current to next node neighbors
        if (direction == 2) {
            // Add current and next node to graph. Connect from curr -> next and cur <- next
            if(currI == 1)
            {
                graph.AddNode(currPos);
                graph.AddNode(nextPos);

                currNode = graph.nodes[currPos];
                nextNode = graph.nodes[nextPos];

                graph.nodes[currPos].AddNeighbor(nextNode);
                graph.nodes[nextPos].AddNeighbor(currNode);
            }
            else if (currI > 1 && currI < maxI)
            {
                graph.AddNode(nextPos);
                currNode = graph.nodes[currPos];
                nextNode = graph.nodes[nextPos];

                graph.nodes[currPos].AddNeighbor(nextNode);
                graph.nodes[nextPos].AddNeighbor(currNode);
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (IsReady == true)
    //    {
    //        //Debug.Log("making gizmos");
    //        Gizmos.color = Color.red;
    //        foreach (var point in graph.nodes)
    //        {
    //            Gizmos.DrawWireSphere(point.Key, 1f);
    //            foreach (var neighbor in graph.nodes[point.Key].neighbors)
    //            {
    //                Gizmos.DrawLine(point.Key, neighbor.position);
    //            }
    //        }

            //Gizmos.color = Color.yellow;
            //foreach(var point in stopLights)
            //{
            //    Gizmos.DrawWireSphere(point, 1f);
            //}
     //   }

    //}
}
