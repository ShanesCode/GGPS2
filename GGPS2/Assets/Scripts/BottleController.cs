using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public GameObject bottle;
    public GameObject bin;
    public bool hasBottle;

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
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();

            // Iterates through all game objects and gets those with tag Bottle
            foreach (GameObject go in allObjects)
                if (go.CompareTag("Bottle"))
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
                Destroy(nearest_bottle);
                hasBottle = true;
            }

            // If there is no bottle yet placed or no bottle within range, place a new bottle
            else
            {
                Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation);
                hasBottle = false;
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
                hasBottle = false;
                Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation);
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

    //void CheckSpace()
    //{
    //    Vector3 placement_coordinates = transform.position + new Vector3(2, 0, 0);
    //    float check_coordinates;
    //    BoxCollider2D bottle_collider;
    //    Vector2 collider_size;
    //    float range_x;
    //    float range_y;
    //    foreach (GameObject go in allObjects)
    //        if (go.tag == "Bottle")
    //        {
    //            check_coordinates = go.transform.position.x - placement_coordinates.x;
    //            bottle_collider = go.GetComponent<BoxCollider2D>();
    //            collider_size = bottle_collider.size;
    //            range_x = go.transform.position.x - (collider_size.x / 2);
    //            range_y = go.transform.position.y - (collider_size.y / 2);
    //            if (check_coordinates < (range_x + collider_size.x) ^ check_coordinates > range_x)
    //            {
    //                Instantiate(bottle, transform.position + new Vector3(2, collider_size.y, 0), transform.rotation);
    //            }
    //            else
    //            {
    //                Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation);
    //            }


    //        }

    //    hasBottle = false;
    //}

}
