using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;

    private Camera mainCamera;
    private float startPos;
    private float length;

    private void Start()
    {
        mainCamera = Camera.main;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float dist = (mainCamera.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, 
            transform.position.y, transform.position.z); 
        float temp = (mainCamera.transform.position.x * (1 - parallaxEffect));

        if (!GameManager.isGamePaused) {
            if (temp > startPos + length) startPos += length;
            else if (temp < startPos - length) startPos -= length;
        }
    }
    
}
