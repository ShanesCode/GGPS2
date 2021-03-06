using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    private const int MAX_BOTTLE_STACK_HEIGHT_ABOVE_PLAYER = 2;
    private const float PICKUP_DISTANCE = 3f;
    private const float MAX_PLACE_DISTANCE = 8f;
    public GameObject bottle;
    public List<GameObject> bins;
    public bool hasBottle;
    public List<GameObject> bottles;

    private Animator anim;
    int flip;

    float bottleHeight;

    private int biggestStack;

    [Range(0, 5)] public float binDetectionDistance = 2.0f;
    
    public int drinkCount;
    public int recycleCount;
    //private int wasteCount;

    GameObject nearest_bottle;
    GameObject carried_bottle;
    Vector3 bottleCarryOffset;

    GameObject gameManager;
    GameObject levelManager;

    bool pickingUpBottle;
    bool placingBottle;

    // Start is called before the first frame update
    void Start()
    {
        carried_bottle = null;

        biggestStack = 0;

        gameManager = GameObject.FindWithTag("GameManager");
        drinkCount = gameManager.GetComponent<GameManager>().GetDrinkCount();
        recycleCount = gameManager.GetComponent<GameManager>().GetRecycleCount();

        foreach (GameObject bottle in GameObject.FindGameObjectsWithTag("Bottle"))
        {
            bottles.Add(bottle);
        }

        foreach (GameObject bin in GameObject.FindGameObjectsWithTag("Bin"))
        {
            bins.Add(bin);
        }

        hasBottle = false;
        anim = GetComponent<Animator>();
        anim.SetBool("hasBottle", hasBottle);

        bottleHeight = bottle.GetComponent<SpriteRenderer>().size.y * bottle.transform.localScale.y;

        levelManager = GameObject.FindWithTag("LevelManager");

        pickingUpBottle = false;
        placingBottle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerController>().GetFacing() == false)
        {
            flip = 1;
        }
        else
        {
            flip = -1;
        }

        if (anim.GetAnimatorTransitionInfo(0).IsUserName("pickupToCarryIdle") || anim.GetAnimatorTransitionInfo(0).IsUserName("pickupToCarryFall"))
        {
            pickingUpBottle = false;
        }

        if (anim.GetAnimatorTransitionInfo(0).IsUserName("placeToIdle"))
        {
            placingBottle = false;
        }


        // Prevent bottle functions from being called whilst an animation is playing that triggers a bottle function
        // on a particular frame - otherwise both will happen and cause issues
        if (!pickingUpBottle)
        {
            if (Input.GetKeyDown("r") && !hasBottle && !placingBottle)
            {
                CreateBottle();
            }

            if (Input.GetKeyDown("e"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("place") == false)
                {
                    InteractBottle();
                }
                else
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).length - 1f > anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
                    {
                        InteractBottle();
                    }
                }
            }
        }

        if (hasBottle && carried_bottle != null)
        {
            bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
            carried_bottle.transform.position = bottleCarryOffset;
        }
    }
    void InteractBottle()
    {               
        if (hasBottle == false)
        {
            CheckPickupBottle();
        }
        else
        {
            placingBottle = true;
            anim.SetTrigger("place");
            // PlaceBottle() function called during animation
        }
    }

    void CheckPickupBottle()
    {
        float shortest_distance_bottle = 100;

        if (bottles.Count == 0)
        {
            nearest_bottle = null;
        }

        // Iterates through all game objects and gets those with tag Bottle
        for (int i = 0; i < bottles.Count; i++)
        {
            if (bottles[i] == null)
            {
                continue;
            }

            // Check bottle is in the direction player is facing
            if (Mathf.Sign(bottles[i].transform.position.x - transform.position.x) != Mathf.Sign(flip)) {
                continue;
            }

            // Calculates the distance between the player and the current bottle being checked
            float working_diff = Vector3.Distance(transform.position, bottles[i].transform.position);

            // If the current bottle is nearer to the player than the previous nearest, it replaces that bottle
            if (working_diff < shortest_distance_bottle)
            {
                shortest_distance_bottle = working_diff;
                nearest_bottle = bottles[i];
            }
        }

        // If the nearest bottle is within the specified distance remove it from the list of bottles in the world and set it
        // to carried_bottle. Begin the pickup animation, which calls PickupBottle() on the frame that the player has leaned down
        if (shortest_distance_bottle < PICKUP_DISTANCE && nearest_bottle != null && nearest_bottle != gameObject.GetComponent<PlayerController>().ground)
        {
            pickingUpBottle = true;
            carried_bottle = nearest_bottle;
            // PickupBottle() function called during animation
            anim.SetTrigger("pickup");
        }
    }

    public void PickupBottle()
    {
        bottles.Remove(nearest_bottle);

        // For each child bottle of the one being picked up (if it is not the top of a stack), make the child bottle dynamic
        for (int i = 0; i < carried_bottle.transform.childCount; i++)
        {
            if (carried_bottle.transform.GetChild(i).tag == "Bottle")
            {
                carried_bottle.transform.GetChild(i).GetComponent<CapsuleCollider2D>().enabled = true;
                carried_bottle.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
                carried_bottle.transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = false;
                carried_bottle.transform.GetChild(i).transform.parent = null;
                i--;
            }
        }

        carried_bottle.GetComponent<Bottle>().SetBeingCarried(true);
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        carried_bottle.transform.position = bottleCarryOffset;

        nearest_bottle = null;
        hasBottle = true;
        anim.SetBool("hasBottle", hasBottle);
    }

    public void PlaceBottle()
    {
        // Calculate distance between player and bin objects
        foreach (GameObject bin in bins) {
            float diff = (transform.position - bin.transform.position).magnitude;

            // If there is a bin nearby
            if (diff < binDetectionDistance && diff > -binDetectionDistance)
            {
                hasBottle = false;
                anim.SetBool("hasBottle", hasBottle);
                recycleCount++;
                gameManager.GetComponent<GameManager>().UpdateRecycleCount(recycleCount);
                
                if (!carried_bottle.GetComponent<Bottle>().playerCreated || carried_bottle.GetComponent<Bottle>().counted)
                {
                    levelManager.GetComponent<LevelManager>().roomWasteCount--;
                }

                levelManager.GetComponent<LevelManager>().roomRecycleCount++;
                bottles.Remove(carried_bottle);
                Destroy(carried_bottle);
                return;
            }
        }

        // Either throw a bottle or add it onto a stack, if there's a stack in front of you
        Vector3 placement_coordinates = transform.position + new Vector3(flip * 1, 0, 0);
        BoxCollider2D bottle_collider;
        GameObject top_of_stack = null;
        List<GameObject> bottlesAtXOfPlacementPosition = new List<GameObject>();
        bool currentlyCheckingBiggestStack = false;
        if (bottles.Count > 0)
        {
            // For each bottle in the world
            foreach (GameObject b in bottles)
            {
                if (b == null)
                {
                    continue;
                }

                // Get the bottle collider
                bottle_collider = b.GetComponent<BoxCollider2D>();

                // Check if the bottle is at the x coordinate of where we want to place our bottle
                // bool bottleAtXOfPlacementPosition = (placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x || placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x);
                bool bottleAtXOfPlacementPosition = (placement_coordinates.x <= bottle_collider.bounds.max.x && placement_coordinates.x >= bottle_collider.bounds.min.x);
                if (bottleAtXOfPlacementPosition && Vector2.Distance(transform.position, b.transform.position) < MAX_PLACE_DISTANCE)
                {
                    // Add it to a new list
                    bottlesAtXOfPlacementPosition.Add(b);
                }
            }

            foreach (GameObject b in bottlesAtXOfPlacementPosition)
            {
                // If first bottle, set top_of_stack equal to that bottles
                if (top_of_stack == null)
                {
                    top_of_stack = b;
                }
                else
                {
                    // If not the first bottle, check if its higher than the top of the stack. If it is, make it the new top_of_stack
                    if (b.transform.position.y > top_of_stack.transform.position.y)
                    {
                        top_of_stack = b;
                    }
                }

                // If the biggest recorded stack is smaller than the stack being checked
                if (biggestStack < bottlesAtXOfPlacementPosition.Count)
                {
                    biggestStack = bottlesAtXOfPlacementPosition.Count;
                    currentlyCheckingBiggestStack = true;
                }
            }

            if (top_of_stack != null && top_of_stack != gameObject.GetComponent<PlayerController>().ground)
            {
                bottle_collider = top_of_stack.GetComponent<BoxCollider2D>();

                // If adding to the stack means the stack will be bigger than 2 bottles above the player's feet
                float playerFeetYPos = gameObject.GetComponent<Collider2D>().bounds.min.y;
                float topOfTopBottleYPos = top_of_stack.transform.position.y + bottleHeight;
                
                if (topOfTopBottleYPos > playerFeetYPos + MAX_BOTTLE_STACK_HEIGHT_ABOVE_PLAYER * bottleHeight)
                {
                    anim.SetTrigger("placeDisallowed");
                    return;
                }
                else
                {
                    // Put bottle on top of stack
                    carried_bottle.GetComponent<Bottle>().SetBeingCarried(false);
                    carried_bottle.transform.SetParent(top_of_stack.transform);
                    carried_bottle.transform.localPosition = new Vector3(0, bottleHeight, 0);

                    carried_bottle.GetComponent<Rigidbody2D>().isKinematic = true;
                    carried_bottle.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    AddToWasteCounters(carried_bottle);

                    // Stop ignoring collisions with player
                    Physics2D.IgnoreCollision(carried_bottle.GetComponent<BoxCollider2D>(), gameObject.GetComponent<Collider2D>(), false);
                    Physics2D.IgnoreCollision(carried_bottle.GetComponent<CapsuleCollider2D>(), gameObject.GetComponent<Collider2D>(), false);

                    bottles.Add(carried_bottle); // Adds the bottle to the list of bottles in the world
                    carried_bottle = null;
                    hasBottle = false;
                    anim.SetBool("hasBottle", hasBottle);

                    if (currentlyCheckingBiggestStack)
                    {
                        biggestStack++;
                        gameManager.GetComponent<GameManager>().UpdateBottleStack(biggestStack, true);
                    }

                    return;
                }
            }
            else
            {
                // Throw the bottle
                carried_bottle.GetComponent<Bottle>().SetBeingCarried(false); // Makes the bottles non-kinematic and adds the collider. Also sets mass and drag to default
                Physics2D.IgnoreCollision(carried_bottle.GetComponent<BoxCollider2D>(), gameObject.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(carried_bottle.GetComponent<CapsuleCollider2D>(), gameObject.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
                carried_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
                bottles.Add(carried_bottle); // Adds the bottle to the list of bottles in the world

                AddToWasteCounters(carried_bottle);

                hasBottle = false;
                anim.SetBool("hasBottle", hasBottle);
                carried_bottle = null;
                return;
            }
        }
        else
        {
            // Throw the bottle
            carried_bottle.GetComponent<Bottle>().SetBeingCarried(false); // Makes the bottles non-kinematic and adds the collider. Also sets mass and drag to default
            Physics2D.IgnoreCollision(carried_bottle.GetComponent<BoxCollider2D>(), gameObject.GetComponent<Collider2D>(), true);
            Physics2D.IgnoreCollision(carried_bottle.GetComponent<CapsuleCollider2D>(), gameObject.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
            carried_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
            bottles.Add(carried_bottle); // Adds the bottle to the list of bottles in the world

            AddToWasteCounters(carried_bottle);

            hasBottle = false;
            anim.SetBool("hasBottle", hasBottle);
            carried_bottle = null;
            return;
        }
    }

    public void CreateBottle()
    {
        drinkCount++;
        gameManager.GetComponent<GameManager>().UpdateDrinkCount(drinkCount);
        levelManager.GetComponent<LevelManager>().roomIndulgenceCount++;

        //carried_bottle = bottle;
        carried_bottle = Instantiate(bottle);
        carried_bottle.GetComponent<Bottle>().SetBeingCarried(true);  // Makes the bottles kinematic and removes the collider.  Also sets mass and drag to default
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<Collider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        carried_bottle.transform.position = bottleCarryOffset;
        carried_bottle.GetComponent<Bottle>().playerCreated = true;
        

        hasBottle = true;
        anim.SetTrigger("drink");
        anim.SetBool("hasBottle", hasBottle);
    }

    private void AddToWasteCounters(GameObject bottle)
    {
        // Add to waste count
        if (bottle.GetComponent<Bottle>().playerCreated && !bottle.GetComponent<Bottle>().counted)
        {
            bottle.GetComponent<Bottle>().wasteCount = gameManager.GetComponent<GameManager>().GetWasteCount();
            bottle.GetComponent<Bottle>().wasteCount++;
            levelManager.GetComponent<LevelManager>().roomWasteCount++;
            bottle.GetComponent<Bottle>().counted = true;
            gameManager.GetComponent<GameManager>().UpdateWasteCount(bottle.GetComponent<Bottle>().wasteCount);
        }
    }

    private void ResetPlaceTrigger()
    {
        anim.ResetTrigger("place");
    }

    private void ResetPickupTrigger()
    {
        anim.ResetTrigger("pickup");
    }

    private void ResetDrinkTrigger()
    {
        anim.ResetTrigger("place");
    }

    private void NoLongerPickingUp()
    {
        pickingUpBottle = false;
    }
}
