using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    #region Singleton
    public static PauseMenuController _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one pause menu controller should be in the scene!");
    }
    #endregion

    [SerializeField] private GameObject activeMenu;

    private Selectable[] selectables;
    public bool IsPaused { get; internal set; }

    // for caching which player paused the game
    private MultiplayerEventSystem playerEventSystem;
    private PlayerInput playerInput;
    private InputSystemUIInputModule uIInputModule;

    private Stack<GameObject> menuStack = new Stack<GameObject>();


    // Start is called before the first frame update
    private void Start()
    {
        selectables = activeMenu.GetComponentsInChildren<Selectable>();
        activeMenu.SetActive(false);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    /// <param name="menu">The menu to navigate to</param>
    public void PushMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
        menuStack.Push(menu);

        SetPlayerRoot(playerEventSystem);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    public void PopMenu()
    {
        if (menuStack.Count > 0)
        {
            activeMenu.SetActive(false);
            menuStack.Pop();
            activeMenu = menuStack.Peek();
            activeMenu.SetActive(true);

            SetPlayerRoot(playerEventSystem);
        }
    }

    public bool isAtRoot()
    {
        return menuStack.Count == 1;
    }

    /// <summary>
    /// Gets the first selectable `GameObject` found in the specified menu
    /// </summary>
    /// <param name="menu">Game object to search for buttons in</param>
    public static GameObject GetDefaultButton(GameObject menu)
    {
        if (menu == null)
        {
            return null;
        }
        Selectable btn = menu.GetComponentInChildren<Selectable>();
        if (btn)
        {
            return btn.gameObject;
        }
        else
        {
            Debug.LogError("No selectable objects found in Pause Menu Game Object");
            return null;
        }
    }


    /// <summary>
    /// Sets the root of a multiplayer event system to a submenu that can only be controlled by that player
    /// </summary>
    /// <param name="eventSystem">A multiplayer event system that corresponds to `playerNumber`</param>
    private void SetPlayerRoot(MultiplayerEventSystem eventSystem)
    {
        eventSystem.playerRoot = activeMenu;
        GameObject defaultButton = GetDefaultButton(activeMenu);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    public void PauseGame(GameObject playerObject)
    {
        if (IsPaused)
        {
            Debug.LogWarning("Game is already paused");
            return;
        }
        Time.timeScale = 0;
        // Set pause menu active
        activeMenu.SetActive(true);
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
        // Set pause menu inactive
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

    private void SwitchCurrentActionMap(string name)
    {
        playerInput.SwitchCurrentActionMap(name);
        uIInputModule.enabled = false;
        uIInputModule.enabled = true;
    }
}
