using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PreviewMenuController : MenuController
{
    #region Singleton
    public new static MenuController Instance;

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
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    /// <param name="playerObject">Game object of the player that paused the game</param>
    public void PreviewLevel(GameObject playerObject)
    {
        if (isPreviewing)
        {
            return;
        }
        isPreviewing = true;
        playerEventSystem = playerObject.GetComponent<MultiplayerEventSystem>();
        playerInput = playerObject.GetComponent<PlayerInput>();
        uIInputModule = playerObject.GetComponent<InputSystemUIInputModule>();
        PushMenu(activeMenu);
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void UnpreviewLevel()
    {
        if (!isPreviewing)
        {
            return;
        }
        isPreviewing = false;
        playerEventSystem.SetSelectedGameObject(null);
        activeMenu.SetActive(false);
        activeMenu = GetRootMenu();
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
