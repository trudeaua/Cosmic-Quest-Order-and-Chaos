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
    /// Pause the game
    /// </summary>
    /// <param name="playerObject">Game object of the player that paused the game</param>
    public void PauseGame(GameObject playerObject)
    {
        if (IsPaused)
        {
            Debug.LogWarning("Game is already paused");
            return;
        }
        Time.timeScale = 0;
        // Set pause menu active
        IsPaused = true;
        playerEventSystem = playerObject.GetComponent<MultiplayerEventSystem>();
        if (playerEventSystem == null)
        {
            Debug.LogError("Player object does not have an Multiplayer Event System component attached to it.");
            return;
        }
        playerInput = playerObject.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("Player object does not have an Player Input component attached to it.");
            return;
        }
        uIInputModule = playerObject.GetComponent<InputSystemUIInputModule>();
        if (uIInputModule == null)
        {
            Debug.LogError("Player object does not have an Input System UI Input Module component attached to it.");
            return;
        }
        PushMenu(activeMenu);
        SwitchCurrentActionMap("UI");
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        if (!IsPaused)
        {
            Debug.LogWarning("Game is already resumed!");
            return;
        }
        Time.timeScale = 1;
        IsPaused = false;
        playerEventSystem.SetSelectedGameObject(null);
        activeMenu.SetActive(false);
        activeMenu = GetRootMenu();
        SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Find the root of the menu stack
    /// </summary>
    /// <returns>The root menu GameObject at the bottom of the menu stack</returns>
    private GameObject GetRootMenu()
    {
        GameObject rootMenu = null;
        for (int i = 0; i <= menuStack.Count; i++)
        {
            rootMenu = menuStack.Pop();
        }
        return rootMenu;
    }

    /// <summary>
    /// Switch the player who paused the game's current controller action map
    /// </summary>
    /// <param name="name"></param>
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
