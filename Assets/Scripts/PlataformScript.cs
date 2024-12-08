using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformScript : MonoBehaviour
{
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    [SerializeField] private bool isFloor;

    private float triggerTimer = 0;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        float width = sr.bounds.size.x;
        bc.size = new Vector2 (width, bc.size.y);
    }

    private void Update()
    {
        if(!isFloor && !bc.isTrigger)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer >= 1) bc.isTrigger = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!isFloor)
        {
            if (other.gameObject.layer == 12 && Input.GetKey(KeyCode.S)) bc.isTrigger = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        bc.isTrigger = false;
    }
}
