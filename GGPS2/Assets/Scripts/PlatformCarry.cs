using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCarry : MonoBehaviour
{
    private Vector2 platformVelocity = Vector2.zero;
    private bool onPlatform;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        onPlatform = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onPlatform) transform.Translate(platformVelocity * Time.deltaTime);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.collider.gameObject;
        if (other.GetComponent<MovingPlatform>())
        {
            Debug.Log("onplatform");
            onPlatform = true;
            platformVelocity = other.GetComponent<Rigidbody2D>().velocity;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatform = false;
        platformVelocity = Vector2.zero;
    }
}
