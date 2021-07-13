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
    public Vector2 size;
    public bool grounded;

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
        gravity = 0.5f;
        drag = 0.5f;
        grounded = false;
        groundMask = LayerMask.GetMask("Ground");
        size = new Vector2(GetComponent<BoxCollider2D>().bounds.extents.x, GetComponent<BoxCollider2D>().bounds.extents.y);

        velocity = new Vector2(0, 0);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        velocity.x = HandleGroundMovement(velocity.x, inputX);

        grounded = GroundCheck();
        if (!grounded)
        {
            velocity.y -= gravity;
            velocity.y = Mathf.Clamp(velocity.y, -topFallSpeed, 0);
        }
        else
        {
            velocity.y = 0;
        }

        Vector2 step = new Vector2(velocity.x,velocity.y);
        transform.Translate(step);
        lastInput = inputX;
    }

    bool GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * 2, Color.magenta, 0.01f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(size.x,size.y/2), 0.0f, Vector2.down, size.y, groundMask);
        if (hit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
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
