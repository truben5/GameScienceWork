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

    // Variables dealing with sensors
    public float sensorLength = 25f;
    public float frontSensorPos = -1.7f;
    public float frontSideSensorPos = 1.1f;
    public float frontSensorAngle = 40;

    // Variables dealing with movement
    public float maxSteerAngle = 40f;
    public float maxSpeed = 10;

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;

    // Variables dealing with NavMeshAgent
    public NavMeshAgent navMeshAgent;
    public float maxNavSpeed = 1f;

	// Use this for initialization
	void Start () {
        //navMeshAgent = GameObject.Find("Car").transform.GetChild(1).gameObject;
        // Remove navmesh movement
        //navMeshAgent.updatePosition = false;
        //navMeshAgent.updateRotation = false;
        navMeshAgent.speed = maxNavSpeed;

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
            navMeshAgent.SetDestination(wayPoints[0].position);
            //Debug.Log(navMeshAgent.GetComponent<NavMeshAgent>().pathEndPosition);
        }
	}

    // Update is called once per frame
    void Update () {
        navMeshAgent.transform.position = navMeshAgent.nextPosition;
        CheckWayPointDistance();
	}

    void FixedUpdate()
    {
        if (!(currPoint >= totalPoints))
        {
            Steer();
            Drive();
        }
        Sensor();
    }

    // Function to check if reached waypoint and keep track of current waypoint we are moving towards
    private void CheckWayPointDistance()
    {
        //Debug.Log(Vector3.Distance(navMeshAgent.GetComponent<NavMeshAgent>().transform.position, wayPoints[currPoint].position));
        if (currPoint >= totalPoints)
        {
            navMeshAgent.isStopped = true;
        }
        else if (Vector3.Distance(navMeshAgent.transform.position, wayPoints[currPoint].position) < 2.0f)
        {
            //Debug.Log("in here");
            currPoint++;
            if (currPoint == totalPoints)
            {
                return;
            }
            Debug.Log("moving to next waypoint" + wayPoints[currPoint].position);
            navMeshAgent.SetDestination(wayPoints[currPoint].position);
        }
    }

    private void Steer()
    {
        Vector3 relativeVect = transform.InverseTransformPoint(navMeshAgent.nextPosition);
        float newSteer = -(relativeVect.x / relativeVect.magnitude) * maxSteerAngle;
        WheelFL.steerAngle = newSteer;
        WheelFR.steerAngle = newSteer;
        //navMeshAgent.Move(relativeVect);

    }

    private void Drive()
    {
        WheelFL.motorTorque = 90f;
        WheelFR.motorTorque = 90f;
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
