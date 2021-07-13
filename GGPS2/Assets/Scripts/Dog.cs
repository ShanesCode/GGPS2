using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject head;
    public Sprite squashedHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SquashHead()
    {
        head.GetComponent<SpriteRenderer>().sprite = squashedHead;
    }
}
