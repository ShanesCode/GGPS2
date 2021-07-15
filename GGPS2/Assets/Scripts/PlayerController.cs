using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speedMax;
    private bool left;

    private float jumpForce;
    private bool jumpSquat;

    private LayerMask groundMask;
    private bool grounded;
    private Vector2 groundOffset;
    private Vector2 size;

    private Vector2 platformVelocity = Vector2.zero;
    private bool onPlatform;

    Animator anim;
    Rigidbody2D rb2d;
    BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        speedMax = 5.0f;
        jumpForce = 400.0f;
        left = true;
        jumpSquat = false;

        onPlatform = false;

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
        
        rb2d.velocity = new Vector2(xVelocity,rb2d.velocity.y);
        if (onPlatform) rb2d.velocity += platformVelocity;
        
        grounded = GroundCheck();
        if(grounded && jump)
        {
            anim.SetBool("jump", true);
            jumpSquat = true;
        }

        anim.SetFloat("xSpeed", Mathf.Abs(inputX));
        anim.SetFloat("ySpeed", Mathf.Abs(rb2d.velocity.y));


    }

    void Jump()
    {
        rb2d.AddForce(new Vector2(0, jumpForce));
        jumpSquat = false;
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
                return true;
            }
        }
        anim.SetBool("grounded", false);
        return false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.GetComponent<MovingPlatform>())
        {
            onPlatform = true;
            platformVelocity = other.GetComponent<Rigidbody2D>().velocity;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatform = false;
        platformVelocity = Vector2.zero;
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
