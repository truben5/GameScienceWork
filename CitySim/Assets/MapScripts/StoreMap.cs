using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class StoreMap : MonoBehaviour
{

    public KeyCode saveKey = KeyCode.F12;
    public Transform map;
    public string folderName;
    //private int OSMCount = 0;
    private Dictionary<string, int> dict = new Dictionary<string, int>();
    //public string saveName = selec;

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveAsset();
            StoreGraph();
            //MakePrefab();

        }
    }

    void MakePrefab()
    {
        GameObject[] objectArray = Selection.gameObjects;

        foreach (GameObject gameObject in objectArray)
        {
            string localPath = "Assets/Assets/Maps/" + gameObject.name + ".prefab";
            Object prefab = PrefabUtility.CreatePrefab(localPath, gameObject);
            //PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }

    void SaveAsset()
    {
        // Create empty prefab
        var emptyPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Assets/Maps/" + folderName + ".prefab");
        // Save all procedural meshes
        foreach (Transform child in transform)
        {
            Debug.Log("Beginning to save road meshes");
            var mf = child.GetComponent<MeshFilter>();
            if (mf)
            {
                string roadName = child.name;
                if (roadName.Contains("/"))
                {
                    roadName = roadName.Replace('/','-');
                }
                int tmp = 0;
                // Storing frequency of road names to create unique mesh names
                if (dict.ContainsKey(roadName))
                {
                    dict[roadName] += 1;
                    tmp = dict[roadName];
                }
                else
                {
                    dict.Add(roadName, 0);
                }
                string addOn = "";
                if (tmp != 0)
                {
                    addOn = tmp.ToString();
                }
                var savePath = "Assets/Assets/Maps/" + folderName + "/" + roadName + addOn + ".asset";
                //Debug.Log("Saved Mesh to:" + savePath);
                AssetDatabase.CreateAsset(mf.mesh, savePath);
                //Debug.Log("successful save");
            }

        }
        Debug.Log("Successfully stored map");
        PrefabUtility.ReplacePrefab(gameObject, emptyPrefab);
    }
     
    void StoreGraph()
    {
        List<KeyValPair> serializedDict = new List<KeyValPair>();
        foreach (KeyValuePair<Vector3, GraphNode> pair in GetComponent<RoadMaker>().graph.nodes)
        {
            KeyValPair newPair = new KeyValPair(pair.Key, pair.Value.position, GetComponent<RoadMaker>().graph);
            newPair.connections.Add(newPair);
            serializedDict.Add(newPair);
        }

        FileStream fs = new FileStream("Assets/Resources/Graphs/" + map.name, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, serializedDict);
        fs.Close();
    }
}
