using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public LevelManager lvl;

    private float startElevator = 0;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float width = sr.bounds.size.x;
        bc.size = new Vector2(width, bc.size.y);
    }

    private void Update()
    {
        if (transform.position.y >= 0) lvl.initScreen.isLevelEnded = true;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        startElevator += Time.deltaTime;
        if (other.gameObject.layer == 12 && startElevator >= 2 && GameManager.Instance.isLevel1Completed) rb.velocity = new Vector2(0, 5);
    }
}
