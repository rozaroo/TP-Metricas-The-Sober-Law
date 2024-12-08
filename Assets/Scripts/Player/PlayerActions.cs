using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody;
    Animator _animator;
    BoxCollider2D _boxCollider;

    [SerializeField] PrefabBullet bullet;

    private float shootDelay;
    private float horizontalAxis;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jump;
    private float jumpCooldown = 0;

    private bool spriteMode;
    private bool isJumping;
    [SerializeField] bool isControlsEnabled;

    Vector3 _velocity;
    Vector3 _position;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        isControlsEnabled = true;
        jumpCooldown = 0.5f;
    }

    void Update()
    {
        jumpCooldown -= Time.deltaTime;
        if (jumpCooldown < 0) jumpCooldown = 0;

        if (!GameManager.isGamePaused)
        {
            if (isControlsEnabled)
            {
                horizontalAxis = Input.GetAxis("Horizontal");
                _animator.SetFloat("Speed", Mathf.Abs(horizontalAxis));
                Jump();
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Shoot()
    {
        shootDelay += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && shootDelay > 0.1f)
        {
            if (spriteMode)
            {
                PrefabBullet playerBullet = Instantiate(bullet, transform.position + new Vector3(-0.5f, 0.25f, 0), new Quaternion(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z, transform.position.magnitude));
                playerBullet.isFromPlayer = true;
                shootDelay = 0;
            }
            else
            {
                PrefabBullet playerBullet = Instantiate(bullet, transform.position + new Vector3(0.5f, 0.25f, 0), Quaternion.identity);
                playerBullet.isFromPlayer = true;
                shootDelay = 0;
            }
        }
    }
    private void Movement()
    {

        _rigidBody.velocity = new Vector2(horizontalAxis * movementSpeed, _rigidBody.velocity.y);
        if (horizontalAxis < 0) spriteMode = true;
        if (horizontalAxis > 0) spriteMode = false;
        _spriteRenderer.flipX = spriteMode;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false && jumpCooldown <= 0 )
        {
            _rigidBody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            isJumping = true;
            _animator.SetBool("IsJumping", true);
            jumpCooldown = 0.75f;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 0 || other.gameObject.layer == 8)
        {
            isJumping = false;
            _animator.SetBool("IsJumping", false);
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 0)
        {
            isJumping = false;
            _animator.SetBool("IsJumping", false);
        }
        if (other.gameObject.CompareTag("eplat") && Input.GetKey(KeyCode.S)) _boxCollider.isTrigger = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("eplat")) _boxCollider.isTrigger = false;
    }
    void ControlsEnabled()
    {
        isControlsEnabled = !isControlsEnabled;
    }

}
