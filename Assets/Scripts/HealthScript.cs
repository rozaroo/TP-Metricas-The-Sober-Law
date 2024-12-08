using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    BoxCollider2D bc;
    SpriteRenderer sr;
    Rigidbody2D rb;
    AudioSource audio;
    bool isDestroyed;
    float counter;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        isDestroyed = false;
    }

    void Update()
    {
        if (isDestroyed)
        {
            counter += Time.deltaTime;
            if (counter >= 3) Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().RefreshHealth(30);
            DestroyHealth();
        }
    }

    void DestroyHealth()
    {
        isDestroyed = true;
        Destroy(bc);
        Destroy(sr);
        Destroy(rb);
        audio.Play();
    }
}
