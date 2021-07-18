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
    [SerializeField] private LayerMask bottleMask;
    BoxCollider2D col;
    GameObject player;

    public bool grounded;
    bool onTrueGround;
    Vector3 trueGroundedPosition;

    int flip;

    public List<string> firstWords;
    public List<string> secondWords;

    private int wasteCount;
    GameObject gameManager;

    // Start is called before the first frame update
    private void Awake()
    {
        tag = "Bottle";
    }

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");

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
            if (!onTrueGround)
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
            }
            else
            {
                transform.localPosition = trueGroundedPosition;
            }
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
            player.GetComponent<BottleController>().bottles.Add(gameObject); // Adds the bottle to the list of bottles in the world
        }
        else
        {
            transform.SetParent(null);
        }
    }

    private void FixedUpdate()
    {
        grounded = GroundCheck();
    }

    bool GroundCheck()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, groundMask);
        RaycastHit2D hitBottle = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, bottleMask);
        if (hitGround)
        {
            gameObject.transform.SetParent(hitGround.transform);
            trueGroundedPosition = transform.localPosition;
            onTrueGround = true;
            return true;
        }

        if (hitBottle)
        {
            gameObject.transform.SetParent(hitBottle.transform);
            onTrueGround = false;
            return true;
        }
        
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
        if (carried)
        {
            // Turn off the collider so that it doesn't hit anything whilst being carried
            // Make it kinematic so that its position can be set directly via the offset in BottleController
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            /*if (transform.childCount == 1)
            {
                transform.GetChild(1).GetComponent<Bottle>().grounded = false;
            }*/
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
