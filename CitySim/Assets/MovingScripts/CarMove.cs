using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMove : MonoBehaviour {

    // Variables dealing with waypoints
    List<Transform> wayPoints = new List<Transform>();
    int totalPoints;
    int currPoint = 0;

    // Variables dealing with movement
    public float sensorLength = 25f;
    public float frontSensorPos = -1.7f;
    public float frontSideSensorPos = 1.1f;
    public float frontSensorAngle = 30;


    NavMeshAgent navMeshAgent;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        // Remove navmesh movement
        //navMeshAgent.updatePosition = false;
        //navMeshAgent.updateRotation = false;
        
        // Store all wayPoints in path
        GameObject path = GameObject.Find("Path");
        totalPoints = path.transform.childCount;
        for (int i = 0; i < totalPoints; i++)
        {
            wayPoints.Add(path.transform.GetChild(i));
        }

        if(navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not attached to " + gameObject.name);
        }
        else
        {
         //   navMeshAgent.SetDestination(wayPoints[0].position);
        }
	}

    // Update is called once per frame
    void FixedUpdate () {
        CheckWayPointDistance();
        MoveForward(wayPoints[currPoint].position);
        Sensor();
	}

    // Function to check if reached waypoint and keep track of current waypoint we are moving towards
    private void CheckWayPointDistance()
    {
        //if (currPoint >= totalPoints)
        //{
            //navMeshAgent.isStopped = true;
        //}
        //else if (Vector3.Distance(transform.position, wayPoints[currPoint].position) < 2.0f)
        //{
        //    currPoint++;
        //    if (currPoint == totalPoints)
        //    {
        //        return;
        //    }
        //    //navMeshAgent.SetDestination(wayPoints[currPoint].position);
        //}
    }

    private void MoveForward(Vector3 wayPoint)
    {
        Vector3 vect = transform.InverseTransformPoint(wayPoint);
        //rb.MovePosition(vect);
        transform.LookAt(vect);
        transform.Translate(0, 0, .01f);
        Vector3 movement = transform.forward * Time.deltaTime * .5f;
        //navMeshAgent.Move(movement);
    }

    private void Sensor()
    {
        RaycastHit hit;
        Vector3 sensorStartingPos = transform.position;
        sensorStartingPos.z += frontSensorPos;

        // Front center sensor
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartingPos, hit.point);
        }
        // Front right sensor
        sensorStartingPos.x += frontSideSensorPos;
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartingPos, hit.point);
        }

        // Front right angle sensor
        if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartingPos, hit.point);
        }

        // Front left sensor
        sensorStartingPos.x -= 2 * frontSideSensorPos;
        if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartingPos, hit.point);
        }

        // Front left angle sensor
        if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartingPos, hit.point);
        }

    }
}
