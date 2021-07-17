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

    bool rightBlocked;
    bool leftBlocked;

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        longestFall = 0;

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

        rightBlocked = false;
        leftBlocked = false;

        gameManager = GameObject.FindWithTag("GameManager");
    }

    private void Update()
    {
        //pull inputs
        inputX = Input.GetAxis("Horizontal");
        xVelocity = speedMax * inputX;

        if (rightBlocked)
        {
            if (xVelocity > 0) { xVelocity = 0; }
        } 
        else if (leftBlocked)
        {
            if (xVelocity < 0) { xVelocity = 0; }
        }

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

        grounded = GroundCheck();

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
        if (jumpSquat) return;
        rb2d.velocity = new Vector2(xVelocity + groundVelocity.x, rb2d.velocity.y);
    }

    void Jump()
    {
        rb2d.AddForce(new Vector2(0, jumpForce));
        jumpSquat = false;

        jumpCount++;
        gameManager.GetComponent<GameManager>().UpdateJumpCount(jumpCount);

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

                fallStarted = false;
                if (longestFall < currentFall)
                {
                    longestFall = currentFall;
                    gameManager.GetComponent<GameManager>().UpdateLongestFallDistance(longestFall);
                }
                currentFall = 0;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get contact points
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        collision.GetContacts(contacts);

        // Below is being used to prevent wall-cling behaviour
        // Check if those contacts are near the leftmost or rightmost edge of collider
        int contactsNearBoundingBoxLeftSide = 0;
        int contactsNearBoundingBoxRightSide = 0;
        int contactsNearBoundingBoxBottomSide = 0;

        foreach (ContactPoint2D contact in contacts)
        {
            if (contact.collider == null) { break; }

            if (Mathf.Abs(contact.point.y - col.bounds.min.y) < 0.1)
            {
                contactsNearBoundingBoxBottomSide++;
            }

            if (Mathf.Abs(contact.point.x - col.bounds.min.x) < 0.1)
            {
                contactsNearBoundingBoxLeftSide++;
            }

            if (Mathf.Abs(contact.point.x - col.bounds.max.x) < 0.1)
            {
                contactsNearBoundingBoxRightSide++;
            }
        }

        // If all collision points are near leftmost or rightmost edge, assume player is colliding to his side and not below
        // Prevent player from adding any xVelocity in that direction
        if (contactsNearBoundingBoxLeftSide > 0 && contactsNearBoundingBoxRightSide == 0 && contactsNearBoundingBoxBottomSide != 0)
        {
            leftBlocked = true;
        }
        else if (contactsNearBoundingBoxRightSide > 0 && contactsNearBoundingBoxLeftSide == 0 && contactsNearBoundingBoxBottomSide != 0)
        {
            rightBlocked = true;
        }
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        rightBlocked = false;
        leftBlocked = false;
    }
}
