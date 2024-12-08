using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [SerializeField]
    private float parallaxEffect;

    private void Start()
    {

    }

    private void Update()
    {
        if (transform.position.x >= 19) transform.position = new Vector3(-19, transform.position.y, transform.position.z);
        transform.position = new Vector3(Time.deltaTime * parallaxEffect + transform.position.x, transform.position.y, transform.position.z);
    }

}
