using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject achievements;
    public GameObject settings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void GoToLevel(int level)
    {
        StartCoroutine(LoadYourAsyncScene("Level" + level));
    }

    public void GoToLevelSelect()
    {
        // Show level select UI
        levelSelect.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToAchievements()
    {
        // Show achievements UI
        achievements.SetActive(true);
        gameObject.SetActive(false);
    }

    public void GoToSettings()
    {
        // Show settings UI
        settings.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
