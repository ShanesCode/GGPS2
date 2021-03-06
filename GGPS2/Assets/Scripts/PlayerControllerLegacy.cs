using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerLegacy : MonoBehaviour
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
    private Transform spr;
    private bool left;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.FindWithTag("InitialSpawn").transform.position;
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
        jumpForce = 0.02f;

        grounded = false;
        groundMask = LayerMask.GetMask("Ground");
        size = new Vector2(GetComponent<BoxCollider2D>().bounds.extents.x, GetComponent<BoxCollider2D>().bounds.extents.y);

        velocity = new Vector2(0, 0);

        anim = GetComponent<Animator>();
        spr = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        string currentAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentAnimation == "jump" && !jumping)
        {
            anim.SetBool("jump", false);
            //jump lockout
            return;
        }

        float inputX = Input.GetAxis("Horizontal");

        grounded = GroundCheck();

        velocity.x = HandleGroundMovement(velocity.x, inputX, grounded);
        if(jumping) velocity.y = HandleJumpVelocity(velocity.y);

        if (!grounded)
        {
            velocity.y -= gravity;
            velocity.y = Mathf.Clamp(velocity.y, -topFallSpeed, topFallSpeed);
        }
        else
        {
            if(velocity.y < 0) velocity.y = 0;
            
            if (Input.GetButtonDown("Jump")) {
                anim.SetBool("jump", true);
                grounded = false;
            }
        }

        Vector2 step = new Vector2(velocity.x, velocity.y);
        transform.Translate(step);

        anim.SetFloat("ySpeed", velocity.y);
        anim.SetFloat("xSpeed", Mathf.Abs(velocity.x));
        if (left && velocity.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            left = false;
        }
        if (!left && velocity.x > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            left = true;
        }
        lastInput = inputX;
    }

    bool GroundCheck()
    {
        Vector2 boxColliderPos = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().offset.x, transform.position.y + GetComponent<BoxCollider2D>().offset.y);

        Debug.DrawRay(transform.position, Vector2.down * 2, Color.magenta, 0.01f);
        RaycastHit2D hit = Physics2D.BoxCast(boxColliderPos, new Vector2(size.x, size.y / 2), 0.0f, Vector2.down, size.y, groundMask);
        if (hit)
        {
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

    float HandleJumpVelocity(float yVel)
    {
        if(jumping) yVel += jumpForce;
        if(jumpTimer <= 0) jumping = false;
        jumpTimer -= Time.deltaTime;
        return yVel;
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            jumpTimer = jumpClimbLong;
        }
        else
        {
            jumpTimer = jumpClimb;
        }

        jumping = true;
    }

    float HandleGroundMovement(float xVel, float inputX, bool grounded)
    {
        //if reversing direction
        if ((Mathf.Sign(inputX) != Mathf.Sign(xVel)) && (inputX != 0 && xVel != 0))
        {
            xVel -= xVel;
        }
        else
        {
            xVel = xVel + inputX * Time.deltaTime * (grounded? acceleration : airAcceleration) ;
        }

        //if slowing or no input then apply friction
        if ((Mathf.Abs(inputX) < Mathf.Abs(lastInput)) || (inputX == 0))
        {
            xVel = Mathf.MoveTowards(xVel, 0, (grounded? friction :drag) * Time.deltaTime);
        }
        xVel = Mathf.Clamp(xVel, -topSpeed, topSpeed);
        return xVel;
    }
    
    void Death()
    {

    }

    void OnCollisionEnter2D(Collision2D c)
    {
        //empty
    }
}
