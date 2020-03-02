using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Only one scene manager should be in the scene!");
            Destroy(this);
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion
    
    // List of all chaos voids
    public ChaosVoid[] chaosVoids;

    public void LoadGame(string saveData)
    {
        // TODO should load players to the last saved level (or take them to the level selection map otherwise)
        // TODO when loading a game, we need a menu to have players connect their controllers to ensure players are mapped correctly
        throw new System.NotImplementedException();
    }
    
    /// <summary>
    /// Start the tutorial level
    /// </summary>
    public void StartTutorial()
    {
        // Load Tutorial scene
        StartCoroutine(LoadYourAsyncScene("Tutorial", true));
    }
    
    public void StartLevel1()
    {
        // TODO DELETE ME AND THIS SCENE
        StartCoroutine(LoadYourAsyncScene("ChaosVoid1", true));
    }

    /// <summary>
    /// Go to the level menu scene
    /// </summary>
    public void StartLevelMenu()
    {
        // TODO if tutorial is skipped then should be taken to the level selection map
        StartCoroutine(LoadYourAsyncScene("LevelMenu"));
    }

    /// <summary>
    /// Starts a given chaos void
    /// </summary>
    /// <param name="chaosVoid">Reference to the chaos void level to load</param>
    public void StartChaosVoid(ChaosVoid chaosVoid)
    {
        StartCoroutine(LoadYourAsyncScene(chaosVoid.scene.name, true));
        chaosVoid.Initialize();
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
        StartCoroutine(LoadYourAsyncScene("MenuStaging"));
    }

    /// <summary>
    /// Load a scene asynchronously
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <param name="isLevel">Whether the scene being loaded is a level</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator LoadYourAsyncScene(string sceneName, bool isLevel = false)
    {
        GameManager.Instance.SetLoadingState();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Set the new game state after loading is finished
        if (isLevel)
            GameManager.Instance.SetPlayState();
        else
            GameManager.Instance.SetMenuState();
    }
}
