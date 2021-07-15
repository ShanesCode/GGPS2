using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler<OnGroundedEventArgs> OnGrounded;
    public class OnGroundedEventArgs
    {
        public Transform groundTransform;
    }

    GameManager gameManager = new GameManager();
    private int jumpCount;

    public float speedMax;
    private bool left;

    private float jumpForce;
    private bool jumpSquat;
    bool jumped;

    private LayerMask groundMask;
    [SerializeField] private bool grounded;
    private Vector2 groundOffset;
    private Vector2 size;

    private Vector2 groundVelocity = Vector2.zero;

    Animator anim;
    Rigidbody2D rb2d;
    BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        jumped = false;

        speedMax = 5.0f;
        jumpForce = 400.0f;
        left = true;
        jumpSquat = false;

        col = GetComponent<BoxCollider2D>();
        size = col.size;
        groundMask = LayerMask.GetMask("Ground");
        groundOffset = new Vector2(0, GetComponent<BoxCollider2D>().size.y / 2);
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lockout during jumpsquat
        if (jumpSquat) return;

        //pull inputs
        float inputX = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        float xVelocity = speedMax * inputX;


        if (Mathf.Abs(xVelocity) > 0)
        {
            if (xVelocity > 0 && left) Flip();
            if (xVelocity < 0 && !left) Flip();
        }

        grounded = GroundCheck();

        rb2d.velocity = new Vector2(xVelocity + groundVelocity.x, rb2d.velocity.y);

        if (grounded && jump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            anim.SetBool("jump", true);
            jumpSquat = true;
        }

        anim.SetFloat("xSpeed", Mathf.Abs(inputX));
        anim.SetFloat("ySpeed", rb2d.velocity.y);


    }

    void Jump()
    {
        rb2d.AddForce(new Vector2(0, jumpForce));
        jumpSquat = false;

        jumpCount++;
        gameManager.UpdateJumpCount(jumpCount);

        grounded = false;
    }

    bool GroundCheck()
    {
        Vector2 boxColliderPos = new Vector2(transform.position.x + col.offset.x, transform.position.y + col.offset.y);

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + groundOffset, col.size.x/2, Vector2.down, groundMask);
        if (hit) 
        {
            float distance = Mathf.Abs(hit.point.y - boxColliderPos.y);
            if (distance <= size.y)
            {
                anim.SetBool("grounded", true);

                if (hit.rigidbody != null) { groundVelocity = hit.rigidbody.velocity; }

                OnGroundedEventArgs e = new OnGroundedEventArgs()
                {
                    groundTransform = hit.transform
                };
                
                OnGrounded?.Invoke(this, e);
                return true;
            }
        }
        anim.SetBool("grounded", false);
        groundVelocity = Vector2.zero;
        return false;
    }

    void Flip()
    {
        transform.localScale *= new Vector2(-1,1);
        left = !left;
    }

    public bool GetFacing()
    {
        return left;
    }
}
