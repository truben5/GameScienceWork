using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarMove : MonoBehaviour {

    // Variables dealing with waypoints
    public List<Vector3> wayPoints = new List<Vector3>();
    //public Vector3 destination;
    int totalPoints;
    int currPoint = 0;
    [NonSerialized]
    public bool inIntersection;
    [NonSerialized]
    public int triggerCount = 0;


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
    private bool sharpTurn = false;
    private bool reachedDest = false;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public Vector3 centerOfMass;

    // Variables dealing with NavMeshAgent
    [Header("NavMeshAgent")]
    public NavMeshAgent navMeshAgent;
    public float maxNavSpeed = 1f;
    public float maxDistFromCar = 15f;
    public float minDistFromCar = 3f;

	// Use this for initialization
	void Start () {

        inIntersection = false;

        // Remove navmesh movement
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.speed = maxNavSpeed;

        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        if(navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not attached to " + gameObject.name);
        }
        else
        {
            
            //navMeshAgent.SetDestination(destination);

            // Start navmesh agent moving towards first point
            // Assign total number of waypoints
            navMeshAgent.SetDestination(wayPoints[0]);
            totalPoints = wayPoints.Count;
            //Debug.Log(wayPoints.Count);
            //Debug.Log(navMeshAgent.GetComponent<NavMeshAgent>().pathEndPosition);
        }
	}


    // Update is called once per frame
    void Update () {
        //Debug.Log("update");
        navMeshAgent.transform.position = navMeshAgent.nextPosition;
        CheckCarDistance();
        CheckWayPointDistance();
	}

    void FixedUpdate()
    {
        //Debug.Log("Fixed update");
        if (!reachedDest)
        {
            Steer();
            //Debug.Log("calling drive");
            Drive();
        }
        else
        {
            //Debug.Log("Reached dest");
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
        }
        Sensor();
    }

    // Function to check if reached waypoint and keep track of current waypoint we are moving towards
    private void CheckWayPointDistance()
    {
        //Debug.Log(Vector3.Distance(navMeshAgent.GetComponent<NavMeshAgent>().transform.position, wayPoints[currPoint]));
        if (currPoint >= totalPoints)
        {
            //Debug.Log("currPoint: " + currPoint + "  total: " + totalPoints);
            navMeshAgent.isStopped = true;
        }

        // If very close to waypoint then set destination to next waypoint. Stop if reached destination
        else if (Vector3.Distance(navMeshAgent.transform.position, wayPoints[currPoint]) < 4.0f)
        //if (Vector3.Distance(navMeshAgent.transform.position, destination) < 3.0f)
        {
            currPoint++;
            if (currPoint == totalPoints)
            {
                //Debug.Log("reached dest");
                reachedDest = true;
                return;
            }
            //Debug.Log("moving to next waypoint" + wayPoints[currPoint]);
            navMeshAgent.SetDestination(wayPoints[currPoint]);
        }
    }

    private void CheckCarDistance()
    {
        float carDist = Vector3.Distance(navMeshAgent.transform.position, GetComponent<Rigidbody>().transform.position);
        // If distance to car is larger than the allowed max distance then slow the navMeshAgent
        if (carDist > maxDistFromCar)
        {
            navMeshAgent.velocity = navMeshAgent.velocity / 1.5f;
            //navMeshAgent.velocity = GetComponent<Rigidbody>().velocity;
            navMeshAgent.speed = maxNavSpeed / 1.5f;
        }
        // If distance to car is less than the allowed min distances then speed up navMeshAgents
        else if (carDist < minDistFromCar)
        {
            //Debug.Log("Too close");
            navMeshAgent.velocity = navMeshAgent.velocity * 1.7f;
            //navMeshAgent.velocity = na;
            //navMeshAgent.speed = maxNavSpeed * 1.5f;
            //maxSpeed += 1;
            navMeshAgent.speed = maxNavSpeed * 1.6f;
        }
    }

    private void Steer()
    {
        Vector3 relativeDest = navMeshAgent.nextPosition;
        // Vector from current position to relative destination
        Vector3 relativeVect = transform.InverseTransformPoint(relativeDest);
        float newSteer = -(relativeVect.x / relativeVect.magnitude) * maxSteerAngle;
        if (newSteer > 12f || newSteer < -12f && currentSpeed > maxSpeed / 2)
        {
            sharpTurn = true;
        }
        else
        {
            sharpTurn = false;
        }
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
        //navMeshAgent.Move(relativeVect);

    }

    private void Drive()
    {
        // calculate current speed
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        // Reset counter if hit 4 triggers
        triggerCount = triggerCount % 2;
        //Debug.Log(triggerCount);
        //Debug.Log("Current speed is: " + currentSpeed);
        //Debug.Log("isbrake is " + isBraking);
        //Debug.Log(currentSpeed);
        //Debug.Log(inIntersection);
        if (inIntersection && triggerCount == 1)
        {
            Debug.Log("In intersection at red light, slow down");
            wheelFL.motorTorque = maxDecelTorque;
            wheelFR.motorTorque = maxDecelTorque;

            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
        }

        else if (currentSpeed < maxSpeed && !isBraking && !sharpTurn)
        {
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            //Debug.Log("accelerate");
            wheelFL.motorTorque += maxAccelTorque / 10;
            wheelFR.motorTorque += maxAccelTorque / 10;
        }
        else if (isBraking && currentSpeed > 5 && !sharpTurn)
        {
            //Debug.Log("decelerate");
            wheelFL.motorTorque = maxDecelTorque;
            wheelFR.motorTorque = maxDecelTorque;
        }
        else if (sharpTurn)
        {
            //Debug.Log("turning");
            wheelFL.motorTorque = maxAccelTorque / 7;
            wheelFR.motorTorque = maxAccelTorque / 7;
            //wheelFL.motorTorque = maxDecelTorque;
            //wheelFR.motorTorque = maxDecelTorque;
        }
        else
        {
            //Debug.Log("none");
            if (wheelFL.motorTorque > 0)
            {
                wheelFL.motorTorque = 0f;
                wheelFR.motorTorque = 0f;
            }
            
        }
    }

    // Function to apply braking torque on wheels
    private void Brake()
    {
        if (isBraking)
        {
            //Debug.Log("Braking");
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
        //RaycastHit hit;
        //Vector3 sensorStartingPos = transform.position;
        //sensorStartingPos.z += frontSensorPos;

        //// Front center sensor
        //if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        //{
        //    Debug.DrawLine(sensorStartingPos, hit.point);
        //}
        //// Front right sensor
        //sensorStartingPos.x += frontSideSensorPos;
        //if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        //{
        //    Debug.DrawLine(sensorStartingPos, hit.point);
        //}

        //// Front right angle sensor
        //if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        //{
        //    Debug.DrawLine(sensorStartingPos, hit.point);
        //}

        //// Front left sensor
        //sensorStartingPos.x -= 2 * frontSideSensorPos;
        //if (Physics.Raycast(sensorStartingPos, transform.forward, out hit, sensorLength))
        //{
        //    Debug.DrawLine(sensorStartingPos, hit.point);
        //}

        //// Front left angle sensor
        //if (Physics.Raycast(sensorStartingPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        //{
        //    Debug.DrawLine(sensorStartingPos, hit.point);
        //}

    }
}
