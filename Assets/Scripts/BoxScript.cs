using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _BoxCollider;
    Rigidbody2D _rigidBody;
    float _VanishTimer;
    Color _initColor;
    public bool isGrounded;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _BoxCollider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isGrounded = false;
        _initColor = _spriteRenderer.color;
        _VanishTimer = 0;
    }

    void Update()
    {
        if (isGrounded)
        {
            _VanishTimer += Time.deltaTime;
            if (_VanishTimer >= 0.25f)
            {
                _initColor.a -= Time.deltaTime;
                _spriteRenderer.color = _initColor;
            }
        }
        if (_spriteRenderer.color.a <= 0) Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
            Destroy(_rigidBody);
            Destroy(_BoxCollider);
        }
    }
}
