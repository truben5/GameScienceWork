using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : IComparable<GraphNode>
{
    public Vector3 position { get; private set; }
    public List<Vector3> neighbors { get; private set; }

    public GraphNode(Vector3 v)
    {
        position = v;
        neighbors = new List<Vector3>();
    }

    // Checks if node is already a neigbor
    public bool ContainsNeighbor(Vector3 checkNode)
    {
        foreach (Vector3 neighbor in neighbors)
        {
            if (neighbor == checkNode)
            {
                return true;
            }
        }
        return false;
    }

    // Add neighbor if not present
    public void AddNeighbor(Vector3 newNode)
    {
        if (!ContainsNeighbor(newNode))
        {
            neighbors.Add(newNode);
        }
    }

    // Remove neighbor if present
    public void RemoveNeighbor(Vector3 toRemove)
    {
        if (ContainsNeighbor(toRemove))
        {
            neighbors.Remove(toRemove);
        }
    }

    public float GetDistance(GraphNode endNode)
    {
        float dist = Vector3.Distance(position, endNode.position);
        return dist;
    }

    public int CompareTo(GraphNode newNode)
    {

        if (position == newNode.position)
        {
            return 0;
        }
        if ((position.x < newNode.position.x) || ((position.x == newNode.position.x) && (position.y < newNode.position.y)))
        {
            return -1;
        }
        return 1;
    }
}
