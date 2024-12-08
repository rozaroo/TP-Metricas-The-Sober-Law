using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBuilding : MonoBehaviour
{
    Transform tr;
    float speed;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    void Start()
    {
        speed = 2;
    }

    void Update()
    {
        if (!GameManager.isGamePaused)
        {
            tr.position -= new Vector3(0, speed * Time.deltaTime, 0);
            if (tr.position.y <= -10) tr.position = new Vector3(transform.position.x, 10, 0);
        }
    }
}
