using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementButton : MonoBehaviour
{
    public int achievementID;
    public GameObject achievementInfo;
    public GameObject achievementsPanel;
    public TextMeshProUGUI achievementTitle;
    public TextMeshProUGUI achievementRequirement;
    public TextMeshProUGUI achievementFlavour;
    public Image achievementImage;

    public GameManager gameManager = new GameManager();
    private Achievement achievement;
    // Start is called before the first frame update
    void Start()
    {
        achievement = gameManager.achievements[achievementID];
        
        if (!achievement.achieved)
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, 0.5f);
        } else
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToAchievementInfo()
    {
        achievementInfo.SetActive(true);
        achievementsPanel.SetActive(false);

        achievementTitle.text = achievement.title;
        achievementRequirement.text = achievement.requirement;
        achievementFlavour.text = achievement.flavourText;
        achievementImage.sprite = achievement.sprite;
    }
}
