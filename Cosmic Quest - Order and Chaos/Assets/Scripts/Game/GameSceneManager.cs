using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    #region Singleton
    private static GameSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Only one scene manager should be in the scene!");
        }
    }
    #endregion

    /// <summary>
    /// Start the tutorial level
    /// </summary>
    public void StartTutorial()
    {
        // Load Tutorial scene
        //Instance.StartCoroutine(LoadYourAsyncScene("Tutorial"));
        LoadYourScene("Tutorial");
    }

    /// <summary>
    /// Start level 1
    /// </summary>
    public void StartLevel1()
    {
        // Load Level 1
        //Instance.StartCoroutine(LoadYourAsyncScene("ChaosVoid1"));
        LoadYourScene("ChaosVoid1");
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void BackToMenu()
    {
        //_instance.StartCoroutine(LoadYourAsyncScene("MenuStaging"));
        LoadYourScene("MenuStaging");

    }

    /// <summary>
    /// Load a scene asynchronously in the background
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

    /// <summary>
    /// Load a scene synchronously
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    private void LoadYourScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
