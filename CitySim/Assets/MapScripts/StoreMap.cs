using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//public class StoreMap : MonoBehaviour {

//    public KeyCode saveKey = KeyCode.F12;
//    public Transform map;
//    public string folderName;
//    private int OSMCount = 0;
//    private Dictionary<string, int> dict = new Dictionary<string, int>();
//    //public string saveName = selec;

//    void Update()
//    {
//        if (Input.GetKeyDown(saveKey))
//        {
//            SaveAsset();
//            //MakePrefab();

//        }
//    }

//    void MakePrefab()
//    {
//        GameObject[] objectArray = Selection.gameObjects;

//        foreach (GameObject gameObject in objectArray)
//        {
//            string localPath = "Assets/Assets/Maps/" + gameObject.name + ".prefab";
//            Object prefab = PrefabUtility.CreatePrefab(localPath, gameObject);
//            //PrefabUtility.ReplacePrefab(gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
//        }
//    }

//    void SaveAsset()
//    {
//        // Create empty prefab
//        var emptyPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Assets/Maps/" + folderName + ".prefab");
//        // Save all procedural meshes
//        foreach (Transform child in transform) {
//            Debug.Log("Trying to save");
//            var mf = child.GetComponent<MeshFilter>();
//            if (mf)
//            {
//                string roadName = child.name;
//                int tmp = 0;
//                // Storing frequency of road names to create unique mesh names
//                if (dict.ContainsKey(roadName))
//                {
//                    dict[roadName] +=1;
//                    tmp = dict[roadName];
//                }
//                else
//                {
//                    dict.Add(roadName, 0);
//                }
//                string addOn = "";
//                if (tmp != 0)
//                {
//                    addOn = tmp.ToString();
//                }
//                var savePath = "Assets/Assets/Maps/" + folderName + "/"+ roadName + addOn + ".asset";
//                Debug.Log("Saved Mesh to:" + savePath);
//                AssetDatabase.CreateAsset(mf.mesh, savePath);
//                Debug.Log("successful save");
//            }

//        }
//        PrefabUtility.ReplacePrefab(gameObject, emptyPrefab);
//    }
//}
