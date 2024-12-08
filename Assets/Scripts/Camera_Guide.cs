using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Guide : MonoBehaviour
{
    public float guideSpeed;
    private Transform guideTransform;

    private void Awake()
    {
        guideTransform = GetComponent<Transform>();
    }

    void Update()
    {
        guideTransform.Translate(new Vector3(guideSpeed,0,0) * Time.deltaTime);
    }
}
