using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChange : MonoBehaviour {

    [Header("Front/Back Lights")]
    public GameObject FTop;
    public GameObject FMid;
    public GameObject FBot;
    public GameObject BTop;
    public GameObject BMid;
    public GameObject BBot;
    [System.NonSerialized]
    public int FBMode = 2;

    [Header("Right/Left Lights")]
    public GameObject RTop;
    public GameObject RMid;
    public GameObject RBot;
    public GameObject LTop;
    public GameObject LMid;
    public GameObject LBot;
    [System.NonSerialized]
    public int RLmode = 0;

    [Header("Colors")]
    public Material stop;
    public Material go;
    public Material yield;
    public Material off;
    
    [Header("Cycle")]
    private float timer;
    public float cycleTime = 6f;

    [Header("Intersection Colliders")]
    public GameObject RCollider;
    public GameObject LCollider;
    public GameObject FCollider;
    public GameObject BCollider;

    void Start()
    {
        FBot.GetComponent<Renderer>().material = go;
        BBot.GetComponent<Renderer>().material = go;
        LTop.GetComponent<Renderer>().material = stop;
        RTop.GetComponent<Renderer>().material = stop;

        FCollider.GetComponent<LightTrigger>().state = 2;
        BCollider.GetComponent<LightTrigger>().state = 2;
        LCollider.GetComponent<LightTrigger>().state = 0;
        RCollider.GetComponent<LightTrigger>().state = 0;
    }

    // Update is called once per frame
    void Update () {
        if (timer > cycleTime)
        {
            lightChange();
            timer = 0;
        }
        timer += Time.deltaTime;

    }

    void lightChange()
    {
        // If front and back are yellow change FB to red and RL to green
        if (FBMode == 1)
        {
            //Debug.Log("FB Yellow changing to red, RL to green");
            FBMode = 0;
            RLmode = 2;

            // Set state for intersection colliders
            FCollider.GetComponent<LightTrigger>().state = 0;
            BCollider.GetComponent<LightTrigger>().state = 0;
            RCollider.GetComponent<LightTrigger>().state = 2;
            LCollider.GetComponent<LightTrigger>().state = 2;

            FMid.GetComponent<Renderer>().material = off;
            FTop.GetComponent<Renderer>().material = stop;

            BMid.GetComponent<Renderer>().material = off;
            BTop.GetComponent<Renderer>().material = stop;

            RBot.GetComponent<Renderer>().material = go;
            RTop.GetComponent<Renderer>().material = off;

            LBot.GetComponent<Renderer>().material = go;
            LTop.GetComponent<Renderer>().material = off;
        }
        else if (RLmode == 2)
        {
            //Debug.Log("RL from green to yellow");
            RLmode = 1;

            RCollider.GetComponent<LightTrigger>().state = 1;
            LCollider.GetComponent<LightTrigger>().state = 1;

            RMid.GetComponent<Renderer>().material = yield;
            RBot.GetComponent<Renderer>().material = off;

            LMid.GetComponent<Renderer>().material = yield;
            LBot.GetComponent<Renderer>().material = off;

        }
        else if (RLmode == 1)
        {
            //Debug.Log("RL going from green to red, FB going from red to green");
            RLmode = 0;
            FBMode = 2;

            RCollider.GetComponent<LightTrigger>().state = 0;
            LCollider.GetComponent<LightTrigger>().state = 0;
            FCollider.GetComponent<LightTrigger>().state = 2;
            BCollider.GetComponent<LightTrigger>().state = 2;

            RMid.GetComponent<Renderer>().material = off;
            RTop.GetComponent<Renderer>().material = stop;

            LMid.GetComponent<Renderer>().material = off;
            LTop.GetComponent<Renderer>().material = stop;

            FBot.GetComponent<Renderer>().material = go;
            FTop.GetComponent<Renderer>().material = off;

            BBot.GetComponent<Renderer>().material = go;
            BTop.GetComponent<Renderer>().material = off;
        }
        // If Front and back are green change front and back to yellow
        else if (FBMode == 2)
        {
            //Debug.Log("FB go from green to yellow");
            FBMode = 1;

            FCollider.GetComponent<LightTrigger>().state = 1;
            BCollider.GetComponent<LightTrigger>().state = 1;

            FMid.GetComponent<Renderer>().material = yield;
            FBot.GetComponent<Renderer>().material = off;

            BMid.GetComponent<Renderer>().material = yield;
            BBot.GetComponent<Renderer>().material = off;

        }
        // If front and back are red change FB to green
        else if (FBMode == 0)
        {
            //Debug.Log("FB red to green, RL to red");
            FBMode = 2;
            RLmode = 0;

            FCollider.GetComponent<LightTrigger>().state = 2;
            BCollider.GetComponent<LightTrigger>().state = 2;
            RCollider.GetComponent<LightTrigger>().state = 0;
            LCollider.GetComponent<LightTrigger>().state = 0;

            FBot.GetComponent<Renderer>().material = go;
            FTop.GetComponent<Renderer>().material = off;

            BBot.GetComponent<Renderer>().material = go;
            BTop.GetComponent<Renderer>().material = off;

            RBot.GetComponent<Renderer>().material = off;
            RTop.GetComponent<Renderer>().material = stop;

            LBot.GetComponent<Renderer>().material = off;
            LTop.GetComponent<Renderer>().material = stop;
        }
        //Debug.Log("FB: " + FBMode + " RL: " + RLmode);
    }
}
