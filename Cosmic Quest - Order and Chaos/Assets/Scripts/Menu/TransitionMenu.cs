using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class TransitionMenu : MenuController
{
    #region Singleton
    public new static TransitionMenu Instance;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one transition menu controller should be in the scene!");
    }
    #endregion

    [SerializeField] protected GameObject victoryMenu;
    [SerializeField] protected GameObject gameOverMenu;

    // for caching which player paused the game
    private MultiplayerEventSystem playerEventSystem;
    private PlayerInput playerInput;
    private InputSystemUIInputModule uIInputModule;

    //// Start is called before the first frame update
    protected override void Start()
    {
        menuStack = new Stack<GameObject>();
        selectedButtonsStack = new Stack<GameObject>();
        activeMenu.SetActive(false);
        FindCameraAndMusic();
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    /// <param name="menu">The menu to navigate to</param>
    public override void PushMenu(GameObject menu)
    {
        GameObject button = GetSelectedButton(playerEventSystem);
        if (button)
        {
            selectedButtonsStack.Push(button);
        }

        base.PushMenu(menu);
        SetPlayerRoot(playerEventSystem);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    public override void PopMenu()
    {
        base.PopMenu();
        SetPlayerRoot(playerEventSystem);
        GameObject button = PopButton();
        playerEventSystem.SetSelectedGameObject(button);
    }

    /// <summary>
    /// Indicates whether the player is at the root menu or not
    /// </summary>
    /// <returns></returns>
    public bool IsAtRoot()
    {
        return menuStack.Count == 1;
    }

    public void ShowVictoryMenu()
    {
        GameObject playerObject = PlayerManager.Instance.Players[0];
        Time.timeScale = 0;
        playerEventSystem = playerObject.GetComponent<MultiplayerEventSystem>();
        playerInput = playerObject.GetComponent<PlayerInput>();
        uIInputModule = playerObject.GetComponent<InputSystemUIInputModule>();
        PushMenu(victoryMenu);
        SwitchCurrentActionMap("UI");
        GameManager.Instance.SetPausedState();
    }

    public void ShowGameOverMenu()
    {
        GameObject playerObject = PlayerManager.Instance.Players[0];
        Time.timeScale = 0;
        playerEventSystem = playerObject.GetComponent<MultiplayerEventSystem>();
        playerInput = playerObject.GetComponent<PlayerInput>();
        uIInputModule = playerObject.GetComponent<InputSystemUIInputModule>();
        PushMenu(gameOverMenu);
        SwitchCurrentActionMap("UI");
        GameManager.Instance.SetPausedState();
    }

    public void NextLevel()
    {
        LevelManager.Instance.StartNextLevel();
    }

    public void RestartLevel()
    {
        LevelManager.Instance.RestartCurrentLevel();
    }

    /// <summary>
    /// Description: Switch the player's action map
    /// Rationale: Should be able to switch the action map between Player and UI states
    /// </summary>
    /// <param name="name">Name of the action map</param>
    private void SwitchCurrentActionMap(string name)
    {
        playerInput.SwitchCurrentActionMap(name);
        /*
         * IMPORTANT this seemingly redundant code is to prevent a bug in the new input system
         * cause by "states not being stored correctly" according to the developers. If the
         * disable-enable sequence isn't there the input system will throw an error every frame
         * after switching action maps.
         * 
         * See https://github.com/Unity-Technologies/InputSystem/issues/941 for more info
         */
        uIInputModule.enabled = false;
        uIInputModule.enabled = true;
    }
}
