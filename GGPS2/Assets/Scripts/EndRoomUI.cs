using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndRoomUI : MonoBehaviour
{
    public TextMeshProUGUI stats;

    public List<int> devCount;

    public TextMeshProUGUI title;
    public int levelNumber;
    public int roomNumber;

    public GameObject levelManager;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindWithTag("LevelManager");
        gameManager = GameObject.FindWithTag("GameManager");

        stats = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        title = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        levelNumber = levelManager.GetComponent<LevelManager>().levelNumber;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (gameManager != null)
        {
            roomNumber = levelManager.GetComponent<LevelManager>().currentRoom;

            if ((levelManager.GetComponent<LevelManager>().roomWasteCount < levelManager.GetComponent<LevelManager>().roomRecycleCount) && !gameManager.GetComponent<GameManager>().achievementsDic["carbon cutter"].achieved)
            {
                gameManager.GetComponent<GameManager>().UnlockAchievement("Carbon Cutter");
            }
        }

        stats.text =
            "Indulged gluttonously:\t\t" + levelManager.GetComponent<LevelManager>().roomIndulgenceCount + '\t' + " times" + '\n' +
            "Thoughtfully recycled:\t" + levelManager.GetComponent<LevelManager>().roomRecycleCount + '\t' + " times" + '\n' + '\n' +
            "Waste score:\t\t\t\t" + levelManager.GetComponent<LevelManager>().roomWasteCount + '\t' + '\n' +
            "Dev's best:\t\t\t\t\t" + devCount[roomNumber] + '\t';

        title.text = "Room " + levelNumber + "-" + roomNumber + " Complete!";
    }
}
