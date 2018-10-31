using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public Vector3 position { get; private set; }
    public List<GraphNode> neighbors { get; private set; }

    public GraphNode(Vector3 v)
    {
        position = v;
        neighbors = new List<GraphNode>();
    }

    // Checks if node is already a neigbor
    public bool ContainsNeighbor(GraphNode checkNode)
    {
        foreach (GraphNode neighbor in neighbors)
        {
            if (neighbor == checkNode)
            {
                return true;
            }
        }
        return false;
    }

    // Add neighbor if not present
    public void AddNeighbor(GraphNode newNode)
    {
        if (!ContainsNeighbor(newNode))
        {
            neighbors.Add(newNode);
        }
    }

    public float GetDistance(GraphNode endNode)
    {
        float dist = Vector3.Distance(position, endNode.position);
        return dist;
    }
}
