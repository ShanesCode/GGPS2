using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementInfo : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToParent()
    {
        parent.SetActive(true);
        gameObject.SetActive(false);
    }
}
