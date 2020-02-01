using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Paused,
        Playing,
        GameOver
    }
    
    #region Singleton
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one GameManager should be in the scene!");
    }
    #endregion

    public GameState currentState { get; private set; } = GameState.Menu;

    private void Start()
    {
        // This code simply updates the initial state based upon the scene the game was started in for testing purposes
        // TODO remove this when no longer needed
        string activeScene = SceneManager.GetActiveScene().name;
        currentState = activeScene.Equals("MenuStaging") ? GameState.Menu : GameState.Playing;
    }

    private void Update()
    {
        // Polls for game events and handles any state changes
        switch (currentState)
        {
            case GameState.Menu:
            case GameState.Paused:
                break;
            case GameState.Playing:
                
                // Check if all players are dead
                break;
            case GameState.GameOver:
                // Trigger game over screen
                // Restart to the last checkpoint?
                break;
            default:
                break;
        }
    }
}