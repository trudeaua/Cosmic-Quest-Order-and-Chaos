using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum SceneType {
        Level,
        Menu,
        Map
    }
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

    // Loading screen to use for scene transitions
    public GameObject loadingScene;
    public UnityEvent loadingDoneEvent = new UnityEvent();
    
    // List of all chaos voids
    public ChaosVoid[] chaosVoids;

    private Animator _anim;

    public ChaosVoid activeLevel;


    private void Start()
    {
        _anim = loadingScene.GetComponent<Animator>();
    }

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
        StartCoroutine(LoadYourAsyncScene("Tutorial", SceneType.Level));
    }
    
    public void StartTestLevel()
    {
        // TODO DELETE ME
        StartCoroutine(LoadYourAsyncScene("ChaosVoid1", SceneType.Level));
    }

    /// <summary>
    /// Go to the level menu scene
    /// </summary>
    public void StartLevelMenu()
    {
        // TODO if tutorial is skipped then should be taken to the level selection map
        StartCoroutine(LoadYourAsyncScene("LevelsScene", SceneType.Map));
    }

    /// <summary>
    /// Starts a given chaos void
    /// </summary>
    /// <param name="chaosVoid">Reference to the chaos void level to load</param>
    public void StartChaosVoid(ChaosVoid chaosVoid)
    {
        StartCoroutine(LoadYourAsyncScene(chaosVoid.scene.name, SceneType.Level));
        chaosVoid.Initialize();
        activeLevel = chaosVoid;
    }

    /// <summary>
    /// Marks a given chaos void as cleared
    /// </summary>
    /// <param name="chaosVoid">Reference to the chaos void level to load</param>
    public void ClearChaosVoid()
    {
        if (activeLevel == null)
        {
            activeLevel = Array.Find(chaosVoids, (level) => level.scene.name == SceneManager.GetActiveScene().name);
        }
        activeLevel.cleared = true;
    }

    /// <summary>
    /// Start next level
    /// </summary>
    /// <param name="chaosVoid">Reference to the chaos void level to load</param>
    public void StartNextLevel()
    {
        int index = Array.FindIndex(chaosVoids, (level) => level.scene.name == SceneManager.GetActiveScene().name);
        if (index >= 0)
        {
            StartChaosVoid(chaosVoids[index+1]);
        }
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartCurrentLevel()
    {
        StartCoroutine(LoadYourAsyncScene(SceneManager.GetActiveScene().name, SceneType.Level));
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
        StartCoroutine(LoadYourAsyncScene("MenuStaging", SceneType.Menu));
    }

    /// <summary>
    /// Load a scene asynchronously
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <param name="isLevel">Whether the scene being loaded is a level</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator LoadYourAsyncScene(string sceneName, SceneType sceneType = SceneType.Menu)
    {
        GameManager.Instance.SetLoadingState();
        
        // Start the loading screen
        _anim.SetTrigger("Show");

        // Set time back to 1 for when players pause the game and go back to the main menu
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.5f);

        // check if scene has already been loaded, if it has then set it to active
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        
        if (loadedScene.IsValid() && sceneName != SceneManager.GetActiveScene().name)
        {
            SceneManager.SetActiveScene(loadedScene);
            yield break;
        }
        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Hide the loading screen
        _anim.SetTrigger("Hide");
        Debug.Log("SceneLoaded");
        // Set the new game state after loading is finished
        if (sceneType == SceneType.Level)
            GameManager.Instance.SetPlayState();
        else if (sceneType == SceneType.Menu)
            GameManager.Instance.SetMenuState();
        else if (sceneType == SceneType.Map)
            GameManager.Instance.SetSelectingLevelState();
        loadingDoneEvent.Invoke();
        yield return new WaitForSeconds(0.5f);
    }
}
