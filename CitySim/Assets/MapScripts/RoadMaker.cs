using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

class RoadMaker : InfrastructureBehaviour
{ 
    public Material roadMaterial;

    IEnumerator Start()
    {
        while (!map.IsReady)
        {
            yield return null;
        }

        // TODO: Add Lanes
        foreach (var way in map.ways.FindAll((w) => { return w.IsRoad && !w.IsRailway; }))
        {
            GameObject go = new GameObject();
            go.name = string.IsNullOrEmpty(way.Name) ? "OSMway" : way.Name;
            Vector3 localOrigin = GetCenter(way);
            go.transform.position = localOrigin - map.bounds.Center;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            mr.material = roadMaterial;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            for (int i = 1; i < way.NodeIDs.Count; i++)
            {
                OSMNode p1 = map.nodes[way.NodeIDs[i - 1]];
                OSMNode p2 = map.nodes[way.NodeIDs[i]];

                Vector3 s1 = p1 - localOrigin;
                // Need to average here
                Vector3 s2 = p2 - localOrigin;
                //Vector3 s3 = Vector3.zero;



                Vector3 diff = (s2 - s1).normalized;
                var cross = Vector3.Cross(diff, Vector3.up) * 3.0f * way.Lanes; // Add lanes here

                //if (i < way.NodeIDs.Count - 1)
                //{
                //    s3 += map.nodes[way.NodeIDs[i + 1]] - localOrigin;
                //}

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;


                vectors.Add(v1);
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                // First Triangle
                indices.Add(idx1);
                indices.Add(idx3);
                indices.Add(idx2);

                // Second
                indices.Add(idx3);
                indices.Add(idx4);
                indices.Add(idx2);
            
            }

            mf.mesh.vertices = vectors.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indices.ToArray();

            NavMeshSurface surface = go.AddComponent<NavMeshSurface>();

            yield return null;

        }
    }
}
