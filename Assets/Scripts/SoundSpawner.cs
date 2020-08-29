using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    double position;
    bool spawn=false;
    
    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        double xPosition = Math.Abs( FindObjectOfType<MovementController>().transform.position.x);
        double yPosition = Math.Abs( FindObjectOfType<MovementController>().transform.position.y);
        double xPositionRound = Math.Round(xPosition) + .5f;
        double yPositionRound = Math.Round(yPosition) + .5f;


        //if (Vector3.Distance(FindObjectOfType<MovementController>().transform.position,FindObjectOfType<MovementController>().movePiont.position) <= .5f)
        if (Math.Abs(xPosition - xPositionRound) < 0.1f && Math.Abs(yPosition - yPositionRound) < 0.1f )
        {
            if (spawn == false)
            {
             //   objectPooler.SpawnFromPool("Sound");
                spawn = true;
            }
        }
        else { spawn = false; }
    }

    
}
