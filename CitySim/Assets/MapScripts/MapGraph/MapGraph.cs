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

}
