using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PreviewMenuController : MenuController
{
    #region Singleton
    public new static PreviewMenuController Instance;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one preview menu controller should be in the scene!");
    }
    #endregion

    private bool isPreviewing;
    private MultiplayerEventSystem playerEventSystem;
    private PlayerInput playerInput;
    private InputSystemUIInputModule uIInputModule;

    protected override void Start()
    {
        menuStack = new Stack<GameObject>();
        selectedButtonsStack = new Stack<GameObject>();
        activeMenu.SetActive(false);
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
    /// Preview the level
    /// </summary>
    /// <param name="playerObject">Game object of the player's UI control</param>
    public void PreviewLevel()
    {
        if (isPreviewing)
        {
            return;
        }
        isPreviewing = true;
        PushMenu(activeMenu);
    }

    /// <summary>
    /// Unpreview the level
    /// </summary>
    public void UnpreviewLevel()
    {
        if (!isPreviewing)
        {
            return;
        }
        isPreviewing = false;
        playerEventSystem.SetSelectedGameObject(null);
        playerEventSystem.firstSelectedGameObject = null;
        playerEventSystem.playerRoot = null;
        activeMenu.SetActive(false);
        activeMenu = GetRootMenu();
    }

    /// <summary>
    /// Assign a Player UI Control game object to a default button
    /// </summary>
    /// <param name="uiControl">UI Control prefab</param>
    public void AssignMultiplayerUIControl(GameObject uiControl)
    {
        playerEventSystem = uiControl.GetComponent<MultiplayerEventSystem>();
        playerInput = uiControl.GetComponent<PlayerInput>();
        uIInputModule = uiControl.GetComponent<InputSystemUIInputModule>();
    }
}
