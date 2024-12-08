using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMovement : MonoBehaviour
{
    private Transform tr;
    private Vector3 initialPosition;
    private bool direction;
    private float distance;
    
    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void Start()
    {
        initialPosition = tr.position;
        direction = true;
        distance = 5;
    }
    
    
    void Update()
    {
        FixedUpdate();
        if (transform.position.x >= initialPosition.x + distance) direction = false;
        if (transform.position.x <= initialPosition.x - distance) direction = true;
    }

    void FixedUpdate()
    {
        if (direction) transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        else transform.position -= new Vector3(1, 0, 0) * Time.deltaTime;
    }
}
