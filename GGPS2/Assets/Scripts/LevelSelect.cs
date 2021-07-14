using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject parentMenu;
    GameManager gameManager = new GameManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameManagerSpawnRoom(int roomNumber)
    {
        gameManager.SetSpawnRoom(roomNumber);
    }

    public void ReturnToParent()
    {
        parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToLevel(int selectedLevel)
    {
        StartCoroutine(LoadYourAsyncScene("Level" + selectedLevel));
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
}
