using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsMenuUI : MonoBehaviour
{
    private List<Achievement> achievements = new List<Achievement>();
    private List<Image> achievementImages = new List<Image>();
    private List<string> achievementTitles = new List<string>();

    public GameManager gameManager = new GameManager();
    public GameObject parentMenu;
    public GameObject achievementPanel;
    // Start is called before the first frame update
    void Start()
    {
        achievements = gameManager.achievements;
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
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i].achieved == true)
            {
                // Need reference to the associated image
                // achievement.color = new Color(achievement.color.r, achievement.color.g, achievement.color.b, 1);
            }
        }
    }
}
