using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgun : MonoBehaviour
{
    Animator _animator;
    BoxCollider2D _boxCollider;
    SpriteRenderer _spriteRenderer;
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] Transform _noozle;
    [SerializeField] PrefabBullet _prefabBullet;
    [SerializeField] LevelManager _bossController;
    [SerializeField] GameObject _whiskeyDrop;
    [SerializeField] ParticleSystem part;

    Vector3 destination = Vector3.zero;

    public bool isAttacking;
    public bool isMoving;
    public bool isReturning;

    public int currentDestination;

    public float restingTimer;
    public float upperAttackCooldown;
    public float whiskeyCooldown;
    float _speedMovement;
    float _busrtDelay;
    float _attackCooldown;
    //Dificultad
    [SerializeField] public Difficulty Dificultad;
    GameManager gamemanager;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        gamemanager = GameManager.Instance;
        upperAttackCooldown = 0;
        _speedMovement = 8;
        isMoving = true;
        isAttacking = true;
        currentDestination = 3;
        _patrolPoints = _bossController.PatrolPoints;
        destination = _patrolPoints[currentDestination].position;
        _animator.SetBool("IsAttacking", true);
    }

    void Update()
    {
        if (!GameManager.isGamePaused && !GameManager.Instance.gameOver)
        {
            _attackCooldown += Time.deltaTime;

            if (currentDestination == 4 && isReturning || currentDestination == 1 && !isReturning)
            {
                isMoving = false;
                _animator.SetBool("IsAttacking", false);
                upperAttackCooldown += Time.deltaTime;

                if (upperAttackCooldown >= 3)
                {
                    isMoving = true;
                    _animator.SetBool("IsAttacking", true);
                }
            }
            if (isAttacking) Attack();
            
            if (isMoving) Move();
            else WhiskeyDrop();

            if (Vector3.Distance(transform.position, _patrolPoints[currentDestination].position) < 0.1)
            {
                DestinationSetter();
                upperAttackCooldown = 0;
            }
            if (_spriteRenderer.color != Color.white) _spriteRenderer.color += new Color(0, 1, 1, 0) * Time.deltaTime;
        }
    }

    void Flip()
    {
        if (transform.rotation.y != 180) transform.Rotate(new Vector3(0, 180, 0));
        else transform.Rotate(new Vector3(0, -180, 0));
    }

    void Move()
    {
        if (gamemanager.Normal) transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[currentDestination].position, (_speedMovement / Dificultad.Normal) * Time.deltaTime);
        if (gamemanager.Easy) transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[currentDestination].position, (_speedMovement / Dificultad.Easy) * Time.deltaTime);
        if (gamemanager.VeryEasy) transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[currentDestination].position, (_speedMovement / Dificultad.VeryEasy) * Time.deltaTime);
    }

    void Attack()
    {
        if (gamemanager.Normal)
        {
            if (_attackCooldown >= (0.8f * Dificultad.Normal))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(_prefabBullet, _noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    _attackCooldown = 0;
                    _animator.SetBool("IsAttacking", true);
                }
            }
        }
        if (gamemanager.Easy)
        {
            if (_attackCooldown >= (0.8f * Dificultad.Easy))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(_prefabBullet, _noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    _attackCooldown = 0;
                    _animator.SetBool("IsAttacking", true);
                }
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (_attackCooldown >= (0.8f * Dificultad.VeryEasy))
            {
                for (int i = 0; i < 3; i++)
                {
                    PrefabBullet enemyBullet = Instantiate(_prefabBullet, _noozle.position, transform.rotation);
                    enemyBullet.isFromPlayer = false;
                    enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
                    enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
                    _attackCooldown = 0;
                    _animator.SetBool("IsAttacking", true);
                }
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
            transform.position = _patrolPoints[2].position;
        }

        else if (currentDestination == 2 && !isReturning)
        {
            currentDestination = 3;
            isAttacking = true;
        }

        else if (currentDestination == 3 && !isReturning)
        {
            currentDestination = 4;
            transform.position = _patrolPoints[4].position;
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
            transform.position = _patrolPoints[3].position;
        }

        else if (currentDestination == 3 && isReturning)
        {
            currentDestination = 2;
            isAttacking = true;
        }

        else if (currentDestination == 2 && isReturning)
        {
            currentDestination = 1;
            transform.position = _patrolPoints[1].position;
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
            if (whiskeyCooldown >= (0.8f * Dificultad.Normal))
            {
                Instantiate(_whiskeyDrop, transform.position, transform.rotation);
                whiskeyCooldown = 0;
            }
        }
        if (gamemanager.Easy)
        {
            if (whiskeyCooldown >= (0.8f * Dificultad.Easy))
            {
                Instantiate(_whiskeyDrop, transform.position, transform.rotation);
                whiskeyCooldown = 0;
            }
        }
        if (gamemanager.VeryEasy)
        {
            if (whiskeyCooldown >= (0.8f * Dificultad.VeryEasy))
            {
                Instantiate(_whiskeyDrop, transform.position, transform.rotation);
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
                _bossController.LifeUpdate(1);
                _spriteRenderer.color = Color.red;
                Destroy(collision.gameObject);
            }
        }
    }
}
