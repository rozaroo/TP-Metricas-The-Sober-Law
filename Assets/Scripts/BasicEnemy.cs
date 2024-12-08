using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour, IEnemyObserver
{
    Animator anim;
    BoxCollider2D bc;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject player;

    [SerializeField] Transform castingTransform;
    [SerializeField] Transform groundCheck;
    [SerializeField] PrefabBullet bulletPrefab;
    [SerializeField] GameObject healingBox;

    [SerializeField] bool isFlippedAtStart;
    [SerializeField] bool isStatic;
    [SerializeField] bool isPatrolling;

    [SerializeField] byte enemyType;

    bool isMovingFoward;
    bool isAttacking;
    bool isInRange;
    bool isOnAir;

    int currentLife;

    float attackCooldown;
    float playerDistance = 7;
    float burstDelay;

    Vector3[] patrolPoints = new Vector3[2];

    //Dificultad
    [SerializeField] public Difficulty Dificultad;

    GameManager gamemanager;

    [SerializeField] LevelManager lvlmanager;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isPatrolling)
        {
            Vector3 diff = new Vector3(5, 0, 0);
            patrolPoints[0] = transform.position - diff;
            patrolPoints[1] = transform.position + diff;
        }
    }
    void Start()
    {
        gamemanager = GameManager.Instance;
        SetEnemyType(enemyType);
        isMovingFoward = true;
        isAttacking = false;
        attackCooldown = 1;
        isOnAir = true;
        ColliderResize();
        if (isFlippedAtStart) Rotate();
        if (isStatic) anim.SetBool("IsIdle", true);
        player = GameManager.Instance.player;
        if (player == null)
        {
            GameManager.Instance.FindPlayer();
            player = GameManager.Instance.player;
        }
    }
    void Update()
    {
        
        if (!GameManager.isGamePaused)
        {
            if (currentLife <= 0)
            {
                DropHealing();
                OnKilledEnemy();
                Destroy(this.gameObject);
            }

            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f);

            if (player != null) CheckForPlayer();
            if (!isAttacking && !isStatic && !isOnAir)
            {
                Move();

                if (groundInfo.collider == false)
                {
                    if (isMovingFoward) Rotate();
                    else Rotate();
                }

                if (isPatrolling)
                {
                    if (Vector3.Distance(transform.position, patrolPoints[0]) <= 0.5f || Vector3.Distance(transform.position, patrolPoints[1]) <= 0.5f) Rotate();
                }
            }
            if (sr.color != Color.white) sr.color += new Color(0, 1, 1, 0) * Time.deltaTime;
        }
    }
    bool PlayerIsInView(float distance) // Verifico que el jugador está a la vista.
    {
        bool value = false;
        float castDistance = distance;
        if (!isMovingFoward) castDistance = -distance;
        Vector2 endPosition = castingTransform.position + Vector3.right * castDistance;
        RaycastHit2D hit = Physics2D.Linecast(castingTransform.position, endPosition, 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && distance <= playerDistance) value = true;
            else value = false;
        }
        return value;
    }
    private void ColliderResize() // Se ajusta el Box Collider según el Sprite.
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
    void Attack(byte attackingType)
    {
        switch (attackingType)
        {
            case 0:
                PistolAttack(); 
                break;
            case 1:
                ShotgunAttack();
                break;
            case 2:
                SubmachineAttack();
                break;
            default:
                Debug.LogError("Attack type not found on the attack index");
                break;
        }
    }
    void SubmachineAttack()
    {
        if (gamemanager.Normal)
        {
            if (attackCooldown >= (0.8f * Dificultad.Normal))
            {
                burstDelay += Time.deltaTime;

                if (burstDelay >= (0.1f * Dificultad.Normal))
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                }
                if (attackCooldown >= (1.3f * Dificultad.Normal)) attackCooldown = 0;
            }
        }
        if (gamemanager.Easy)
        {
            if (attackCooldown >= (0.8f * Dificultad.Easy))
            {
                burstDelay += Time.deltaTime;
                if (burstDelay >=  (0.1f * Dificultad.Easy))
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                }
                if (attackCooldown >= (1.3f * Dificultad.Easy)) attackCooldown = 0;
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (attackCooldown >= (0.8f * Dificultad.VeryEasy))
            {
                burstDelay += Time.deltaTime;

                if (burstDelay >= (0.1f * Dificultad.VeryEasy))
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
                    burstDelay = 0;
                }
                if (attackCooldown >= (1.3f * Dificultad.VeryEasy)) attackCooldown = 0;
            }
        }
    }
    void PistolAttack()
    {
        attackCooldown += Time.deltaTime;
        if (gamemanager.Normal)
        {
            if (attackCooldown >= (0.5f * Dificultad.Normal))
            {
                PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                enemyBullet.isFromPlayer = false;
                attackCooldown = 0;
            }
        }
        if (gamemanager.Easy)
        {
            if (attackCooldown >= (0.5f * Dificultad.Easy))
            {
                PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                enemyBullet.isFromPlayer = false;
                attackCooldown = 0;
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (attackCooldown >= (0.5f * Dificultad.VeryEasy))
            {
                PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                enemyBullet.isFromPlayer = false;
                attackCooldown = 0;
            }
        }
    }
    void ShotgunAttack()
    {
        if (gamemanager.Normal)
        {
            if (attackCooldown >= (0.8f * Dificultad.Normal))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    attackCooldown = 0;
                }
            }
        }
        if (gamemanager.Easy)
        {
            if (attackCooldown >= (0.8f * Dificultad.Easy))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    attackCooldown = 0;
                }
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (attackCooldown >= (0.8f * Dificultad.VeryEasy))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(bulletPrefab, castingTransform.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    attackCooldown = 0;
                }
            }
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
    void CheckForPlayer() // Verifico que el jugador se encuentra en distancia de ataque.
    {
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        if (PlayerIsInView(distancePlayer))
        {
            isAttacking = true;
            anim.SetBool("IsShooting", true);
            attackCooldown += Time.deltaTime;
            Attack(enemyType);
        }
        else
        {
            isAttacking = false;
            anim.SetBool("IsShooting", false);
        }
    }
    void DropHealing() // Otorga vida en caso de detectar que el jugador tiene poca vida.
    {
        if (player.GetComponent<Player>().playerHp <= 30) Instantiate(healingBox, transform.position, transform.rotation);
    }
    void SetEnemyType(byte enemyType)
    {
        switch (enemyType)
        {
            case 0:
                currentLife = 100;
                attackCooldown = 1;
                break;

            case 1:
                currentLife = 75;
                attackCooldown = 1;
                break;

            case 2:
                currentLife = 120;
                attackCooldown = 1;
                break;

            default:
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PrefabBullet>() != null)
        {
            if (collision.gameObject.GetComponent<PrefabBullet>().isFromPlayer)
            {
                currentLife -= 25;
                sr.color = Color.red;
                collision.gameObject.GetComponent<PrefabBullet>().DestroyBullet();
                if (!isAttacking) Rotate();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isOnAir = false;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (isPatrolling)
        {
            Vector3 diff = new Vector3(5, 0, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + diff);
            Gizmos.DrawLine(transform.position, transform.position - diff);
        }
    }
    public void OnKilledEnemy()
    {
        if (lvlmanager.CurrentNivel == 1)
        {
            lvlmanager.enemiesToDefeat--;

            if (lvlmanager.enemyCounter == null) lvlmanager.enemyCounter = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
            lvlmanager.enemyCounter.text = lvlmanager.enemiesToDefeat.ToString();
            if (lvlmanager.enemiesToDefeat == 0) GameManager.Instance.isLevel1Completed = true;
        }
        if (lvlmanager.CurrentNivel == 2)
        {
            if (lvlmanager.timerChanger > 0.8f) lvlmanager.timerChanger -= 0.1f;
        }
    }
}
