using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D),
typeof(Animator))]
public class SoyBoyController : MonoBehaviour
{
    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration;
    public float speed = 14f;
    public float accel = 6f;
    private Vector2 input;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;
    void Start()
    {
        
    }
    public bool PlayerIsOnGround()
    {
        // 1
        bool groundCheck1 = Physics2D.Raycast(new Vector2(
                transform.position.x, transform.position.y - height),
            -Vector2.up, rayCastLengthCheck);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(
                transform.position.x + (width - 0.2f),
                transform.position.y - height), -Vector2.up,
            rayCastLengthCheck);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(
                transform.position.x - (width - 0.2f),
                transform.position.y - height), -Vector2.up,
            rayCastLengthCheck);
        // 2
        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }
    void Update()
    {
        // 1
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");
        // 2
        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX = true;
        }
        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;
        }
        else
        {
            isJumping = false;
            jumpDuration = 0f;
        }
        if (PlayerIsOnGround() && isJumping == false)
        {
            if (input.y > 0f)
            {
                isJumping = true;
            }
        }
        if (jumpDuration > jumpDurationThreshold) input.y = 0f;
    }
    void FixedUpdate()
    {
        // 1
        var acceleration = accel;
        var xVelocity = 0f;
        // 2
        if (input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rb.velocity.x;
        }
        // 3
        rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x)
                                * acceleration, 0));
        // 4
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        if (isJumping && jumpDuration < jumpDurationThreshold)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}
