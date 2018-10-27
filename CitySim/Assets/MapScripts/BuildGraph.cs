using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGraph : MonoBehaviour {

    public GameObject map;

	// Use this for initialization
	void Start () {
        List<Vector3> wayPoints = map.GetComponent<RoadMaker>().wayPoints;
	}
}
