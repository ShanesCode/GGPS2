using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public GameObject bottle;
    public GameObject bin;
    public bool hasBottle;
    public List<GameObject> bottles;
    private Animator anim;
    int flip;
    int drinkCount;

    GameObject nearest_bottle;
    Vector3 bottleCarryOffset;

    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        drinkCount = gameManager.GetComponent<GameManager>().GetDrinkCount();

        hasBottle = false;
        anim = GetComponent<Animator>();
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

        if (hasBottle && nearest_bottle != null)
        {
            bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
            nearest_bottle.transform.position = bottleCarryOffset;
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
        nearest_bottle.GetComponent<Bottle>().SetBeingCarried(true);
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        nearest_bottle.transform.position = bottleCarryOffset;

        hasBottle = true;
    }

    public void PlaceBottle()
    {
        // Calculate distance between player and bin objects
        float diff = transform.position.x - bin.transform.position.x;

        // If there is a bin nearby
        if (diff < 2 && diff > -2)
        {
            // Maybe play an animation on the bin? At least increment recycled counter
            hasBottle = false;
        }
        else
        {
            // Either throw a bottle or add it onto a stack, if there's a stack in front of you
            Vector3 placement_coordinates = transform.position + new Vector3(flip * 1, 0, 0);
            BoxCollider2D bottle_collider;
            GameObject top_of_stack = null;
            if (bottles.Count > 0)
            {
                // This foreach loop is used to check how big the stack of bottles is
                // I think it might also prevent the player form stacking above some value
                foreach (GameObject go in bottles)
                {
                    bottle_collider = go.GetComponent<BoxCollider2D>();
                    if (placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x + ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x || placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) <= bottle_collider.bounds.max.x && placement_coordinates.x - ((bottle_collider.size.x * bottle_collider.transform.localScale.x) / 2) >= bottle_collider.bounds.min.x)
                    {
                        if (top_of_stack != null)
                        {
                            if (go.transform.position.y > top_of_stack.transform.position.y)
                            {
                                top_of_stack = go;
                            }
                        }
                        else
                        {
                            top_of_stack = go;
                        }
                    }
                }
                if (top_of_stack != null)
                {
                    bottle_collider = top_of_stack.GetComponent<BoxCollider2D>();
                    if (top_of_stack.transform.position.y + (bottle_collider.size.y * bottle_collider.transform.localScale.y) > 2 * (bottle_collider.size.y * bottle_collider.transform.localScale.y))
                    {
                        hasBottle = true;
                    }
                    else
                    {
                        // Put bottle on top of stack?
                        nearest_bottle.GetComponent<Bottle>().SetBeingCarried(false);
                        nearest_bottle.transform.position = new Vector3(top_of_stack.transform.position.x, top_of_stack.transform.position.y + (bottle_collider.size.y * bottle_collider.transform.localScale.y), 0);
                        bottles.Add(nearest_bottle);
                        hasBottle = false;
                    }

                }
                else
                {
                    // Throw the bottle
                    nearest_bottle.GetComponent<Bottle>().SetBeingCarried(false); // Makes the bottles non-kinematic and adds the collider. Also sets mass and drag to default
                    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), nearest_bottle.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
                    nearest_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
                    bottles.Add(nearest_bottle); // Adds the bottle to the list of bottles in the world
                    hasBottle = false;
                }
            }

            else
            {
                // Throw the bottle
                nearest_bottle.GetComponent<Bottle>().SetBeingCarried(false); // Makes the bottles non-kinematic and adds the collider. Also sets mass and drag to default
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), nearest_bottle.GetComponent<Collider2D>(), true); // Prevents the bottle colliding with the player
                nearest_bottle.GetComponent<Bottle>().ChuckBottle(); // Gives the bottle velocity
                bottles.Add(nearest_bottle); // Adds the bottle to the list of bottles in the world
                hasBottle = false;
            }
        }
    }

    public void CreateBottle()
    {
        drinkCount++;
        gameManager.GetComponent<GameManager>().UpdateDrinkCount(drinkCount);

        nearest_bottle = bottle;
        nearest_bottle.GetComponent<Bottle>().SetBeingCarried(true);  // Makes the bottles kinematic and removes the collider.  Also sets mass and drag to default
        bottleCarryOffset = new Vector3(transform.position.x + (gameObject.GetComponent<BoxCollider2D>().bounds.size.x * flip) / 2, transform.position.y + 1, transform.position.z);
        nearest_bottle.transform.position = bottleCarryOffset;
        nearest_bottle = Instantiate(nearest_bottle);

        hasBottle = true;
    }

}
