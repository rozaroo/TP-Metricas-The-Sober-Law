using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    SpriteRenderer sr;
    Animator anim;

    [SerializeField]
    Transform noozle;
    
    [SerializeField]
    PrefabBullet bulletPrefab;

    bool isMovingFoward;
    bool isAttacking;

    int bossLevel;

    float attackTimer;
    float attackCooldown;
    float burstDelay;
    //Dificultad
    [SerializeField] public Difficulty Dificultad;
    GameManager gamemanager;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        gamemanager = GameManager.Instance;
        isMovingFoward = true;
        isAttacking = false;
        ColliderResize();
    }

    void Update()
    {

    }

    //Ajusto el collider según el tamaño del sprite
    private void ColliderResize()
    {
        Vector2 colliderSize = sr.bounds.size;
        bc.size = colliderSize;
    }

    void Move()
    {
        if (isMovingFoward)
        {
            if (gamemanager.Normal) transform.position += new Vector3((5 / Dificultad.Normal) * Time.deltaTime, 0, 0);
            if (gamemanager.Easy) transform.position += new Vector3((5 / Dificultad.Easy) * Time.deltaTime, 0, 0);
            if (gamemanager.VeryEasy) transform.position += new Vector3((5 / Dificultad.VeryEasy) * Time.deltaTime, 0, 0);
        }
        else
        {
            if (gamemanager.Normal) transform.position -= new Vector3((5 / Dificultad.Normal) * Time.deltaTime, 0, 0);
            if (gamemanager.Easy) transform.position -= new Vector3((5 / Dificultad.Easy) * Time.deltaTime, 0, 0);
            if (gamemanager.VeryEasy) transform.position -= new Vector3((5 / Dificultad.VeryEasy) * Time.deltaTime, 0, 0);
        }
    }

    void Attack(int bossLevel)
    {
        switch (bossLevel)
        {
            case 1:
                    if (attackTimer >= attackCooldown)
                    {
                        PrefabBullet enemyBullet = Instantiate(bulletPrefab, noozle.position, transform.rotation);
                        enemyBullet.isFromPlayer = false;
                        attackTimer = 0;
                    }
            break;

            case 2:
                    if (attackCooldown >= 0.8f)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            PrefabBullet enemyBullet = Instantiate(bulletPrefab, noozle.position, transform.rotation);
                            enemyBullet.isFromPlayer = false;
                            enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                            enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                            attackCooldown = 0;
                        }
                    }
                break;

            case 3:
                    if (attackTimer >= attackCooldown)
                    {
                        burstDelay += Time.deltaTime;
                        if (burstDelay >= 0.1f)
                        {
                            PrefabBullet enemyBullet = Instantiate(bulletPrefab, noozle.position, transform.rotation);
                            enemyBullet.isFromPlayer = false;
                            enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                            burstDelay = 0;
                        }
                        if (attackTimer >= 1.3f) attackTimer = 0;
                    }
                break;

            default:
                break;
        }
        
    }

    void Rotate()
    {
        if (isMovingFoward)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isMovingFoward = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isMovingFoward = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PrefabBullet>().isFromPlayer)
        {
            Destroy(collision.gameObject);
            if (!isAttacking) Rotate();
        }
    }

    private void LateUpdate()
    {
        if (!isAttacking) Move();
        else
        {
            attackTimer += Time.deltaTime;
            Attack(bossLevel);
        }
    }
}
