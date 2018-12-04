using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValPair{

    public  float keyX;
    public float keyY;
    public float keyZ;

    public float valX;
    public float valY;
    public float valZ;

    public List<KeyValPair> connections;

    public KeyValPair(Vector3 key)
    {
        keyX = key.x;
        keyY = key.y;
        keyZ = key.z;
    }

    public KeyValPair(Vector3 key, Vector3 val, MapGraph graph)
    {
        keyX = key.x;
        keyY = key.y;
        keyZ = key.z;

        valX = val.x;
        valY = val.y;
        valZ = val.z;

        connections = new List<KeyValPair>();

        foreach (Vector3 neighbor in graph.nodes[key].neighbors)
        {
            // go through each key value pair and store the key
            foreach (KeyValuePair<Vector3, GraphNode> pair in graph.nodes)
            {
                if (pair.Value.Equals(neighbor))
                {
                    KeyValPair pseudoConnect = new KeyValPair(pair.Key);
                    connections.Add(pseudoConnect);
                    break;
                }
            }
        }
    }
}
