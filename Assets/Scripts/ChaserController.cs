﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserController : MonoBehaviour
{
    public Canvas canvas;
    public Image reticle;
    public float maxAimSpeed = 200;
    public float aimAccelaration = 50;
    public float aimDecelaration = 0.75f;
    public float maxReticlePosition = 500f;

    private Rigidbody rb;
    private float aimSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(x, 0))
        {
            aimSpeed *= aimDecelaration * Time.deltaTime;
        }
        else
        {
            aimSpeed += x * aimAccelaration * Time.deltaTime;
            aimSpeed = Mathf.Min(aimSpeed, maxAimSpeed);
            aimSpeed = Mathf.Max(aimSpeed, -maxAimSpeed);
        }
        

        reticle.rectTransform.anchoredPosition += new Vector2(aimSpeed*Time.deltaTime, 0);

        float xCoord = Mathf.Min(reticle.rectTransform.anchoredPosition.x, maxReticlePosition);
        xCoord = Mathf.Max(xCoord, -maxReticlePosition);
        reticle.rectTransform.anchoredPosition = new Vector2(xCoord, 0);


    }
}
