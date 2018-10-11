using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

class PopulateStreets : MonoBehaviour {

    public int carPopulation = 100;
    public RoadMaker roadMaker;
    public GameObject carPrefab;
    Random random = new Random();

    // Use this for initialization
    IEnumerator Start () {
        while (!roadMaker.IsReady)
        {
            yield return null;
        }

        Debug.Log("Population starting");
        for (int i=0; i < carPopulation; i++)
        {
            Vector3 pos = roadMaker.wayPoints[random.Next(roadMaker.wayPoints.Count - 1)];
            Instantiate(carPrefab, pos, Quaternion.identity);
        }
	}
	

}
