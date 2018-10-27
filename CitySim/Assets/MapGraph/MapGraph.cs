using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph {

    public List<GraphNode> nodes;

    public bool ContainsNode(GraphNode checkNode)
    {
        return nodes.Contains(checkNode);
    }

    public void AddNode(GraphNode newNode)
    {
        if (!ContainsNode(newNode))
        {
            nodes.Add(newNode);
        }
    }

}
