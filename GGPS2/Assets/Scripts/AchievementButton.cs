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

    private GameObject gameManager;
    private Achievement achievement;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        achievement = gameManager.GetComponent<GameManager>().achievements[achievementID];
        
        if (!achievement.achieved)
        {
            gameObject.GetComponent<Button>().interactable = false;
            gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.r, 0.2f);
        } else
        {
            gameObject.GetComponent<Button>().interactable = true;
            gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.r, 1.0f);
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
