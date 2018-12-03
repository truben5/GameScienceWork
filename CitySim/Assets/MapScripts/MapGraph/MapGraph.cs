using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            //Debug.Log("Trying to add " + v);
            GraphNode vNode = new GraphNode(v);
            nodes.Add(v, vNode);
            //nodes[v] = vNode;
        }
    }

    public void AddNode(Vector3 key, Vector3 pair)
    {
        if (!nodes.ContainsKey(key))
        {
            GraphNode newNode = new GraphNode(pair);
            nodes.Add(key,newNode);
        }
        //else if(!nodes.ContainsKey(pair))
        //{
        //    //GraphNode newNode = new GraphNode(pair);
        //    AddNode(pair);
        //    nodes[key] = nodes[pair];
        //}
        //else
        //{
        //    nodes[key] = nodes[pair];
        //}
    }

    public List<Vector3> ShortestPath(GraphNode start, GraphNode end)
    {
        Dictionary<GraphNode, float> distanceToEnd = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, float> startDist = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, bool> visited = new Dictionary<GraphNode, bool>();
        Dictionary<GraphNode, GraphNode> closest = new Dictionary<GraphNode, GraphNode>();
        foreach (var node in nodes)
        {
            distanceToEnd[node.Value] = node.Value.GetDistance(end);
            visited[node.Value] = false;
        }
        closest = AStarSearch(start, end, distanceToEnd, visited, closest);
        
        List<Vector3> shortestPath = BuildPath(end,closest);
        return shortestPath;
    }

    public Dictionary<GraphNode, GraphNode> AStarSearch(GraphNode start, GraphNode end, Dictionary<GraphNode, 
        float> endDist, Dictionary<GraphNode, bool> visited, Dictionary<GraphNode, GraphNode> closest)
    {
        Dictionary<GraphNode, float> startDist = new Dictionary<GraphNode, float>();
        startDist[start] = 0;
        List<GraphNode> queue = new List<GraphNode>();
        queue.Add(start);
        while (queue.Count > 0)
        {
            //Debug.Log("queueueueue");
            // Sort queue by starting distance
            queue = queue.OrderBy( x => startDist[x] + endDist[x]).ToList();
            GraphNode node = queue.First();
            //Debug.Log(node.position);
            queue.Remove(node);
            if (node == end)
            {
                return closest;
            }
            // g is the cost to get to the current node nad heuristic is euclidian distance between node youre looking at and goal
            // f = g + h
            // Loop through sorted list of neighbors by ending distance
            foreach (GraphNode neighbor in node.neighbors.OrderBy(x => endDist[x]))
            {
                //Debug.Log("neighbor is: " + neighbor.position);
                if (visited[neighbor] == true)
                {
                    continue;
                }
                float connectionDist = Vector3.Distance(neighbor.position, node.position);
                if (!startDist.ContainsKey(neighbor) || startDist[node] + connectionDist < startDist[neighbor])
                {
                    startDist[neighbor] = startDist[node] + connectionDist;
                    closest[neighbor] = node;

                    if (!queue.Contains(neighbor))
                    {
                        queue.Add(neighbor);
                    }
                }

            }
            visited[node] = true;
        }
        return closest;
    }

    public List<Vector3> BuildPath(GraphNode currNode, Dictionary<GraphNode,GraphNode> closest)
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(currNode.position);
        while (closest.ContainsKey(currNode))
        {
            path.Add(closest[currNode].position);
            currNode = closest[currNode];
        }
        path.Reverse();
        return path;
    }

    public List<GraphNode> FindNodesToDelete()
    {
        List<GraphNode> toDelete = new List<GraphNode>();
        Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();
        foreach (KeyValuePair<Vector3, GraphNode> mapping in nodes)
        {
            if (mapping.Value.neighbors != null)
            {
                foreach (GraphNode neighbor in mapping.Value.neighbors)
                {
                    foreach (GraphNode nextNeighbor in neighbor.neighbors)
                    {
                        bool isColinear = Colinear(mapping.Value, neighbor, nextNeighbor);
                        if (isColinear == true && neighbor.neighbors.Count == 2)
                        {
                            toDelete.Add(neighbor);
                        }
                    }
                }
            }
        }
        return toDelete;
    }

    public void RemoveNode(GraphNode node)
    {
        Vector3[] keys = new Vector3[nodes.Keys.Count];
        nodes.Keys.CopyTo(keys, 0);
        if (nodes.ContainsValue(node))
        {
            nodes.Remove(node.position);
        }
        for(int i = 0; i < keys.Length; i++)
        {
            //Vector3 vKey = keys[i];
            //if (nodes.ContainsKey(vKey))
            //{

                List<GraphNode> neighborNeighbors = nodes[keys[i]].neighbors;
                // For each neighbors neighbor
                for (int j = 0; j < neighborNeighbors.Count; j++)
                {
                    if (neighborNeighbors.Contains(node))
                    {
                        neighborNeighbors.Remove(node);
                    }
                }
            //}
        }
    }

    public bool Colinear(GraphNode first, GraphNode second, GraphNode third)
    {
        float a = first.position.x * (second.position.y - third.position.y)+
                    second.position.x * (third.position.y - first.position.y) +
                    third.position.x * (first.position.y - second.position.y);
        if (a == 0)
        {
            return true;
        }
        return false;
    }

    public void SimplifyGraph()
    {
        List<GraphNode> toDelete = FindNodesToDelete();
        foreach (GraphNode node in toDelete)
        {
            RemoveNode(node);
        }
    }

}
