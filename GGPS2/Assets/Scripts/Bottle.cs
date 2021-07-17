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

    private int wasteCount;
    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        size = col.size;
        player = GameObject.FindWithTag("Player");

        tag = "Bottle";

        RandomiseBottleColours();
        RandomiseBottleWords();

        gameManager = GameObject.FindWithTag("GameManager");
        wasteCount = gameManager.GetComponent<GameManager>().GetWasteCount();
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

        // Set this bottle to default layer so that it doesn't hit itself whilst going through these lines of code
        gameObject.layer = LayerMask.NameToLayer("Default");
        RaycastHit2D hit = Physics2D.CircleCast(bottom_of_object, col.size.x / 2, Vector2.down, Mathf.Infinity, groundMask);

        if (hit)
        {
            float distance = Mathf.Abs(hit.point.y - boxColliderPos.y);
            if (distance <= 0.85)
            {
                // If ground changes
                if (ground != hit.transform.gameObject)
                { 
                    // Set mass super high so player can't move the bottle
                    // Set drag to 1 so that the bottle stays on the dog (haven't checked bird). A high value seems to make the bottle get stuck in the air
                    gameObject.GetComponent<Rigidbody2D>().drag = 1;
                    gameObject.GetComponent<Rigidbody2D>().mass = 9999999;

                    // If the ground has a rigidbody (moving platforms or other bottle on platform)
                    if (hit.rigidbody != null)
                    {
                        // Set the velocity equal to that of the ground
                        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(hit.rigidbody.velocity.x, hit.rigidbody.velocity.y);
                    }

                    // Update current ground
                    ground = hit.transform.gameObject;
                }

                // Set the bottle back to the ground layer so that it is hit by the same code for other bottles
                gameObject.layer = LayerMask.NameToLayer("Ground");
                return true;
            }
        }

        // Set the bottle back to the ground layer so that it is hit by the same code for other bottles
        gameObject.layer = LayerMask.NameToLayer("Ground");
        return false;
    }

    public void ChuckBottle()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(flip * 3f, 5f);
        wasteCount++;
        gameManager.GetComponent<GameManager>().UpdateWasteCount(wasteCount);
    }

    public void SetBeingCarried(bool carried)
    {
        // Set ground to null so that next time we're grounded, the ground has changed for the condition in GroundCheck
        ground = null;
        // Set the mass and drag to default values so that the bottle moves nicely in the air and whilst being carried
        gameObject.GetComponent<Rigidbody2D>().drag = 0;
        gameObject.GetComponent<Rigidbody2D>().mass = 1;

        if (carried)
        {
            // Turn off the collider so that it doesn't hit anything whilst being carried
            // Make it kinematic so that its position can be set directly via the offset in BottleController
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else
        {
            // Turn on the collider so that it hits things again
            // Make it non-kinematic so that it has gravity and forces act on it as normal
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
