using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    [SerializeField] public int nextRoomNumber;
    [SerializeField] private List<EndRoomTrigger> endRoomTriggers;
    [SerializeField] private List<GameObject> roomSpawnPoints;
    [SerializeField] private GameObject endRoomUI;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < endRoomTriggers.Count; i++)
        {
            endRoomTriggers[i].GetComponent<EndRoomTrigger>().OnTrigger += LevelManager_OnTrigger;
        }
    }

    private void LevelManager_OnTrigger(object sender, EndRoomTrigger.OnTriggerEventArgs e)
    {
        // Open End Room UI
        Debug.Log("Room complete: " + e.roomNumber);
        endRoomUI.SetActive(true);
        nextRoomNumber = e.roomNumber;
    }

    IEnumerator LoadYourAsyncScene(string scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void GoToNextRoom()
    {
        // Go to next room
        endRoomUI.SetActive(false);
        player.transform.position = roomSpawnPoints[nextRoomNumber].transform.position;
    }

    public void ResetLevel()
    {
        // Reset the level, remove the bottles
        endRoomUI.SetActive(false);
        StartCoroutine(LoadYourAsyncScene(SceneManager.GetActiveScene().name));
    }

    public void GoToMainMenu()
    {
        // Go to main menu
        endRoomUI.SetActive(false);
        StartCoroutine(LoadYourAsyncScene("MainMenuScene"));
    }
}
