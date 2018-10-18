﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

class RoadMaker : InfrastructureBehaviour
{ 
    public Material roadMaterial;

    [System.NonSerialized]
    public  List<Vector3> wayPoints = new List<Vector3>();
    [System.NonSerialized]
    public List<Vector3> stopLights = new List<Vector3>();

    public bool IsReady { get; private set; }

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
            // Name road if name is available
            go.name = string.IsNullOrEmpty(way.Name) ? "OSMway" : way.Name;
            // Set map to parent
            go.transform.parent = map.transform;

            Vector3 localOrigin = GetCenter(way);
            go.transform.position = localOrigin - map.bounds.Center;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            //BoxCollider bx = go.AddComponent<BoxCollider>();
            //MeshCollider mc = go.AddComponent<MeshCollider>();
            //mc.convex = true;
            //mc.sharedMesh = mf.mesh;

            mr.material = roadMaterial;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            for (int i = 1; i < way.NodeIDs.Count; i++)
            {
                OSMNode p1 = map.nodes[way.NodeIDs[i - 1]];
                OSMNode p2 = map.nodes[way.NodeIDs[i]];


                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;
                Vector3 s3 = Vector3.zero;

                //int avg = 1;
                Vector3 diff = Vector3.zero;

                //if (i > 0 && i < way.NodeIDs.Count - 1)
                //{
                //    avg = 2;
                //    s3 = map.nodes[way.NodeIDs[i + 1]];
                //    diff = ((s2 - s1) + (s3 - s2)).normalized / avg;
                //}
                //else
                //{
                    diff = (s2 - s1).normalized;
                //}

                //Vector3 diff = ((s2 - s1) + (s3 - s2) / avg).normalized;
                var cross = Vector3.Cross(diff, Vector3.up) * 4.0f * way.Lanes; // Add lanes here

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;

                // Add node location to waypoint
                wayPoints.Add(p1 - map.bounds.Center);

                // Add node locations to stoplights
                if (p1.IsStreetLight)
                {
                    //Debug.Log("light");
                    stopLights.Add(p1 - map.bounds.Center);
                }


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

                if (i > 1)
                {
                    int idPrev4, idPrev3;
                    idPrev4 = vectors.Count - 5;
                    idPrev3 = vectors.Count - 6;

                    //Triangles to connect breaks
                    indices.Add(idx1);
                    indices.Add(idx2);
                    indices.Add(idPrev4);

                    indices.Add(idPrev3);
                    indices.Add(idx1);
                    indices.Add(idx2);
                }

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
            surface.BuildNavMesh();

            yield return null;

        }

        IsReady = true;
        Debug.Log("Completed Road Rendering");
    }

    void OnDrawGizmos()
    {
        if (IsReady == true)
        {
            //Debug.Log("making gizmos");
            Gizmos.color = Color.red;
            foreach (var point in wayPoints)
            {
                Gizmos.DrawWireSphere(point, 1f);
            }

            Gizmos.color = Color.yellow;
            foreach(var point in stopLights)
            {
                Gizmos.DrawWireSphere(point, 1f);
            }
        }

    }
}
