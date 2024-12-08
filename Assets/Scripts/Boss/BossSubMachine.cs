using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSubMachine : MonoBehaviour
{
    Animator anim;
    BoxCollider2D bc;
    SpriteRenderer sr;

    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform noozle;
    [SerializeField] PrefabBullet prefabBullet;
    [SerializeField] LevelManager bossController;
    [SerializeField] GameObject whiskeyDrop;
    [SerializeField] ParticleSystem part;

    Vector3 destination;

    public bool isAttacking;
    public bool isMoving;
    public bool isReturning;

    public int currentDestination;

    public float restingTimer;
    float speedMovement;
    public float whiskeyCooldown;
    float burstDelay;
    float attackCooldown;
    public float upperAttackCooldown;
    //Dificultad
    [SerializeField] public Difficulty Dificultad;
    GameManager gamemanager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        gamemanager = GameManager.Instance;
        upperAttackCooldown = 0;
        speedMovement = 10;
        isMoving = true;
        isAttacking = true;
        currentDestination = 3;
        destination = patrolPoints[currentDestination].position;
        anim.SetBool("IsAttacking", true);
    }
    void Update()
    {
        if (!GameManager.isGamePaused && !GameManager.Instance.gameOver)
        {
            attackCooldown += Time.deltaTime;

            if (currentDestination == 4 && isReturning || currentDestination == 1 && !isReturning)
            {
                isMoving = false;
                anim.SetBool("IsAttacking", false);
                upperAttackCooldown += Time.deltaTime;

                if (upperAttackCooldown >= 3)
                {
                    isMoving = true;
                    anim.SetBool("IsAttacking", true);
                }
            }

            if (isAttacking) Attack();

            if (isMoving) Move();
            else WhiskeyDrop();

            if (Vector3.Distance(transform.position, patrolPoints[currentDestination].position) < 0.1)
            {
                DestinationSetter();
                upperAttackCooldown = 0;
            }
            if (sr.color != Color.white) sr.color += new Color(0, 1, 1, 0) * Time.deltaTime;
        }
    }
    void Flip()
    {
        if (transform.rotation.y != 180) transform.Rotate(new Vector3(0, 180, 0));
        else transform.Rotate(new Vector3(0, -180, 0));
    }
    void Move()
    {
        if (gamemanager.Normal) transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentDestination].position, (speedMovement / Dificultad.Normal) * Time.deltaTime);
        if (gamemanager.Easy) transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentDestination].position, (speedMovement / Dificultad.Easy) * Time.deltaTime);
        if (gamemanager.VeryEasy) transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentDestination].position, (speedMovement / Dificultad.VeryEasy) * Time.deltaTime);
    }
    void Attack()
    {
        if (gamemanager.Normal)
        {
            if (attackCooldown >= (0.3f * Dificultad.Normal))
            {
                burstDelay += Time.deltaTime;
                if (burstDelay >= (0.1f * Dificultad.Normal))
                {
                    PrefabBullet enemyBullet = Instantiate(prefabBullet, noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                    anim.SetBool("IsAttacking", true);
                }
                if (attackCooldown >= (1.3f * Dificultad.Normal)) attackCooldown = 0;
            }
        }
        if (gamemanager.Easy)
        {
            if (attackCooldown >= (0.3f * Dificultad.Easy))
            {
                burstDelay += Time.deltaTime;
                if (burstDelay >= (0.1f * Dificultad.Easy))
                {
                    PrefabBullet enemyBullet = Instantiate(prefabBullet, noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                    anim.SetBool("IsAttacking", true);
                }
                if (attackCooldown >= (1.3f * Dificultad.Easy)) attackCooldown = 0;
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (attackCooldown >= (0.3f * Dificultad.VeryEasy))
            {
                burstDelay += Time.deltaTime;
                if (burstDelay >= (0.1f * Dificultad.VeryEasy))
                {
                    PrefabBullet enemyBullet = Instantiate(prefabBullet, noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                    anim.SetBool("IsAttacking", true);
                }
                if (attackCooldown >= (1.3f * Dificultad.VeryEasy)) attackCooldown = 0;
            }
        }
    }
    void DestinationSetter()
    {
        if (currentDestination == 0)
        {
            currentDestination = 1;
            isReturning = false;
        }

        else if (currentDestination == 1 && !isReturning)
        {
            currentDestination = 2;
            transform.position = patrolPoints[2].position;
        }

        else if (currentDestination == 2 && !isReturning)
        {
            currentDestination = 3;
            isAttacking = true;
        }

        else if (currentDestination == 3 && !isReturning)
        {
            currentDestination = 4;
            transform.position = patrolPoints[4].position;
            isAttacking = false;
            Flip();
        }

        else if (currentDestination == 4 && !isReturning) currentDestination = 5;

        else if (currentDestination == 5)
        {
            isReturning = true;
            currentDestination = 4;
        }

        else if (currentDestination == 4 && isReturning)
        {
            currentDestination = 3;
            transform.position = patrolPoints[3].position;
        }

        else if (currentDestination == 3 && isReturning)
        {
            currentDestination = 2;
            isAttacking = true;
        }

        else if (currentDestination == 2 && isReturning)
        {
            currentDestination = 1;
            transform.position = patrolPoints[1].position;
            isAttacking = false;
            Flip();
        }
        else if (currentDestination == 1 && isReturning) currentDestination = 0;
    }
    void WhiskeyDrop()
    {
        whiskeyCooldown += Time.deltaTime;
        if (gamemanager.Normal)
        {
            if (whiskeyCooldown >= (0.5f * Dificultad.Normal))
            {
                Instantiate(whiskeyDrop, transform.position, transform.rotation);
                whiskeyCooldown = 0;
            }
        }
        if (gamemanager.Easy)
        {
            if (whiskeyCooldown >= (0.5f * Dificultad.Easy))
            {
                Instantiate(whiskeyDrop, transform.position, transform.rotation);
                whiskeyCooldown = 0;
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (whiskeyCooldown >= (0.5f * Dificultad.VeryEasy))
            {
                Instantiate(whiskeyDrop, transform.position, transform.rotation);
                whiskeyCooldown = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PrefabBullet>() != null)
        {
            if (collision.gameObject.GetComponent<PrefabBullet>().isFromPlayer)
            {
                bossController.LifeUpdate(1);
                sr.color = Color.red;
                Destroy(collision.gameObject);
            }
        }
    }
    private void OnDisable()
    {
        if (part != null)
        {
            part.gameObject.transform.position = transform.position;
            part.Play();
        }
    }
}
