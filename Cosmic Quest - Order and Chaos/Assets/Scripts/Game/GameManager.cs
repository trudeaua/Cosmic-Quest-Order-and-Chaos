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
        BossFight,
        GameOver
    }
    
    #region Singleton
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Only one GameManager should be in the scene!");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public GameState currentState { get; private set; } = GameState.Menu;

    private void Update()
    {
        // Polls for game events and handles any state changes
        switch (currentState)
        {
            case GameState.Menu:
            case GameState.Paused:
                break;
            case GameState.Playing:
            case GameState.BossFight:

                // Check if all players are dead
                if (PlayerManager.Instance.NumPlayersAlive() == 0)
                {
                    currentState = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                // Trigger game over screen
                // Restart to the last checkpoint?
                
                //LevelManager.Instance.StartLevel1();
                //currentState = GameState.Playing;
                break;
            default:
                break;
        }
    }

    public void SetGameState(GameState state)
    {
        currentState = state;
    }
}