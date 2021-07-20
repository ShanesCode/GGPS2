using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleGroundedTrigger : MonoBehaviour
{
    public LayerMask groundMask;
    public LayerMask bottleMask;
    GameObject bottle;
    GameObject player;

    private void Start()
    {
        bottle = transform.parent.gameObject;
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bottle.GetComponent<Bottle>().beingCarried && (bottle.transform.parent == null || bottle.transform.parent.name == "Room0" || bottle.transform.parent.name == "Room1" || bottle.transform.parent.name == "Room2" || bottle.transform.parent.name == "Room3"))
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bottle")
            {
                // Set the ground as the parent
                bottle.transform.SetParent(collision.transform);

                if (collision.gameObject.tag == "Ground")
                {
                    bottle.transform.localPosition = bottle.transform.localPosition;
                }

                if (collision.gameObject.tag == "Bottle")
                {
                    bottle.transform.localPosition = new Vector3(0, bottle.transform.localPosition.y, 0);
                }

                bottle.GetComponent<Rigidbody2D>().isKinematic = true;
                bottle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                // Stop ignoring collisions with player
                Physics2D.IgnoreCollision(bottle.GetComponent<BoxCollider2D>(), player.GetComponent<Collider2D>(), false);
                Physics2D.IgnoreCollision(bottle.GetComponent<CapsuleCollider2D>(), player.GetComponent<Collider2D>(), false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bottle") && bottle.GetComponent<Bottle>().beingCarried)
        {
            bottle.transform.SetParent(null);
            //bottle.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
