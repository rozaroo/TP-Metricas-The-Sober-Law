using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PrefabBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    SpriteRenderer sr;
    BoxCollider2D bc;
    AudioSource audio;
    public bool isFromPlayer;
    bool isDestroyed;
    float lifeSpawn;

    private void Awake()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        bc = this.gameObject.GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        ColliderResize();
        speed = 15f;
        audio.Play();
        lifeSpawn = 0;
        isDestroyed = false;
    }

    void Update()
    {
        if (!GameManager.isGamePaused)
        {
            lifeSpawn += Time.deltaTime;

            if (!sr.isVisible || isDestroyed)
            {
                Destroy(bc);
                sr.color = new Vector4(0, 0, 0, 0);
                if (lifeSpawn >= 1.25f) Destroy(this.gameObject);
            }
            gameObject.transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    private void ColliderResize()
    {
        Vector2 colliderSize = sr.bounds.size;
        bc.size = colliderSize;
    }

    public void DestroyBullet()
    {
        isDestroyed = true;
    }
}

