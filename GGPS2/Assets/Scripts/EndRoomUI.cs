using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndRoomUI : MonoBehaviour
{
    public TextMeshProUGUI stats;
    public int wasteCount;
    public int indulgenceCount;
    public int recycleCount;
    public int devCount;

    private int prevRoomWasteCount;
    private int prevRoomIndulgenceCount;
    private int prevRoomRecycleCount;

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

        levelNumber = int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5));

        prevRoomWasteCount = levelManager.GetComponent<LevelManager>().startWasteCount;
        prevRoomIndulgenceCount = levelManager.GetComponent<LevelManager>().startDrinkCount;
        prevRoomRecycleCount = levelManager.GetComponent<LevelManager>().startRecycleCount;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (gameManager != null)
        {
            roomNumber = levelManager.GetComponent<LevelManager>().currentRoom;

            wasteCount = gameManager.GetComponent<GameManager>().GetWasteCount() - prevRoomWasteCount;
            indulgenceCount = gameManager.GetComponent<GameManager>().GetDrinkCount() - prevRoomIndulgenceCount;
            recycleCount = gameManager.GetComponent<GameManager>().GetRecycleCount() - prevRoomRecycleCount;
        }

        stats.text =
            "Dumped litter:\t\t\t\t" + wasteCount + '\t' + " times" + '\n' +
            "Indulged gluttonously:\t\t" + indulgenceCount + '\t' + " times" + '\n' +
            "Thoughtfully recycled:\t" + recycleCount + '\t' + " times" + '\n' + '\n' +
            "Dev's best:\t\t\t\t\t" + devCount + '\t' + " bottles dumped" + '\n';

        title.text = "Room " + levelNumber + "-" + roomNumber + " Complete!";

        prevRoomWasteCount = wasteCount;
        prevRoomIndulgenceCount = indulgenceCount;
        prevRoomRecycleCount = recycleCount;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
