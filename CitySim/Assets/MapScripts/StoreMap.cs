using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoreMap : MonoBehaviour {

    public KeyCode saveKey = KeyCode.F12;
    public Transform map;
    public string folderName;
    private int OSMCount = 0;
    //public string saveName = selec;

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveAsset();
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
        foreach (Transform child in transform) {
            Debug.Log("Trying to save");
            var mf = child.GetComponent<MeshFilter>();
            if (mf)
            {
                string roadName = child.name;
                var savePath = "Assets/Assets/Maps/" + folderName + "/"+ roadName + ".asset";
                Debug.Log("Saved Mesh to:" + savePath);
                AssetDatabase.CreateAsset(mf.mesh, savePath);
                Debug.Log("successful save");
            }
        }
        PrefabUtility.ReplacePrefab(gameObject, emptyPrefab);
    }
}
