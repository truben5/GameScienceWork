using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

class GraphLoader : InfrastructureBehaviour
{

    [System.NonSerialized]
    public MapGraph graph;
    [System.NonSerialized]
    public List<KeyValPair> serializeDict;
    [System.NonSerialized]
    public  List<Vector3> wayPoints = new List<Vector3>();

    public bool IsReady { get; private set; }

    private int OSMCount = 0;

    IEnumerator Start()
    {
        //Debug.Log(map.name);
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

                if (way.Lanes == 2)
                {
                    //Debug.Log(i);
                    //GraphNode[] paths = new GraphNode[way.Lanes];
                    // Split central node into two lane nodes
                    Vector3 lane1C = currPos - (cross / 2);
                    Vector3 lane1N = nextPos - (cross / 2);

                    // Check if this is a valid connector to the multilane road
                    // (key) currPos -> (value) lane1C
                    // (key) nextPos -> (value) lane1N
                    // (key) lane1C -> (value) lane1C
                    // (key) lane1N -> (value) lane1N
                    bool validConnector = CheckMultiLaneConnection(currPos, lane1C);
                    if (!validConnector)
                    {
                        // If all nodes are present in graph then make sure that the currPos and nextPos key has the value of lane1C and lane1N respectively
                        if (graph.nodes.ContainsKey(currPos) && graph.nodes.ContainsKey(nextPos) && graph.nodes.ContainsKey(lane1C) && graph.nodes.ContainsKey(lane1N))
                        {
                            graph.nodes[currPos] = graph.nodes[lane1C];
                            graph.nodes[nextPos] = graph.nodes[lane1N];
                        }
                        // If not all present, call addNode for all of them. Setting the currPos and nextPos to the value of lane1C and lane1C
                        else
                        {
                            graph.AddNode(lane1C);
                            graph.AddNode(lane1N);
                            graph.AddNode(currPos, lane1C);
                            graph.AddNode(nextPos, lane1N);
                        }
                    }
                    UpdateGraph(lane1C, lane1N, i, way.NodeIDs.Count, 2);
                    //wayPoints.Add(lane1C);

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
                //yield return null;
            }

            yield return null;

        }

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


                //Debug.Log(i);
                //GraphNode[] paths = new GraphNode[way.Lanes];
                // Split central node into two lane nodes
                Vector3 lane1C = currPos - (cross / 2);
                Vector3 lane1N = nextPos - (cross / 2);

                bool validConnector = CheckMultiLaneConnection(currPos, lane1C);

                UpdateGraph(currPos, nextPos, i, way.NodeIDs.Count, 2);
                wayPoints.Add(currPos);
                //yield return null;
            }
            yield return null;
        }
        IsReady = true;
    }
    //    private void Start()
    //{

    //    graph = new MapGraph();

    //    // Store graph?
    //    //Debug.Log("storing graph");
    //    //StoreGraph();
        
    //    LoadNodes();
    //    Debug.Log("Completed node Loading");
    //    ConnectNeighbors();
    //    // Debug.Log("Starting simplifying graph");
    //    //graph.SimplifyGraph();
    //    //foreach (KeyValuePair<Vector3, GraphNode> point in graph.nodes)
    //    //{
    //    //wayPoints.Add(point.Value.position);
    //    //}
    //    //Debug.Log(graph.nodes.Count);
    //    IsReady = true;
    //}

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

                graph.nodes[currPos].AddNeighbor(nextPos);
                graph.nodes[nextPos].AddNeighbor(currPos);
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

                graph.nodes[currPos].AddNeighbor(nextPos);
                graph.nodes[nextPos].AddNeighbor(currPos);
            }
        }
    }

    // Check if connector to multilane road merges correctly
    // Makes sure the key is the multilane node but the location is the outer lane
    private bool CheckMultiLaneConnection(Vector3 key, Vector3 nodeLocation)
    {
        if (!graph.nodes.ContainsKey(key) || graph.nodes[key].position != nodeLocation)
        {
            return false;
        }
        return true;
    }

    //void OnDrawGizmos()
    //{
    //    if (IsReady == true)
    //    {
    //        //Debug.Log("making gizmos");
    //        Gizmos.color = Color.red;
    //        foreach (var point in graph.nodes)
    //        {
    //            Gizmos.DrawWireSphere(point.Value.position, 1f);
    //            foreach (var neighbor in graph.nodes[point.Value.position].neighbors)
    //            {
    //                Gizmos.DrawLine(point.Value.position, graph.nodes[neighbor].position);
    //            }
    //        }
    //    }
    //}

    void StoreGraph()
    {
        List<KeyValPair> serializedDict = new List<KeyValPair>();
        foreach (KeyValuePair<Vector3, GraphNode> pair in graph.nodes)
        {
            KeyValPair newPair = new KeyValPair(pair.Key, pair.Value.position, graph);
            serializedDict.Add(newPair);
        }

        FileStream fs = new FileStream("Assets/Resources/Graphs/" + map.name, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, serializedDict);
        fs.Close();
    }

    void LoadNodes()
    {
        serializeDict = new List<KeyValPair>();
        FileStream fs = new FileStream("Assets/Resources/Graphs/" + map.name, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            serializeDict = (List<KeyValPair>)formatter.Deserialize(fs);
            //serializeDict.ForEach(x => Debug.Log(x.connections));
            foreach (KeyValPair loaded in serializeDict)
            {
                Vector3 key = new Vector3(loaded.keyX, loaded.keyY, loaded.keyZ);
                Vector3 valVec = new Vector3(loaded.valX, loaded.valY, loaded.valZ);
                //GraphNode value = new GraphNode(valVec);
                GraphNode added = graph.AddNode(key, valVec);
                wayPoints.Add(added.position);
            }
        }
        catch (System.Runtime.Serialization.SerializationException e)
        {
            //Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            Debug.Log("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    void ConnectNeighbors()
    {
        foreach (KeyValPair pair in serializeDict)
        {
            foreach (KeyValPair connections in pair.connections)
            {
                Vector3 conKey = new Vector3(connections.keyX, connections.keyY, connections.keyZ);
                Debug.Log(conKey);
            }
        }
    }
    
}