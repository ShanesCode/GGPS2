using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public GameObject bottle;
    public GameObject bin;
    public bool hasBottle;
    public List<GameObject> bottles;

    // Start is called before the first frame update
    void Start()
    {
        hasBottle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            CreateBottle();
        }

        if (Input.GetKeyDown("e"))
        {
            PlaceBottle();
        }
    }
    void PlaceBottle()
    {
        if (hasBottle == false)
        {
            GameObject nearest_bottle = null;
            float shortest_distance_bottle = 100;
            
            // Iterates through all game objects and gets those with tag Bottle
            foreach (GameObject go in bottles)
             {
                    
                // Calculates the distance between the player and the current bottle being checked
                float working_diff = Mathf.Abs(transform.position.x - go.transform.position.x);
                    
                // If the current bottle is nearer to the player than the previous nearest, it replaces that bottle
                if (working_diff < shortest_distance_bottle)
                {
                    shortest_distance_bottle = working_diff;
                    nearest_bottle = go;
                }
             }

            // If the nearest bottle is within the specified distance destroy it and change the player hasBottle state
            if (shortest_distance_bottle < 2 && nearest_bottle != null)
            {
                bottles.Remove(nearest_bottle);
                Destroy(nearest_bottle);
                hasBottle = true;
            }
        }
        else 
        {
            // Calculate distance between player and bin objects
            float diff = transform.position.x - bin.transform.position.x;
            if (diff < 2 && diff > -2)
            {
                hasBottle = false;
            }
            else
            {
                
                Vector3 placement_coordinates = transform.position + new Vector3(2, 0, 0);
                BoxCollider2D bottle_collider;
                GameObject top_of_stack = null;
                if (bottles.Count > 0)
                {
                    foreach (GameObject go in bottles)
                        {
                            bottle_collider = go.GetComponent<BoxCollider2D>();
                            if (placement_coordinates.x + (bottle_collider.size.x / 2) < bottle_collider.bounds.max.x && placement_coordinates.x + (bottle_collider.size.x / 2) > bottle_collider.bounds.min.x || placement_coordinates.x - (bottle_collider.size.x / 2) < bottle_collider.bounds.max.x && placement_coordinates.x - (bottle_collider.size.x / 2) > bottle_collider.bounds.min.x)
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
                        bottles.Add(Instantiate(bottle, new Vector3(top_of_stack.transform.position.x, top_of_stack.transform.position.y + (bottle_collider.size.y + 1), 0), transform.rotation));
                    }
                    else
                    {
                        bottles.Add(Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation));
                    }
                }
                    
                else
                {
                    bottles.Add(Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation));
                }


                hasBottle = false;
            }
        }
    }

    void CreateBottle()
    {
        if (hasBottle == false)
            {
                hasBottle = true;
            } 
    }

}
