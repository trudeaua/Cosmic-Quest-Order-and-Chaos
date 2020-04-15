﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenuController : MenuController
{
    #region Singleton
    public new static MainMenuController Instance;

    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one main menu controller should be in the scene!");
    }
    #endregion
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject lobbyConfirmButton;

    // Maintains the number of players selected on the multiplayer menu
    private int NumberOfPlayers = 0;

    // Maintains all multiplayer event systems
    protected List<MultiplayerEventSystem> multiplayerEventSystems = new List<MultiplayerEventSystem>();

    [SerializeField] private GameObject[] playerJoinAreas;

    private static List<bool> ReadyPlayers;

    protected override void Start()
    {
        PlayerManager.Instance.InitializePlayers();
        lobbyConfirmButton.SetActive(false);
        base.Start();
        startScreen.SetActive(true);
        activeMenu.SetActive(false);
        ReadyPlayers = new List<bool>();
        SetJoinAreasActive(false);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    /// <param name="menu">The menu to navigate to</param>
    public override void PushMenu(GameObject menu)
    {
        if (multiplayerEventSystems.Count > 0)
        {
            GameObject button = GetSelectedButton(multiplayerEventSystems[0]);
            if (button)
            {
                selectedButtonsStack.Push(button);
            }
        }
        base.PushMenu(menu);
        SetPlayerRoots();
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    public override void PopMenu()
    {
        if (ReadyPlayers[0] == false)
        {
            base.PopMenu();
            SetPlayerRoots();
            if (multiplayerEventSystems.Count > 0)
            {
                GameObject button = PopButton();
                multiplayerEventSystems[0].SetSelectedGameObject(button);
            }
        }
    }

    /// <summary>
    /// Assign a Player UI Control game object to a default button
    /// </summary>
    /// <param name="playerNumber">Number of the player</param>
    /// <param name="uiControl">UI Control prefab</param>
    public void AssignMultiplayerUIControl(GameObject uiControl, int playerNumber)
    {
        MultiplayerEventSystem eventSystem = uiControl.GetComponentInChildren<MultiplayerEventSystem>();
        PlayerJoined(playerNumber);
        SetPlayerRoot(eventSystem, playerNumber);
        multiplayerEventSystems.Add(eventSystem);
    }

    /// <summary>
    /// Sets roots of all multiplayer event systems in the scene
    /// </summary>
    protected void SetPlayerRoots()
    {
        int playerNumber = 0;
        foreach (MultiplayerEventSystem eventSystem in multiplayerEventSystems)
        {
            SetPlayerRoot(eventSystem, playerNumber);
            playerNumber++;
        }
    }

    /// <summary>
    /// Sets the root of a multiplayer event system to a submenu that can only be controlled by that player
    /// </summary>
    /// <param name="eventSystem">A multiplayer event system that corresponds to `playerNumber`</param>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    protected void SetPlayerRoot(MultiplayerEventSystem eventSystem, int playerNumber)
    {
        GameObject playerRoot = null;
        RectTransform[] rectTransforms = activeMenu.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rectTransform in rectTransforms)
        {
            // Tags are used to distinguish which submenus can be controlled by which player
            if (rectTransform.gameObject.CompareTag("Player" + (playerNumber + 1) + "Choice"))
            {
                playerRoot = rectTransform.gameObject;
                break;
            }
        }
        if (playerRoot == null)
        {
            //playerRoot = activeMenu;
            return;
        }
        eventSystem.playerRoot = playerRoot;
        GameObject defaultButton = GetDefaultButton(playerRoot);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    /// <summary>
    /// Sets the active property of a certain player's "Join Area"
    /// </summary>
    /// <param name="setActive">Indicates whether the Join Area should be set to active or not</param>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    private void SetJoinAreaActive(bool setActive, int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < playerJoinAreas.Length)
        {
            Transform[] children = playerJoinAreas[playerNumber].GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.CompareTag("Joined"))
                {
                    child.gameObject.SetActive(setActive);
                }
                if (child.CompareTag("NotJoined"))
                {
                    child.gameObject.SetActive(!setActive);
                }
            }
        }
    }

    /// <summary>
    /// Sets the active property of all players' "Join Area"
    /// </summary>
    /// <param name="setActive">Indicates whether the Join Areas should be set to active or not</param>
    /// <param name="start">Number of the player to start at. Sometimes it is desireable to set all but player 1's Join Area to inactive</param>
    private void SetJoinAreasActive(bool setActive, int start = 0)
    {
        // Find the player join areas
        for (int i = start; i < playerJoinAreas.Length; i++)
        {
            SetJoinAreaActive(setActive, i);
        }
    }

    /// <summary>
    /// Set the number of players in the game
    /// </summary>
    /// <param name="numberOfPlayers">Number of players in the game</param>
    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        NumberOfPlayers = numberOfPlayers;
    }

    /// <summary>
    /// Allow new players to join the game
    /// </summary>
    public void EnableJoining()
    {
        PlayerInputManager.instance.EnableJoining();
    }

    /// <summary>
    /// Don't allow new players to join the game
    /// </summary>
    public void DisableJoining()
    {
        PlayerInputManager.instance.DisableJoining();
    }

    /// <summary>
    /// When a player joins the game in the multiplayer lobby, set their join area to active
    /// </summary>
    /// <param name="playerNumber">Number of the player</param>
    public void PlayerJoined(int playerNumber)
    {
        if (playerNumber < playerJoinAreas.Length)
        {
            if (NumberOfPlayers >= PlayerManager.Instance.MinNumberPlayers)
            {
                lobbyConfirmButton.SetActive(false);
                multiplayerEventSystems[0].SetSelectedGameObject(GetDefaultButton(playerJoinAreas[0]));
            }
            SetJoinAreaActive(true, playerNumber);
            ReadyPlayers.Add(false);
            SetNumberOfPlayers(NumberOfPlayers + 1);
        }
        // Disable joining so that only player 1 can control the menu.
        // Joining is enabled again in the multiplayer lobby and then disabled when players leave it. See unity inspector.
        if (NumberOfPlayers == 1)
        {
            // once a player joins hide the start screen and show the entry screen
            startScreen.SetActive(false);
            PushMenu(activeMenu);
            DisableJoining();
        }
    }

    /// <summary>
    /// Set a player's "ready" and "character selection" states
    /// </summary>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    /// <param name="isPlayerReady">Indicates whether the player has finished selecting their character</param>
    private void SetReady(int playerNumber, bool isPlayerReady)
    {
        RectTransform[] rectTransforms = playerJoinAreas[playerNumber].GetComponentsInChildren<RectTransform>(true);
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            if (rectTransforms[i].gameObject.CompareTag("PlayerReady"))
            {
                rectTransforms[i].gameObject.SetActive(isPlayerReady);
            }
            else if (rectTransforms[i].gameObject.CompareTag("PlayerNotReady"))
            {
                rectTransforms[i].gameObject.SetActive(!isPlayerReady);
            }
        }
        SetPlayerRoot(multiplayerEventSystems[playerNumber], playerNumber);
    }

    /// <summary>
    /// Set a player's "character selection" state to inactive and "ready" state to active
    /// </summary>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    public void PlayerReady(int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < ReadyPlayers.Count)
        {
            ReadyPlayers[playerNumber] = true;
            SetReady(playerNumber, ReadyPlayers[playerNumber]);
            if (ReadyPlayers.Count(r => r == true) == NumberOfPlayers && NumberOfPlayers >= PlayerManager.Instance.MinNumberPlayers)
            {
                lobbyConfirmButton.SetActive(true);
                multiplayerEventSystems[0].SetSelectedGameObject(lobbyConfirmButton);
            }
        }
    }

    /// <summary>
    /// Set a player's "character selection" state to active and "ready" state to inactive
    /// </summary>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    public void PlayerUnready(int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < ReadyPlayers.Count)
        {
            ReadyPlayers[playerNumber] = false;
            SetReady(playerNumber, ReadyPlayers[playerNumber]);
            lobbyConfirmButton.SetActive(false);
        }
    }
}