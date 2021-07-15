using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject head;
    public Sprite squashedHead;
    public GameObject player;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().OnGrounded += Bird_OnGrounded;
        anim = GetComponent<Animator>();
    }

    private void Bird_OnGrounded(object sender, PlayerController.OnGroundedEventArgs e)
    {
        if (e.groundTransform == gameObject.transform)
        {
            anim.SetTrigger("squashed");
        }
    }

    public void SquashHead()
    {
        head.GetComponent<SpriteRenderer>().sprite = squashedHead;
    }
}
