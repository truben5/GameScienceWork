using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph {

    public Dictionary<Vector3, GraphNode> nodes;

    public MapGraph()
    {
        nodes = new Dictionary<Vector3, GraphNode>();
    }

    public void AddNode(Vector3 v)
    {
        if (!nodes.ContainsKey(v))
        {
            GraphNode vNode = new GraphNode(v);
            nodes[v] = vNode;
        }
    }

}
