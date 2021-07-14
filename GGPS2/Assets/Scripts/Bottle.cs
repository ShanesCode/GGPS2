using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public GameObject bottle;
    public GameObject bin;
    public bool hasBottle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left ctrl"))
        {
            PlaceBottle();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Stay");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Beep Boop Exit");
        }
    }
    void PlaceBottle()
    {
        print(hasBottle);
        print(bin.transform.position.x);
        print(transform.position.x);
        if (hasBottle == false)
        {
            Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation);
        }
        if (hasBottle == true)
        {
            // Calculate distance between player and bin objects
            float diff = transform.position.x - bin.transform.position.x;
            if (diff < 2 && diff > -2)
            {
                print("this is working");
                hasBottle = false;
            }
            else
            {
                hasBottle = false;
                Instantiate(bottle, transform.position + new Vector3(2, 0, 0), transform.rotation);
            }
        }
    }
}
