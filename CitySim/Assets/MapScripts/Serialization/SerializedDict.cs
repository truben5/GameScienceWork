using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryContainer : MonoBehaviour {

    [SerializeField]
    public class SerializedDict : Dictionary<Vector3, GraphNode>
    {
        public SerializedDict() : base()
        {
            //graph = new Dictionary<Vector3, GraphNode>();
        }
    }

    public SerializedDict graph;


}
