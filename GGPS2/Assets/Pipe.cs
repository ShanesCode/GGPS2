using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject bottle;
    GameObject gameManager;
    int bottleCount;
    Vector3 spawnPos;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        if (gameManager != null)
        {
            bottleCount = gameManager.GetComponent<GameManager>().GetWasteCount();
        } else
        {
            bottleCount = 50;
        }
        spawnPos = new Vector3(-44.40639f, 18.8f, 0.0f);
        anim = gameObject.GetComponent<Animator>();
    }

    public void ShootBottle()
    {
        Instantiate(bottle, spawnPos, Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)));
        bottleCount--;

        if (bottleCount <= 0)
        {
            anim.SetTrigger("idle");
        }
    }
}
