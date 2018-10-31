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
            GraphNode vNode = new GraphNode(v);
            nodes[v] = vNode;
        }
    }

    public List<GraphNode> ShortestPath(GraphNode start, GraphNode end)
    {
        Dictionary<GraphNode, float> distanceToEnd = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, float> startDist = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, bool> visited = new Dictionary<GraphNode, bool>();
        foreach (var node in nodes)
        {
            distanceToEnd[node.Value] = node.Value.GetDistance(end);
            visited[node.Value] = false;
        }
        AStarSearch(start, end, distanceToEnd, visited, startDist);
        var shortestPath = new List<GraphNode>();
        return shortestPath;
    }

    public void AStarSearch(GraphNode start, GraphNode end, Dictionary<GraphNode, float> endDist, Dictionary<GraphNode, 
        bool> visited, Dictionary<GraphNode, float>startDist)
    {
        startDist[start] = 0;
        List<GraphNode> queue = new List<GraphNode>();
        queue.Add(start);
        while (queue.Count > 0)
        {
            // Sort queue by starting distance
            queue = queue.OrderBy( x => startDist[x]).ToList();
            GraphNode node = queue.First();
            queue.Remove(node);
            // Loop through sorted list of neighbors by ending distance
            foreach(GraphNode neighbor in node.neighbors.OrderBy(x => endDist[x]))
            {
                if (visited[neighbor] == true)
                {
                    continue;
                }
                if (!startDist.ContainsKey(neighbor))
                {

                }

            }
            visited[node] = true;
            if (node == end)
            {
                return;
            }
        }
    }

}
