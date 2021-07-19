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
    GameObject player;

    public bool beingCarried;

    int flip;

    public List<string> firstWords;
    public List<string> secondWords;

    public int wasteCount;
    GameObject gameManager;
    GameObject levelManager;

    public bool playerCreated;
    public bool counted;

    // Start is called before the first frame update
    private void Awake()
    {
        tag = "Bottle";
        counted = false;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        RandomiseBottleColours();
        RandomiseBottleWords();

        gameManager = GameObject.FindWithTag("GameManager");
        levelManager = GameObject.FindWithTag("LevelManager");
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

        if (transform.parent != null)
        {
            if (transform.parent.gameObject.tag == "Bottle" && transform.parent.gameObject.GetComponent<Bottle>().beingCarried)
            {
                transform.parent = null;
            }
        }
    }

    public void ChuckBottle()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(flip * 3f, 5f);

        if (playerCreated && !counted)
        {
            wasteCount = gameManager.GetComponent<GameManager>().GetWasteCount();
            wasteCount++;
            levelManager.GetComponent<LevelManager>().roomWasteCount++;
            counted = true;
            
            Debug.Log("Room Waste Count: " + levelManager.GetComponent<LevelManager>().roomWasteCount);
            gameManager.GetComponent<GameManager>().UpdateWasteCount(wasteCount);
        }
    }

    public void SetBeingCarried(bool carried)
    {
        beingCarried = carried;

        if (carried)
        {
            // Turn off the collider so that it doesn't hit anything whilst being carried
            // Make it kinematic so that its position can be set directly via the offset in BottleController
            transform.SetParent(null);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else
        {
            // Turn on the collider so that it hits things again
            // Make it non-kinematic so that it has gravity and forces act on it as normal
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
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
