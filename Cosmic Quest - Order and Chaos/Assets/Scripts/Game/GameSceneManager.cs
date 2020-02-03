using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject loadingScreen;
    public Slider slider;
    public TMPro.TextMeshProUGUI progressText;
    private bool isSceneLoading = false;

    /// <summary>
    /// Start the tutorial level
    /// </summary>
    public void StartTutorial()
    {
        // Load Tutorial scene
        //Instance.StartCoroutine(LoadYourAsyncScene("Tutorial"));
        StartCoroutine(LoadYourAsyncScene("Tutorial"));
    }

    /// <summary>
    /// Start level 1
    /// </summary>
    public void StartLevel1()
    {
        // Load Level 1
        //Instance.StartCoroutine(LoadYourAsyncScene("ChaosVoid1"));
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

    public void BackToMenu()
    {
        //_instance.StartCoroutine(LoadYourAsyncScene("MenuStaging"));
        StartCoroutine(LoadYourAsyncScene("MenuStaging"));

    }

    /// <summary>
    /// Load a scene asynchronously in the background
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        if (!isSceneLoading)
        {
            isSceneLoading = true;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            loadingScreen.SetActive(true);
            float progress = 0;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                progress = Mathf.Clamp01(asyncLoad.progress / .9f);
                progressText.text = (Mathf.RoundToInt(progress * 100)).ToString() + "%";
                slider.value = progress;
                yield return null;
            }
            yield return new WaitForSeconds(1);
            loadingScreen.SetActive(false);
            isSceneLoading = false;
        }
        else
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
