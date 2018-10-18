using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoreMap : MonoBehaviour {

    public KeyCode saveKey = KeyCode.F12;
    public Transform map;
    private int OSMCount = 0;
    //public string saveName = selec;

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            SaveAsset();
        }
    }

    void SaveAsset()
    {
        foreach(Transform child in transform) {
            Debug.Log("Trying to save");
            var mf = child.GetComponent<MeshFilter>();
            if (mf)
            {
                string roadName = child.name;
                var savePath = "";
                if (roadName == "OSMway")
                {
                    savePath = "Assets/Assets/Maps/Simple/OSMway" + OSMCount + ".asset";
                }
                else
                {
                    savePath = "Assets/Assets/Maps/Simple/" + roadName + ".asset";
                }
                Debug.Log("Saved Mesh to:" + savePath);
                AssetDatabase.CreateAsset(mf.mesh, savePath);
                Debug.Log("successful save");
            }
        }
    }
}
