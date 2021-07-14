using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public GameObject parentMenu;
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
        parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void showUnlockedAchievement()
    {
        // for all achievements, get the unlocked ones and set the alpha to 1
        // achievement.color = new Color(achievement.color.r, achievement.color.g, achievement.color.b, 1);
    }
}
