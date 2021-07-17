using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    private const int MAX_BOTTLE_STACK_HEIGHT_ABOVE_PLAYER = 2;
    public GameObject bottle;
    public List<GameObject> bins;
    public bool hasBottle;
    public List<GameObject> bottles;

    public List<BottleStack> bottleStacks;

    public class BottleStack
    {
        public float xmin;
        public float xmax;
        public List<GameObject> bottles;
    }

    private Animator anim;
    int flip;

    float bottleHeight;

    private int biggestStack;

    [Range(0, 5)] public float binDetectionDistance = 2.0f;
    
    private int drinkCount;
    private int recycleCount;
    //private int wasteCount;

    GameObject nearest_bottle;
    GameObject carried_bottle;
    Vector3 bottleCarryOffset;

    GameObject gameManager;

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

        bottleHeight = bottle.GetComponent<BoxCollider2D>().size.y * bottle.GetComponent<BoxCollider2D>().transform.localScale.y;
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

        if (Input.GetKeyDown("r") && !hasBottle)
        {
            anim.SetTrigger("drink");
            CreateBottle();
        }

        if (Input.GetKeyDown("e"))
        {
            InteractBottle();
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
            anim.SetTrigger("place");
            // PlaceBottle() function called during animation
        }
    }

    void CheckPickupBottle()
    {
        float shortest_distance_bottle = 100;

        // Iterates through all game objects and gets those with tag Bottle
        foreach (GameObject bottle in bottles)
        {

            // Calculates the distance between the player and the current bottle being checked
            float working_diff = Mathf.Abs(transform.position.x - bottle.transform.position.x);

            // If the current bottle is nearer to the player than the previous nearest, it replaces that bottle
            if (working_diff < shortest_distance_bottle)
            {
                shortest_distance_bottle = working_diff;
                nearest_bottle = bottle;
            }
        }

        // If the nearest bottle is within the specified distance destroy it and change the player hasBottle state
        if (shortest_distance_bottle < 2 && nearest_bottle != null)
        {
            anim.SetTrigger("pickup");
        }
    }

    public void PickupBottle()
    {
        bottles.Remove(nearest_bottle);
        carried_bottle = nearest_bottle;
        carried_bottle.GetComponent<Bottle>().SetBeingCarried(true);
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        carried_bottle.transform.position = bottleCarryOffset;

        //wasteCount = gameManager.GetComponent<GameManager>().GetWasteCount();
        //wasteCount--;
        //gameManager.GetComponent<GameManager>().UpdateWasteCount(wasteCount);

        hasBottle = true;
    }

    public void PlaceBottle()
    {
        // Calculate distance between player and bin objects
        foreach (GameObject bin in bins) {
            float diff = (transform.position - bin.transform.position).magnitude;

            // If there is a bin nearby
            if (diff < binDetectionDistance && diff > -binDetectionDistance)
            {
                // Maybe play an animation on the bin? At least increment recycled counter
                hasBottle = false;
                recycleCount++;
                gameManager.GetComponent<GameManager>().UpdateRecycleCount(recycleCount);
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
                // Get the bottle collider
                bottle_collider = b.GetComponent<BoxCollider2D>();

                // Check if the bottle is at the x coordinate of where we want to place our bottle
                // bool bottleAtXOfPlacementPosition = (placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x || placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x);
                bool bottleAtXOfPlacementPosition = (placement_coordinates.x <= bottle_collider.bounds.max.x && placement_coordinates.x >= bottle_collider.bounds.min.x);
                if (bottleAtXOfPlacementPosition)
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

            if (top_of_stack != null)
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
                    // Put bottle on top of stack?
                    carried_bottle.GetComponent<Bottle>().SetBeingCarried(false);
                    carried_bottle.transform.position = new Vector3(top_of_stack.transform.position.x, top_of_stack.transform.position.y + (bottle_collider.size.y * bottle_collider.transform.localScale.y), 0);
                    bottles.Add(carried_bottle);
                    hasBottle = false;
                    
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
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), carried_bottle.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
                carried_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
                bottles.Add(carried_bottle); // Adds the bottle to the list of bottles in the world
                hasBottle = false;
                carried_bottle = null;
                return;
            }
        }
        else
        {
            // Throw the bottle
            carried_bottle.GetComponent<Bottle>().SetBeingCarried(false); // Makes the bottles non-kinematic and adds the collider. Also sets mass and drag to default
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), carried_bottle.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
            carried_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
            bottles.Add(carried_bottle); // Adds the bottle to the list of bottles in the world
            hasBottle = false;
            carried_bottle = null;

            // Create a new bottlestack and add the new bottle to it
            /*BottleStack bottleStack = new BottleStack();
            bottleStack.bottles.Add(carried_bottle);
            bottleStack.xmin = carried_bottle.GetComponent<Collider2D>().bounds.min.x;
            bottleStack.xmax = carried_bottle.GetComponent<Collider2D>().bounds.max.x;*/
            return;
        }
    }

    public void CreateBottle()
    {
        drinkCount++;
        gameManager.GetComponent<GameManager>().UpdateDrinkCount(drinkCount);

        carried_bottle = bottle;
        carried_bottle.GetComponent<Bottle>().SetBeingCarried(true);  // Makes the bottles kinematic and removes the collider.  Also sets mass and drag to default
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        carried_bottle.transform.position = bottleCarryOffset;
        carried_bottle = Instantiate(carried_bottle);

        hasBottle = true;
    }

}
