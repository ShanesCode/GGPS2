using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int levelNumber;

    const int FINAL_LEVEL = 2;

    public GameObject player;
    public GameObject camera;
    GameObject gameManager;
    GameObject musicManager;
    public AudioClip song;
    public int currentRoom;
    [SerializeField] public int nextRoomNumber;
    [SerializeField] private List<EndRoomTrigger> endRoomTriggers;
    [SerializeField] private List<GameObject> roomSpawnPoints;
    [SerializeField] private GameObject endRoomUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settings;

    public int roomWasteCount;
    public int roomIndulgenceCount;
    public int roomRecycleCount;

    public bool paused;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level")) {
            levelNumber = int.Parse(SceneManager.GetActiveScene().name.Remove(0, 5));
        }
        else
        {
            levelNumber = FINAL_LEVEL + 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "LevelManager";
        gameManager = GameObject.FindWithTag("GameManager");

        musicManager = GameObject.FindWithTag("MusicManager");
        musicManager.GetComponent<AudioSource>().clip = song;
        musicManager.GetComponent<AudioSource>().Play();

        Time.timeScale = 1;
        paused = false;

        for (int i = 0; i < endRoomTriggers.Count; i++)
        {
            endRoomTriggers[i].GetComponent<EndRoomTrigger>().OnTrigger += LevelManager_OnTrigger;
        }

        camera = GameObject.FindWithTag("MainCamera");

        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().OnDeath += LevelMan_OnDeath;

        player.transform.position = roomSpawnPoints[gameManager.GetComponent<GameManager>().spawnRoom].transform.position;
        currentRoom = gameManager.GetComponent<GameManager>().spawnRoom;

        roomWasteCount = 0;
        roomIndulgenceCount = 0;
        roomRecycleCount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    private void LevelManager_OnTrigger(object sender, EndRoomTrigger.OnTriggerEventArgs e)
    {
        // Open End Room UI
        Time.timeScale = 0;
        paused = true;
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
        Time.timeScale = 1;
        paused = false;
        endRoomUI.SetActive(false);

        if (nextRoomNumber < roomSpawnPoints.Count)
        {
            player.transform.position = roomSpawnPoints[nextRoomNumber].transform.position;
            ResetRoomCounters();
            camera.GetComponent<CameraController>().InitialisePosition();
            currentRoom += 1;
        }
        else
        {
            if (levelNumber < FINAL_LEVEL)
            {
                switch (levelNumber)
                {
                    case 0:
                        gameManager.GetComponent<GameManager>().UpdateTutorialComplete(true);
                        break;
                    case 1:
                        gameManager.GetComponent<GameManager>().UpdateLevel1Complete(true);
                        break;
                }

                gameManager.GetComponent<GameManager>().SetSpawnRoom(0);
                StartCoroutine(LoadYourAsyncScene("Level" + (levelNumber + 1)));
            }
            else
            {
                gameManager.GetComponent<GameManager>().UpdateGameComplete(true);
                StartCoroutine(LoadYourAsyncScene("EndScene"));
            }
        }
    }

    public void ResetRoom()
    {
        // Reset the level, remove the bottles
        /*GameObject[] bottles = GameObject.FindGameObjectsWithTag("Bottle");
        foreach(GameObject b in bottles)
        {
            Destroy(b);
        }*/
        endRoomUI.SetActive(false);
        gameManager.GetComponent<GameManager>().spawnRoom = currentRoom;
        ResetRoomCounters();
        StartCoroutine(LoadYourAsyncScene(SceneManager.GetActiveScene().name));
    }

    public void GoToMainMenu()
    {
        // Go to main menu
        endRoomUI.SetActive(false);
        StartCoroutine(LoadYourAsyncScene("MainMenuScene"));
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        pauseMenu.SetActive(true);
    }

    public void TogglePause()
    {
        if (!paused)
        {
            PauseGame();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            settings.SetActive(false);
            paused = false;
        }
    }
    private void LevelMan_OnDeath(object sender, PlayerController.OnDeathEventArgs e)
    {
        ResetRoom();
        ResetRoomCounters();
    }

    public void ResetRoomCounters()
    {
        roomWasteCount = 0;
        roomIndulgenceCount = 0;
        roomRecycleCount = 0;
    }
}
