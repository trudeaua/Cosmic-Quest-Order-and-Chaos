using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    /// <summary>
    /// Start the tutorial level
    /// </summary>
    public void StartTutorial()
    {
        // Load Tutorial scene
        StartCoroutine(LoadYourAsyncScene("Tutorial"));
    }

    /// <summary>
    /// Start level 1
    /// </summary>
    public void StartLevel() 
    {
        // Load Level 1
        StartCoroutine(LoadYourAsyncScene("ChaosVoid1"));
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public static IEnumerator BackToMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuStaging");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Load a scene asynchronously
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}