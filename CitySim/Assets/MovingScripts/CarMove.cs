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
    [Header("Sensors")]
    public float sensorLength = 25f;
    public float frontSensorPos = -1.7f;
    public float frontSideSensorPos = 1.1f;
    public float frontSensorAngle = 40;

    // Variables dealing with movement
    [Header("Car Movement")]
    public float maxSteerAngle = 40f;
    public float maxSpeed = 100f;
    public float maxAccelTorque = 100f;
    public float maxDecelTorque = -150f;
    public float maxBrakeTorque = 3500f;
    public bool isBraking = false;
    private float currentSpeed;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public Vector3 centerOfMass;

    // Variables dealing with NavMeshAgent
    [Header("NavMeshAgent")]
    public NavMeshAgent navMeshAgent;
    public float maxNavSpeed = 1f;

	// Use this for initialization
	void Start () {
        // Remove navmesh movement
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.speed = maxNavSpeed;

        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

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
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
        //navMeshAgent.Move(relativeVect);

    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 100;
        Debug.Log("Current speed is: " + currentSpeed);
        //Debug.Log("isbrake is " + isBraking);
        if (currentSpeed < maxSpeed && !isBraking)
        {
            //Debug.Log("accelerate");
            wheelFL.motorTorque = maxAccelTorque;
            wheelFR.motorTorque = maxAccelTorque;
        }
        else if (isBraking && currentSpeed > 5)
        {
            //Debug.Log("decelerate");
            wheelFL.motorTorque = maxDecelTorque;
            wheelFR.motorTorque = maxDecelTorque;
        }
        else
        {
            wheelFL.motorTorque = 0f;
            wheelFR.motorTorque = 0f;
        }

        
    }

    // Function to apply braking torque on wheels
    private void Brake()
    {
        if (isBraking)
        {
            Debug.Log("Braking");
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
        }
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
