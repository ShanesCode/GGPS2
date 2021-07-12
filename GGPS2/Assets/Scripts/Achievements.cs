using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    private int jumpCount;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerController.OnJumped += PlayerController_OnJumped;

        jumpCount = 0;
    }

    private void PlayerController_OnJumped(object sender, PlayerController.OnJumpedEventArgs e)
    {
        jumpCount = e.jumpCount;
        GiveAchievement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GiveAchievement()
    {
        Debug.Log("Achievement unlocked!" + '\n' + "Jumped " + jumpCount);
    }
}
