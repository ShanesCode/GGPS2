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
    public event EventHandler<OnDeathEventArgs> OnDeath;

    public class OnDeathEventArgs
    {
        //empty..... for now..........
    }

    GameObject gameManager;
    private int jumpCount;

    private bool fallStarted;
    private float fallStartHeight;
    private float longestFall;
    private float currentFall;

    public float speedMax;
    private bool left;

    private float jumpForce;
    private bool jumpSquat;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask bottleMask;
    public bool grounded;

    private Vector2 groundVelocity;

    public GameObject ground;

    Animator anim;
    Rigidbody2D rb2d;
    BoxCollider2D col;

    private float inputX;
    private bool jump;
    private float xVelocity;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");

        jumpCount = gameManager.GetComponent<GameManager>().GetJumpCount();
        longestFall = 0;

        speedMax = 5.0f;
        jumpForce = 400.0f;
        left = true;
        jumpSquat = false;

        col = GetComponent<BoxCollider2D>();
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

        if (grounded)
        {
            jump = Input.GetButtonDown("Jump");
        }

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

        if (grounded)
        {
            anim.SetBool("grounded", true);

            OnGroundedEventArgs e = new OnGroundedEventArgs();
            e.groundTransform = ground.transform;

            if (ground.GetComponent<Rigidbody2D>() != null)
            {
                groundVelocity = ground.GetComponent<Rigidbody2D>().velocity;
            }

            fallStarted = false;
            if (longestFall < currentFall)
            {
                longestFall = currentFall;
                gameManager.GetComponent<GameManager>().UpdateLongestFallDistance(longestFall);
            }
            currentFall = 0;

            OnGrounded?.Invoke(this, e);
        } 
        else
        {
            anim.SetBool("grounded", false);
            groundVelocity = Vector2.zero;
        }

        anim.SetFloat("xSpeed", Mathf.Abs(xVelocity));
        anim.SetFloat("ySpeed", rb2d.velocity.y);

        if (!grounded && rb2d.velocity.y < -0.01 && !fallStarted)
        {
            fallStarted = true;
            fallStartHeight = transform.position.y;
        }

        if (fallStarted)
        {
            currentFall = fallStartHeight - transform.position.y;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GroundCheck();
        if (jumpSquat) return;
        rb2d.velocity = new Vector2(xVelocity + groundVelocity.x, rb2d.velocity.y);
    }

    public void Jump()
    {
        rb2d.AddForce(new Vector2(0, jumpForce));
        jumpSquat = false;

        jumpCount++;
        gameManager.GetComponent<GameManager>().UpdateJumpCount(jumpCount);

        grounded = false;
    }

    /*void GroundCheck()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .2f, groundMask);
        RaycastHit2D hitBottle = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .2f, bottleMask);

        if (hitGround)
        {
            ground = hitGround.transform.gameObject;
            grounded = true;
            return;
        }

        if (hitBottle)
        {
            ground = hitBottle.transform.gameObject;
            grounded = true;
            return;
        }

        grounded = false;
        return;
    }*/

    void Flip()
    {
        transform.localScale *= new Vector2(-1,1);
        left = !left;
    }

    public bool GetFacing()
    {
        return left;
    }

    public void Kill()
    {
        anim.SetTrigger("death");
    }

    public void Death()
    {
        OnDeathEventArgs e = new OnDeathEventArgs();

        OnDeath?.Invoke(this, e);
    }

    private void ResetJumpTrigger()
    {
        anim.ResetTrigger("jump");
    }
}
