using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{

    private LayerMask groundMask;
    private bool grounded;
    private Vector2 groundOffset;
    private Vector2 size;
    BoxCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        size = col.size;
        groundMask = LayerMask.GetMask("Ground");
        groundOffset = new Vector2(0, GetComponent<BoxCollider2D>().size.y / 2);
        GameObject player = GameObject.Find("Player");
        int flip = 1;
        if (player.GetComponent<PlayerController>().GetFacing() == false)
        {
            flip = 1;
        }
        else
        {
            flip = -1;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(flip * 3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GroundCheck() == true)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Stay");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Exit");
        }
    }
    bool GroundCheck()
    {
        //Vector2 boxColliderPos = new Vector2(transform.position.x + col.offset.x, transform.position.y + col.offset.y);
        Vector2 bottom_of_object = new Vector2(transform.position.x, transform.position.y - (col.size.y / 2));

        RaycastHit2D hit = Physics2D.BoxCast(bottom_of_object, new Vector2(col.size.x * 0.5f, 0.0002f), 0f, new Vector2(0, 0), groundMask);
        
        if (hit)
        {
            float distance = Mathf.Abs(hit.point.y - bottom_of_object.y);
            if (distance <= size.y)
            {
                                             
                return true;
            }
        }
        return false;
    }
}
