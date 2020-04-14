using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseMenuController : MenuController
{
    #region Singleton
    public new static PauseMenuController Instance;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one pause menu controller should be in the scene!");
    }
    #endregion

    public bool IsPaused { get; internal set; }

    // for caching which player paused the game
    private MultiplayerEventSystem playerEventSystem;
    private PlayerInput playerInput;
    private InputSystemUIInputModule uIInputModule;

    // Start is called before the first frame update
    protected override void Start()
    {
        menuStack = new Stack<GameObject>();
        selectedButtonsStack = new Stack<GameObject>();
        activeMenu.SetActive(false);
        FindCameraAndMusic();
        SetUpSpeakerModesDropdown();
        SetUpQualitySettingsDropdown();
        SetUpAntiAliasingDropdown();
        SetUpDisplayModeDropdown();
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

    /// <summary>
    /// Description: Pause the game
    /// Rationale: Players should be able to pause the game and navigate menu options
    /// </summary>
    /// <param name="playerObject">Game object of the player that paused the game</param>
    public void PauseGame(GameObject playerObject)
    {
        if (IsPaused)
        {
            return;
        }
        Time.timeScale = 0;
        IsPaused = true;
        playerEventSystem = playerObject.GetComponent<MultiplayerEventSystem>();
        playerInput = playerObject.GetComponent<PlayerInput>();
        uIInputModule = playerObject.GetComponent<InputSystemUIInputModule>();
        PushMenu(activeMenu);
        SwitchCurrentActionMap("UI");
        GameManager.Instance.SetPausedState();
    }

    /// <summary>
    /// Description: Resume the game
    /// Rationale: Players should be able to resume the game after pausing
    /// </summary>
    public void ResumeGame()
    {
        if (!IsPaused)
        {
            return;
        }
        Time.timeScale = 1;
        IsPaused = false;
        playerEventSystem.SetSelectedGameObject(null);
        activeMenu.SetActive(false);
        activeMenu = GetRootMenu();
        SwitchCurrentActionMap("Player");
        GameManager.Instance.SetPlayState();
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
