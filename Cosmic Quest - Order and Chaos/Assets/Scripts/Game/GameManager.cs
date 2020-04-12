using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Loading,
        Paused,
        Playing,
        BossFight,
        GameOver,
        SelectingLevel,
        Victory
    }
    
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Only one GameManager should be in the scene!");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    // Tracking and delegate events for state changes
    private GameState _currentState = GameState.Menu;
    public UnityEvent onStateChange = new UnityEvent();
    public GameState CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState == value)
                return;
            
            _currentState = value;
            onStateChange.Invoke();
        }
    }
    
    [Tooltip("Y coordinate where players instantly die if they fall below")]
    public float playerDeathZone = -30f;
    
    // Keeps track if the game is in a testing state (i.e. not started from the menu)
    [HideInInspector] public bool isTestInstance = false;

    private void Update()
    {
        // Polls for game events and handles any state changes
        switch (CurrentState)
        {
            case GameState.Menu:
            case GameState.Paused:
            case GameState.SelectingLevel:
                break;
            case GameState.Loading:
                break;
            case GameState.Playing:
            case GameState.BossFight:

                // Check if all players are dead
                if (PlayerManager.Instance.NumPlayersAlive() == 0)
                {
                    CurrentState = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                // Trigger game over screen
                // Restart to the last checkpoint?
                TransitionMenu.Instance.ShowGameOverMenu();
                break;
            case GameState.Victory:
                break;
            default:
                break;
        }
    }

    public void SetLoadingState()
    {
        CurrentState = GameState.Loading;
        onStateChange.Invoke();
    }
    
    public void SetPlayState()
    {
        CurrentState = GameState.Playing;
        onStateChange.Invoke();
    }

    public void SetMenuState()
    {
        CurrentState = GameState.Menu;
        onStateChange.Invoke();
    }

    public void SetPausedState()
    {
        CurrentState = GameState.Paused;
        onStateChange.Invoke();
    }

    public void SetSelectingLevelState()
    {
        CurrentState = GameState.SelectingLevel;
        onStateChange.Invoke();
    }

    public void SetGameOverState()
    {
        CurrentState = GameState.GameOver;
        onStateChange.Invoke();
    }

    public void SetBossState()
    {
        CurrentState = GameState.BossFight;
        onStateChange.Invoke();
    }

    public void SetVictoryState()
    {
        CurrentState = GameState.Victory;
        onStateChange.Invoke();
        StartCoroutine(CompleteLevel());
    }

    public IEnumerator CompleteLevel()
    {
        LevelManager.Instance.ClearChaosVoid();
        yield return new WaitForSeconds(2.5f);
        TransitionMenu.Instance.ShowVictoryMenu();
    }
}