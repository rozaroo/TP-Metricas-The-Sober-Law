using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCollider : MonoBehaviour
{
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        float width = sr.bounds.size.x;
        bc.size = new Vector2(width, bc.size.y);
    }
}
