﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Powerups : MonoBehaviour, DelegateTimer
{
    public float speedUpMultiplier;
    public float slowDownMultiplier;
    public float speedUpTime;

    private ScrollingTerrain scrollingTerrain;
    private bool poweredUp = false;
    private bool slowedDown = false;

    // Start is called before the first frame update
    void Start()
    {
        scrollingTerrain = FindObjectOfType<ScrollingTerrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Test Button"))
        {
            SpeedUp();
        }

        if (Input.GetButtonDown("Test Button 2"))
        {
            SlowDown();
        }
    }

    void SpeedUp()
    {
        if (!slowedDown)
        {
            if (!poweredUp)
            {
                poweredUp = true;
                scrollingTerrain.ModifyScrollSpeed(speedUpMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.SpeedUp, this);
            }
        }
        else
            NormalSpeed();
    }

    void SlowDown()
    {
        if (!poweredUp)
        {
            scrollingTerrain.ModifyScrollSpeed(slowDownMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.SlowDown, this);
            slowedDown = true;
        }
        else
            NormalSpeed();
    }

    void NormalSpeed()
    {
        poweredUp = false;
        slowedDown = false;
        scrollingTerrain.ModifyScrollSpeed(slowDownMultiplier, speedUpTime, ScrollingTerrain.ScrollModificationType.BackToNormal, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            SpeedUp();
        }
        else if (other.transform.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            SlowDown();
        }
    }

    public void TimerFinishedCallback()
    {
        poweredUp = false;
        slowedDown = false;
    }
}
