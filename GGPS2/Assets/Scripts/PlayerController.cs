using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float topSpeed;
    public float acceleration;
    public float airAcceleration;
    public float gravity;
    public float friction;
    public float drag;
    private float lastInput;

    public Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        topSpeed = 0.02f;
        acceleration = 0.5f;
        friction = 0.2f;
        /*
        airAcceleration = 0.5f;
        gravity = 0.5f;
        drag = 0.5f;
        */
        velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        velocity.x = HandleGroundMovement(velocity.x, inputX);
        
        Vector2 step = new Vector2(velocity.x,0);
        transform.Translate(step);
        lastInput = inputX;
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
        return xVel;
    }
}
