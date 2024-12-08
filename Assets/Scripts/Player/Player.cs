using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Animations;
using Image = UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public float playerHp;
    private float _maxHp = 100f;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidBody;
    
    public float PlayerHealth { get => playerHp; }
    //Observer
    private List<IPlayerObserver> playerObservers = new List<IPlayerObserver>();
    private List<IHpObserver> hpObservers = new List<IHpObserver>();
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        ColliderResize();
        if (GameManager.Instance.player == null)
        {
            GameManager.Instance.player = this.gameObject;
        }

        playerHp = _maxHp;
        GameManager.Instance.gameOver = false;
    }

    void Update()
    {
        if (playerHp <= 0)
        {
            NotifyPlayerDead();
            GameManager.Instance.gameOver = true;
        }
        if (_spriteRenderer.color != Color.white) _spriteRenderer.color += new Color(0, 1, 1, 0) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 11)
        {
            RefreshHealth(-15);
            Destroy(other.gameObject);
        }

        if (other.gameObject.GetComponent<BoxScript>() != null)
        {
            if (!other.gameObject.GetComponent<BoxScript>().isGrounded)
            {
                RefreshHealth(-30);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Healing"))
        {
            RefreshHealth(25);
            if (playerHp >= _maxHp) playerHp = _maxHp;
            NotifyHpChangedToObservers();
            Destroy(other.gameObject);
        }
    }

    private void ColliderResize()
    {
        Vector2 colliderSize = _spriteRenderer.bounds.size;
        _boxCollider.size = colliderSize;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.GetComponent<PrefabBullet>() != null)
        {
            if (!other.gameObject.GetComponent<PrefabBullet>().isFromPlayer)
            {
                RefreshHealth(-5);
                _spriteRenderer.color = Color.red;
                NotifyHpChangedToObservers();
                other.gameObject.GetComponent<PrefabBullet>().DestroyBullet();
            }
       }
    }

    public void RefreshHealth(int value)
    {
        playerHp += value;
        if (playerHp > _maxHp) playerHp = _maxHp;
        NotifyHpChangedToObservers();
    }
    //Observers
    public void RegisterObserver(IPlayerObserver observer)
    {
        playerObservers.Add(observer);
    }
    public void UnregisterObserver(IPlayerObserver observer)
    {
        playerObservers.Remove(observer);
    }
    private void NotifyPlayerDead()
    {
        foreach (var observer in playerObservers)
            observer.OnPlayerDead();
    }
    public void OnPlayerDead()
    {
        GameManager.Instance.player.SetActive(false);
    }
    public void RegisterHpObserver(IHpObserver observer)
    {
        hpObservers.Add(observer);
    }

    public void UnregisterHpObserver(IHpObserver observer)
    {
        hpObservers.Remove(observer);
    }
    private void NotifyHpChangedToObservers()
    {
        foreach (var observer in hpObservers)
        {
            observer.OnHpChanged();
        }
    }
}
