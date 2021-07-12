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

    public Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        topSpeed = 0.02f;
        acceleration = 0.5f;
        friction = 0.5f;
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
        //if reversing direction
        if((Mathf.Sign(inputX) != Mathf.Sign(velocity.x))&&(inputX!=0 && velocity.x != 0))
        {
            velocity.x -= velocity.x;
        }
        else
        {
            velocity.x = inputX * Time.deltaTime * acceleration + velocity.x;
        }
     
        if (inputX == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, friction*Time.deltaTime);
        }
        velocity.x = Mathf.Clamp(velocity.x, -topSpeed, topSpeed);
        Vector2 step = new Vector2(velocity.x,0);

        transform.Translate(step);
    }
}
