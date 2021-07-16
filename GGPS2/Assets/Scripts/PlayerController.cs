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

    private LayerMask groundMask;
    [SerializeField] private bool grounded;
    private Vector2 groundOffset;
    private Vector2 size;

    private Vector2 groundVelocity;

    Animator anim;
    Rigidbody2D rb2d;
    BoxCollider2D col;

    private float inputX;
    private bool jump;
    private float xVelocity;

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;

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

        xVelocity = 0;

        groundVelocity = Vector2.zero;
    }

    private void Update()
    {
        //pull inputs
        inputX = Input.GetAxis("Horizontal");
        xVelocity = speedMax * inputX;

        jump = Input.GetButtonDown("Jump");

        if (grounded && jump)
        {
            anim.SetBool("jump", true);
            jumpSquat = true;
        }

        if (Mathf.Abs(xVelocity) > 0)
        {
            if (xVelocity > 0 && left) Flip();
            if (xVelocity < 0 && !left) Flip();
        }

        grounded = GroundCheck();

        anim.SetFloat("xSpeed", Mathf.Abs(xVelocity));
        anim.SetFloat("ySpeed", rb2d.velocity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (jumpSquat) return;
        rb2d.velocity = new Vector2(xVelocity + groundVelocity.x, rb2d.velocity.y);
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

                if (hit.rigidbody != null) {
                    groundVelocity = hit.rigidbody.velocity;
                }

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
