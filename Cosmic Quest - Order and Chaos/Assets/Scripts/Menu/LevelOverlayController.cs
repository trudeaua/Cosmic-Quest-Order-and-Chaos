using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class LevelOverlayController : MenuController
{
    #region Singleton
    public new static LevelOverlayController Instance;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one level overlay controller should be in the scene!");
    }
    #endregion

    [SerializeField] private TMPro.TextMeshProUGUI levelTitle;
    private bool isPreviewing;
    private MultiplayerEventSystem playerEventSystem;

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
    public void ShowOverlay()
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
    public void HideOverlay()
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
    }

    /// <summary>
    /// Set the title text of the level overlay
    /// </summary>
    /// <param name="text">Text to change the title to</param>
    public IEnumerator SetTitle(string text, float delay)
    {
        levelTitle.text = null;
        yield return new WaitForSeconds(delay);
        levelTitle.text = text;
    }
}
