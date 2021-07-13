using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float topSpeed;
    public float acceleration;
    public float friction;

    public float topFallSpeed;
    public float airAcceleration;
    public float gravity;
    public float drag;
    public bool grounded;
    public float jumpDelay;
    public float jumpClimb;
    public float jumpClimbLong;
    public float jumpTimer;
    public float jumpForce;
    public bool jumping;

    public Vector2 size;

    public Vector2 velocity;
    private float lastInput;
    LayerMask groundMask;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        topSpeed = 0.02f;
        acceleration = 0.5f;
        friction = 0.2f;

        topFallSpeed = 0.02f;
        airAcceleration = 0.2f;
        gravity = 0.0005f;
        drag = 0.5f;

        jumpDelay = 0.2f;
        jumpClimb = 0.5f;
        jumpClimbLong = 0.75f;
        jumpForce = 1.0f;

        grounded = false;
        groundMask = LayerMask.GetMask("Ground");
        size = new Vector2(GetComponent<BoxCollider2D>().bounds.extents.x, GetComponent<BoxCollider2D>().bounds.extents.y);

        velocity = new Vector2(0, 0);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpTimer > 0)
        {
            if (!jumping)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    jumping = true;
                    if (Input.GetButton("Jump"))
                    {
                        jumpTimer = jumpClimbLong;
                    }
                    else
                    {
                        jumpTimer = jumpClimb;
                    }
                }
                return;
            }
            else
            {
                jumpTimer -= Time.deltaTime;
                velocity.y += jumpForce;
                if(jumpTimer <= 0)
                {
                    jumping = false;
                }
            }
            
        }

        float inputX = Input.GetAxis("Horizontal");
        velocity.x = HandleGroundMovement(velocity.x, inputX);

        grounded = GroundCheck();
        if (!grounded)
        {
            velocity.y -= gravity;
            velocity.y = Mathf.Clamp(velocity.y, -topFallSpeed, topFallSpeed);
        }
        else
        {
            if(velocity.y < 0)
            {
                velocity.y = 0;
            }
            if (Input.GetButton("Jump"))
            {
                jumpTimer = jumpDelay;
            }
        }

        Vector2 step = new Vector2(velocity.x,velocity.y);
        transform.Translate(step);
        anim.SetFloat("ySpeed",velocity.y);
        lastInput = inputX;
    }

    bool GroundCheck()
    {
        Vector2 boxColliderPos = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y);

        Debug.DrawRay(transform.position, Vector2.down * 2, Color.magenta, 0.01f);
        RaycastHit2D hit = Physics2D.BoxCast(boxColliderPos, new Vector2(size.x,size.y/2), 0.0f, Vector2.down, size.y, groundMask);
        if (hit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            float distance = Mathf.Abs(hit.point.y - boxColliderPos.y);
            if (distance > size.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    float HandleGroundMovement(float xVel, float inputX)
    {
        //if reversing direction
        if ((Mathf.Sign(inputX) != Mathf.Sign(xVel)) && (inputX != 0 && xVel != 0))
        {
            xVel -= xVel;
        }
        else
        {
            xVel = inputX * Time.deltaTime * acceleration + xVel;
        }

        //if slowing or no input then apply friction
        if ((Mathf.Abs(inputX) < Mathf.Abs(lastInput)) || (inputX == 0))
        {
            xVel = Mathf.MoveTowards(xVel, 0, friction * Time.deltaTime);
        }
        xVel = Mathf.Clamp(xVel, -topSpeed, topSpeed);
        anim.SetFloat("xSpeed", Mathf.Abs(xVel));
        return xVel;
    }
}
