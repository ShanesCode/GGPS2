using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bottle : MonoBehaviour
{
    public GameObject label;
    public GameObject labelText;

    [SerializeField] private LayerMask groundMask;
    private Vector2 size;
    BoxCollider2D col;
    GameObject player;

    GameObject ground;

    float x_offset_from_ground;

    bool grounded;

    int flip;

    public List<string> firstWords;
    public List<string> secondWords;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        size = col.size;
        player = GameObject.FindWithTag("Player");

        tag = "Bottle";

        RandomiseBottleColours();
        RandomiseBottleWords();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().GetFacing() == false)
        {
            flip = 1;
        }
        else
        {
            flip = -1;
        }

        if (grounded)
        {
            //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
        }
    }

    private void FixedUpdate()
    {
        grounded = GroundCheck();
    }

    bool GroundCheck()
    {
        Vector2 boxColliderPos = new Vector2(transform.position.x + col.offset.x, transform.position.y + col.offset.y);
        Vector2 bottom_of_object = new Vector2(transform.position.x, transform.position.y + (col.size.y / 2));

        gameObject.layer = LayerMask.NameToLayer("Default");
        RaycastHit2D hit = Physics2D.CircleCast(bottom_of_object, col.size.x / 2, Vector2.down, Mathf.Infinity, groundMask);

        if (hit)
        {
            float distance = Mathf.Abs(hit.point.y - boxColliderPos.y);
            if (distance <= 0.85)
            {
                if (ground != hit.transform.gameObject)
                {
                    gameObject.GetComponent<Rigidbody2D>().drag = 1;
                    gameObject.GetComponent<Rigidbody2D>().mass = 9999999;

                    if (hit.rigidbody != null)
                    {
                        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.y);
                    }

                    ground = hit.transform.gameObject;
                }

                gameObject.layer = LayerMask.NameToLayer("Ground");
                return true;
            }
        }

        gameObject.layer = LayerMask.NameToLayer("Ground");
        return false;
    }

    public void ChuckBottle()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(flip * 3f, 5f);
    }

    public void SetBeingCarried(bool carried)
    {
        ground = null;
        gameObject.GetComponent<Rigidbody2D>().drag = 0;
        gameObject.GetComponent<Rigidbody2D>().mass = 1;

        if (carried)
        {
            //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    void RandomiseBottleColours()
    {
        float bottleRed = UnityEngine.Random.Range(0.0f, 1.0f);
        float bottleGreen = UnityEngine.Random.Range(0.0f, 1.0f);
        float bottleBlue = UnityEngine.Random.Range(0.0f, 1.0f);

        gameObject.GetComponent<SpriteRenderer>().color = new Color(bottleRed, bottleGreen, bottleBlue);

        float labelRed = 1.0f - bottleRed;
        float labelGreen = 1.0f - bottleGreen;
        float labelBlue = 1.0f - bottleBlue;

        label.GetComponent<SpriteRenderer>().color = new Color(labelRed, labelGreen, labelBlue);
    }

    void RandomiseBottleWords()
    {
        int firstWord = UnityEngine.Random.Range(0, firstWords.Count);
        int secondWord = UnityEngine.Random.Range(0, secondWords.Count);

        labelText.GetComponent<TextMeshPro>().text = firstWords[firstWord] + '\n' + secondWords[secondWord];
    }
}
