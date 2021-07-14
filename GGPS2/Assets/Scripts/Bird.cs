using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject head;
    public Sprite squashedHead;
    
    public GameObject wing;
    public Sprite wingUp;
    public Sprite wingMiddle;
    public Sprite wingDown;
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
