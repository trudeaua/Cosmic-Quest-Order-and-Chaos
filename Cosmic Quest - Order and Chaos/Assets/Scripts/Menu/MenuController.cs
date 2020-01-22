using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Maintains the currently active menu in the menu canvas
    [SerializeField] private GameObject ActiveMenu;
    [SerializeField] private GameObject MenuAfterLobby;
    private static GameObject activeMenu;

    // Maintains the number of players selected on the multiplayer menu
    private int NumberOfPlayers = 0;

    // Maintains all multiplayer event systems
    protected static List<MultiplayerEventSystem> multiplayerEventSystems = new List<MultiplayerEventSystem>();

    // Maintains the menus that the player has navigated through
    protected Stack<GameObject> menuStack = new Stack<GameObject>();

    [SerializeField] protected GameObject[] playerJoinAreas;
    private static List<bool> ReadyPlayers;

    protected virtual void Awake()
    {
        activeMenu = ActiveMenu;

        menuStack = new Stack<GameObject>();
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);

        ReadyPlayers = new List<bool>();
        for (int i = 0; i < ReadyPlayers.Count; i++)
        {
            ReadyPlayers[i] = false;
        }
        SetJoinAreasActive(false);
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

        GameObject selectedButton = FindDefaultButton(activeMenu);
        SetSelectedButton(selectedButton);
        SetPlayerRoots();
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

            GameObject selectedButton = FindDefaultButton(activeMenu);
            SetSelectedButton(selectedButton);
            SetPlayerRoots();
        }
    }

    /// <summary>
    /// Set the button that should be highlighted on the next menu
    /// </summary>
    public static void SetSelectedButton(GameObject btn)
    {
        foreach(MultiplayerEventSystem eventSystem in multiplayerEventSystems)
        {
            eventSystem.SetSelectedGameObject(btn);
        }
    }

    /// <summary>
    /// Find a default button to select
    /// </summary>
    /// <param name="menu">Game object to search for buttons in</param>
    public static GameObject FindDefaultButton(GameObject menu)
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
            return null;
        }
    }

    /// <summary>
    /// Assign a Player UI Control game object to a default button
    /// </summary>
    /// <param name="playerNumber">Number of the player</param>
    /// <param name="uiControl">UI Control prefab</param>
    public static void AssignMultiplayerUIControl(GameObject uiControl, int playerNumber)
    {
        MultiplayerEventSystem eventSystem = uiControl.GetComponentInChildren<MultiplayerEventSystem>();
        GameObject defaultButton = FindDefaultButton(activeMenu);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
        SetPlayerRoot(eventSystem, playerNumber);
        multiplayerEventSystems.Add(eventSystem);
    }

    /// <summary>
    /// If there are sub menu's that can only be controlled by a certain player, this method will set the root of that player's ui control to those menus
    /// Sets roots of all player event systems
    /// </summary>
    private static void SetPlayerRoots()
    {
        int playerNumber = 1;
        foreach (MultiplayerEventSystem eventSystem in multiplayerEventSystems)
        {
            SetPlayerRoot(eventSystem, playerNumber);
            playerNumber++;
        }
    }

    /// <summary>
    /// If there is a sub menu that can only be controlled by a certain player, this method will set the root of that player's ui control to that menu
    /// Sets root of one player event system
    /// </summary>
    private static void SetPlayerRoot(MultiplayerEventSystem eventSystem, int playerNumber)
    {
        //GameObject playerRoot = GameObject.FindGameObjectWithTag("Player" + playerNumber + "Choice");
        // Tags are used to distinguish which submenus can be controlled by which player
        GameObject playerRoot = null;
        RectTransform[] rectTransforms = activeMenu.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform rectTransform in rectTransforms)
        {
            if (rectTransform.gameObject.CompareTag("Player" + playerNumber + "Choice"))
            {
                playerRoot = rectTransform.gameObject;
                break;
            }
        }
        eventSystem.playerRoot = playerRoot;
        GameObject defaultButton = FindDefaultButton(playerRoot);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    private void SetJoinAreaActive(bool active, int playerNumber)
    {
        if (playerNumber > 0 && playerNumber < playerJoinAreas.Length)
        {
            // Set the join messages to inactive
            Transform[] children = playerJoinAreas[playerNumber].GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.CompareTag("JoinMessage"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetJoinAreasActive(bool active, int start = 0)
    {
        // Find the player join areas
        for (int i = start; i < playerJoinAreas.Length; i++)
        {
            SetJoinAreaActive(active, i);
        }
    }

    /// <summary>
    /// Set the number of players in the game
    /// </summary>
    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        NumberOfPlayers = numberOfPlayers;
    }

    public void EnableJoining()
    {
        PlayerInputManager.instance.EnableJoining();
    }

    public void DisableJoining()
    {
        PlayerInputManager.instance.DisableJoining();
    }

    public void PlayerJoined(int index)
    {
        if (index < playerJoinAreas.Length)
        {
            // Find the inactive join message and set it to active
            Transform[] children = playerJoinAreas[index].GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.CompareTag("JoinMessage"))
                {
                    child.gameObject.SetActive(true);
                    SetNumberOfPlayers(NumberOfPlayers + 1);
                }
            }
            ReadyPlayers.Add(false);
        }
        if (NumberOfPlayers == 1)
        {
            DisableJoining();
        }
    }

    public void PlayerReady(int playerNumber)
    {
        Debug.Log("Player " + playerNumber + " ready");
        if (playerNumber >= 0 && playerNumber < ReadyPlayers.Count)
        {
            ReadyPlayers[playerNumber] = true;
            RectTransform[] rectTransforms = playerJoinAreas[playerNumber].GetComponentsInChildren<RectTransform>(true);
            for(int i = 0; i < rectTransforms.Length; i++)
            {
                if (rectTransforms[i].CompareTag("PlayerReady"))
                {
                    rectTransforms[i].gameObject.SetActive(true);
                }
                else if (rectTransforms[i].CompareTag("PlayerNotReady"))
                {
                    rectTransforms[i].gameObject.SetActive(false);
                }
            }
        }
        bool allPlayersReady = NumberOfPlayers > 1;
        foreach(bool readyPlayer in ReadyPlayers)
        {
            if (!readyPlayer)
            {
                allPlayersReady = false;
                break;
            }
        }
        if (allPlayersReady)
        {
            PushMenu(MenuAfterLobby);
            PreviewPlayers(MenuAfterLobby);
        }
    }

    public void PlayerUnready(int playerNumber)
    {
        Debug.Log("Player " + playerNumber + 1 + " unready");
        if (playerNumber >= 0 && playerNumber < ReadyPlayers.Count)
        {
            ReadyPlayers[playerNumber] = false;
            RectTransform[] rectTransforms = playerJoinAreas[playerNumber].GetComponentsInChildren<RectTransform>(true);
            for (int i = 0; i < rectTransforms.Length; i++)
            {
                if (rectTransforms[i].CompareTag("PlayerReady"))
                {
                    rectTransforms[i].gameObject.SetActive(true);
                }
                else if (rectTransforms[i].CompareTag("PlayerNotReady"))
                {
                    rectTransforms[i].gameObject.SetActive(false);
                }
            }
        }
        bool allPlayersNotReady = true;
        foreach (bool readyPlayer in ReadyPlayers)
        {
            if (readyPlayer)
            {
                allPlayersNotReady = false;
                break;
            }
        }
        if (allPlayersNotReady)
        {
            for (int i = NumberOfPlayers - 1; i >= 1; i--)
            {
                PlayerManager.RemovePlayer(i);
                multiplayerEventSystems.RemoveAt(i);
                SetNumberOfPlayers(NumberOfPlayers - 1);
            }
            PlayerUIControl[] uiControls = FindObjectsOfType<PlayerUIControl>();
            for (int i = 1; i < uiControls.Length + 1; i++)
            {
                if (uiControls[i - 1].gameObject.name == "Player " + (i + 1) + " UI Control")
                {
                    Destroy(uiControls[i - 1].gameObject);
                }
            }
            DisableJoining();
            // Hide all but player 1s
            SetJoinAreasActive(false, 1);
            PopMenu();
        }
    }

    /// <summary>
    ///  Instantiate the players around a certain position
    /// </summary>
    /// <param name="positionObj">Game object in which to instantiate the players</param>
    public void PreviewPlayers(GameObject positionObj)
    {
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            GameObject playerInstance = PlayerManager.InstantiatePlayer(i);
            playerInstance.transform.parent = positionObj.transform;

            // Transform the player instance so it looks nice on screen
            playerInstance.transform.localPosition = new Vector3(((i - 1) * NumberOfPlayers / 2 + (NumberOfPlayers % 2 == 0 ? 0.5f : 0)) * 200, 0, 1);
            playerInstance.transform.Rotate(new Vector3(0, 180, 0));
            playerInstance.transform.localScale = new Vector3(45, 45, 45);

            // Turn off components so the player is simply displayed and can't be controlled
            playerInstance.GetComponent<Collider>().enabled = false;
            playerInstance.GetComponent<PlayerInput>().enabled = false;
            playerInstance.GetComponent<PlayerInteractionController>().enabled = false;
            playerInstance.GetComponent<EntityStatsController>().SetSpawn(false);
            playerInstance.GetComponent<EntityCombatController>().enabled = false;
            playerInstance.GetComponentInChildren<StatBar>().gameObject.SetActive(false);
            playerInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    /// <summary>
    /// Destroy the player preview objects
    /// </summary>
    /// <param name="positionObj">Parent game object of the player objects</param>
    public void DestroyPlayerPreview(GameObject positionObj)
    {
        EntityStatsController[] children = positionObj.GetComponentsInChildren<EntityStatsController>();
        foreach(EntityStatsController child in children)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Log each player's character and class selections
    /// </summary>
    public void LogPlayerInfo()
    {
        foreach(Player p in PlayerManager._Players)
        {
            if (p != null)
            {
                Debug.Log(p.characterColour + "\n" + p.characterChoice + "\n" + p.playerObject.name);
            }
        }
    }

    /// <summary>
    /// Register all selected players with the player manager
    /// </summary>
    /// <param name="playerContainer">Game object whose children are the selected players</param>
    public void RegisterPlayers(GameObject playerContainer)
    {
        EntityStatsController[] players = playerContainer.GetComponentsInChildren<EntityStatsController>();
        foreach (EntityStatsController entityStats in players)
        {
            PlayerManager.RegisterPlayer(entityStats.gameObject);
        }
    }

    /// <summary>
    /// Deregister all selected players with the player manager
    /// </summary>
    /// <param name="playerContainer">Game object whose children are the selected players</param>
    public void DeregisterPlayers(GameObject playerContainer)
    {
        EntityStatsController[] players = playerContainer.GetComponentsInChildren<EntityStatsController>();
        foreach (EntityStatsController entityStats in players)
        {
            PlayerManager.DeregisterPlayer(entityStats.gameObject);
        }
    }

    /// <summary>
    /// Start the tutorial level
    /// </summary>
    public void StartTutorial()
    {
        // Load Tutorial scene
        StartCoroutine(LoadYourAsyncScene("Tutorial"));
    }

    /// <summary>
    /// Start level 1
    /// </summary>
    public void StartLevel() 
    {
        // Load Level 1
        StartCoroutine(LoadYourAsyncScene("ChaosVoid1"));
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public static IEnumerator BackToMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuStaging");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Load a scene asynchronously
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


}