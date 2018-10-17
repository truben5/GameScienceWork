using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour {

    [Header("Front/Back Lights")]
    public GameObject FTop;
    public GameObject FMid;
    public GameObject FBot;
    public GameObject BTop;
    public GameObject BMid;
    public GameObject BBot;
    private int FRMode = 2;

    [Header("Right/Left Lights")]
    public GameObject RTop;
    public GameObject RMid;
    public GameObject RBot;
    public GameObject LTop;
    public GameObject LMid;
    public GameObject LBot;
    private int RLmode = 0;

    [Header("Colors")]
    public Material stop;
    public Material go;
    public Material yield;
    public Material off;
    
    [Header("Cycle")]
    private float timer;
    public float cycleTime = 6f;

    void Start()
    {
        FBot.GetComponent<Renderer>().material = go;
        BBot.GetComponent<Renderer>().material = go;
        LTop.GetComponent<Renderer>().material = stop;
        RTop.GetComponent<Renderer>().material = stop;
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
        if (FRMode == 1)
        {
            Debug.Log("FB Yellow changing to red, RL to green");
            FRMode = 0;
            RLmode = 2;

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
            Debug.Log("RL go from green to yellow");
            RLmode = 1;
            RMid.GetComponent<Renderer>().material = yield;
            RBot.GetComponent<Renderer>().material = off;

            LMid.GetComponent<Renderer>().material = yield;
            LBot.GetComponent<Renderer>().material = off;

        }
        else if (RLmode == 1)
        {
            RLmode = 0;
            FRMode = 2;
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
        else if (FRMode == 2)
        {
            Debug.Log("FB go from green to yellow");
            FRMode = 1;
            FMid.GetComponent<Renderer>().material = yield;
            FBot.GetComponent<Renderer>().material = off;

            BMid.GetComponent<Renderer>().material = yield;
            BBot.GetComponent<Renderer>().material = off;

        }
        // If front and back are red change FB to green
        else if (FRMode == 0)
        {
            Debug.Log("FB red to go, RL to red");
            FRMode = 2;
            RLmode = 0;
            FBot.GetComponent<Renderer>().material = go;
            FTop.GetComponent<Renderer>().material = off;

            BBot.GetComponent<Renderer>().material = go;
            BTop.GetComponent<Renderer>().material = off;

            RBot.GetComponent<Renderer>().material = off;
            RTop.GetComponent<Renderer>().material = stop;

            LBot.GetComponent<Renderer>().material = off;
            LTop.GetComponent<Renderer>().material = stop;
        }
    }
}
