using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUnlocked : MonoBehaviour
{
    public GameObject achievementUnlockedUI;
    public TextMeshProUGUI achievement;
    public TextMeshProUGUI requirement;
    public Image image;

    [Range(0.0f, 5.0f)]public float displayTime = 3.0f;
    private float displayReset;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnAchievementUnlocked += GameManager_OnAchievementUnlocked;
        displayReset = displayTime;
    }

    private void GameManager_OnAchievementUnlocked(object sender, GameManager.OnAchievementUnlockedEventArgs e)
    {
        achievementUnlockedUI.SetActive(true);
        achievement.text = e.title;
        requirement.text = e.requirement;
        image.sprite = e.sprite;

        displayTime = displayReset;
    }

    // Update is called once per frame
    void Update()
    {
        displayTime -= Time.deltaTime;
        if (displayTime <= 0)
        {
            achievementUnlockedUI.SetActive(false);
            achievement.text = "";
            requirement.text = "";
            image.sprite = null;
        }
    }
}
