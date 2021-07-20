using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedTrigger : MonoBehaviour
{
    public LayerMask groundMask;
    public LayerMask bottleMask;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bottle")
        {
            player.GetComponent<PlayerController>().grounded = true;
            player.GetComponent<PlayerController>().ground = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bottle")
        {
            player.GetComponent<PlayerController>().grounded = true;
            player.GetComponent<PlayerController>().ground = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bottle")
        {
            player.GetComponent<PlayerController>().ground = null;
            player.GetComponent<PlayerController>().grounded = false;
        }
    }
}
