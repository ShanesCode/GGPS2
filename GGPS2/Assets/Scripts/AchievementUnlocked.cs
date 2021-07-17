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

    [Range(0.0f, 10.0f)]public float displayTime = 3.0f;

    GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        achievementUnlockedUI = transform.GetChild(0).transform.gameObject;
        achievement = transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        requirement = transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        image = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        gameManager = GameObject.FindWithTag("GameManager");
        gameManager.GetComponent<GameManager>().OnAchievementUnlocked += AchievementUnlocked_OnAchievementUnlocked;
    }

    private void AchievementUnlocked_OnAchievementUnlocked(object sender, GameManager.OnAchievementUnlockedEventArgs e)
    {
        achievementUnlockedUI.SetActive(true);
        achievement.text = e.title;
        requirement.text = e.requirement;
        image.sprite = e.sprite;

        StartCoroutine(AchievementDisplayCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AchievementDisplayCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSecondsRealtime(displayTime);

        achievementUnlockedUI.SetActive(false);
        achievement.text = "";
        requirement.text = "";
        image.sprite = null;
        displayTime = 0;

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
