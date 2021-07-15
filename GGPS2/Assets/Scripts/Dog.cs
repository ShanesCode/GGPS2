using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public GameObject head;
    public Sprite squashedHead;
    private MovingPlatform mp;
    private float speed;
    public GameObject player;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().OnGrounded += Dog_OnGrounded;
        mp = GetComponent<MovingPlatform>();
        anim = GetComponent<Animator>();
        speed = mp.speed;
    }

    private void Dog_OnGrounded(object sender, PlayerController.OnGroundedEventArgs e)
    {
        if (e.groundTransform == gameObject.transform)
        {
            anim.SetTrigger("squashed");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SquashHead()
    {
        head.GetComponent<SpriteRenderer>().sprite = squashedHead;
    }

    public void ZeroSpeed()
    {
        mp.speed = 0;
    }

    public void SetSpeedToDefault()
    {
        mp.speed = speed;
    }
}
