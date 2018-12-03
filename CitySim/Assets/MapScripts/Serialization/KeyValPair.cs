using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValPair{

    public  float keyX;
    public float keyY;
    public float keyZ;

    public float valX;
    public float valY;
    public float valZ;

    public KeyValPair(Vector3 key, Vector3 val)
    {
        keyX = key.x;
        keyY = key.y;
        keyZ = key.z;

        valX = val.x;
        valY = val.y;
        valZ = val.z;
    }
}
