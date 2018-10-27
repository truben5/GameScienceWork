using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge
{

    public GraphNode start { get; private set; }
    public GraphNode end { get; private set; }
    public float distance { get; private set; }

    public GraphEdge(GraphNode _n1, GraphNode _n2)
    {
        start = _n1;
        end = _n2;
        CalculateDistance(start, end);
    }

    // Calculates path between two graph nodes
    private void CalculateDistance(GraphNode n1, GraphNode n2)
    {
        distance = Vector3.Distance(n1.position, n2.position);
    }

}
