using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Pistol, Pump, Submachine}
public enum States {Attack, Idle, Patrol}

public class EnemyController : MonoBehaviour
{
    EnemyAttack attack;
    PlataformWalk walk;
    Animator animator;
    [SerializeField] EnemyType type;
    States state;
    bool isOnAir = true;
    [SerializeField] bool isStatic;
    float life;
    float shootCooldown;
    float shootCounter;
    [SerializeField] float distance;

    

    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
        walk = GetComponent<PlataformWalk>();
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        life = 100;
        if (type == EnemyType.Submachine) shootCooldown = 0.2f;
        else if (type == EnemyType.Pump) shootCooldown = 1f;
        else shootCooldown = 0.5f;

        if (isStatic) state = States.Idle;
        else state = States.Patrol;
    }
    
    void Update()
    {
        shootCounter += Time.deltaTime;
        if (isOnAir) return;
        PlayerInSight();

        switch (state)
        {
            case States.Attack:
                if (shootCounter >= shootCooldown)
                {
                    attack.Shoot(type);
                    shootCounter = 0;
                }
                break;
            case States.Idle:
                break;
            case States.Patrol:
                walk.Walk();
                break;
            default:
                break;
        }
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + transform.right/2 , transform.right * 2);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 0 || other.gameObject.layer == 8) isOnAir = false;
        else isOnAir = true;
    }

    void PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right / 2, transform.right, 2f);

        if (hit.collider == false)
        {
            if (isStatic) state = States.Idle;
            else state = States.Patrol;
        }
        else
        {
            if (hit.collider.CompareTag("Player")) state = States.Attack;
            else
            {
                if (isStatic) state = States.Idle;
                else state = States.Patrol;
            }
        }
    }
}
