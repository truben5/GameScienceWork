using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public Vector3 position { get; private set; }
    public List<GraphEdge> neighbors { get; private set; }

    public GraphNode(Vector3 v)
    {
        position = v;
        neighbors = new List<GraphEdge>();
    }

    // Checks if node is already a neigbor
    public bool ContainsNeighbor(GraphNode checkNode)
    {
        foreach (GraphEdge edge in neighbors)
        {
            if (edge.end == checkNode)
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
            GraphEdge newEdge = new GraphEdge(this, newNode);
            neighbors.Add(newEdge);
        }
    }
}
