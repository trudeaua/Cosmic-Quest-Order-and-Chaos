using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject ActiveMenu;
    [SerializeField] private GameObject PlayerUIControlPrefab;

    private static GameObject activeMenu;
    private static GameObject playerUIControlPrefab;
    private int NumberOfPlayers;

    protected static List<MultiplayerEventSystem> multiplayerEventSystems = new List<MultiplayerEventSystem>();
    protected Stack<GameObject> menuStack = new Stack<GameObject>();
    
    protected virtual void Awake()
    {
        activeMenu = ActiveMenu;
        playerUIControlPrefab = PlayerUIControlPrefab;

        menuStack = new Stack<GameObject>();
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);
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
        Button btn = menu.GetComponentInChildren<Button>();
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
            // Tags are used to distinguish which submenus can be controlled by which player
            GameObject playerRoot = GameObject.FindGameObjectWithTag("Player" + playerNumber + "Choice");
            if (playerRoot)
            {
                eventSystem.playerRoot = playerRoot;
                GameObject defaultButton = FindDefaultButton(playerRoot);
                eventSystem.firstSelectedGameObject = defaultButton;
                eventSystem.SetSelectedGameObject(defaultButton);
            }
            playerNumber++;
        }
    }

    /// <summary>
    /// If there is a sub menu that can only be controlled by a certain player, this method will set the root of that player's ui control to that menu
    /// Sets root of one player event system
    /// </summary>
    private static void SetPlayerRoot(MultiplayerEventSystem eventSystem, int playerNumber)
    {
        // Tags are used to distinguish which submenus can be controlled by which player
        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player" + playerNumber + "Choice");
        if (playerRoot)
        {
            eventSystem.playerRoot = playerRoot;
            GameObject defaultButton = FindDefaultButton(playerRoot);
            eventSystem.firstSelectedGameObject = defaultButton;
            eventSystem.SetSelectedGameObject(defaultButton);
        }
    }

    /// <summary>
    /// Set the number of players in the game
    /// </summary>
    public void SetNumberOfPlayers(int numberOfPlayers)
    {
        NumberOfPlayers = numberOfPlayers;
    }

    /// <summary>
    /// Filter the player options to reflect the number of players playing the game
    /// </summary>
    public void FilterPlayerOptions()
    {
        for (int i = 4; i > NumberOfPlayers; i--)
        {
            GameObject[] playerRoots = GameObject.FindGameObjectsWithTag("Player" + i + "Choice");
            foreach (GameObject playerRoot in playerRoots)
            {
                playerRoot.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set Player 1's class prefab
    /// </summary>
    public void SetPlayer1Class(GameObject playerPrefab)
    {
        PlayerManager._Players[0].playerObject = playerPrefab;
    }

    /// <summary>
    /// Set Player 2's class prefab
    /// </summary>
    public void SetPlayer2Class(GameObject playerPrefab)
    {
        PlayerManager._Players[1].playerObject = playerPrefab;
    }

    /// <summary>
    /// Set Player 3's class prefab
    /// </summary>
    public void SetPlayer3Class(GameObject playerPrefab)
    {
        PlayerManager._Players[2].playerObject = playerPrefab;
    }

    /// <summary>
    /// Set Player 4's class prefab
    /// </summary>
    public void SetPlayer4Class(GameObject playerPrefab)
    {
        PlayerManager._Players[3].playerObject = playerPrefab;
    }

    /// <summary>
    /// Set Player 1's character texture
    /// </summary>
    public void SetPlayer1Character(Texture characterChoice)
    {
        PlayerManager._Players[0].characterChoice = characterChoice;
    }

    /// <summary>
    /// Set Player 2's character texture
    /// </summary>
    public void SetPlayer2Character(Texture characterChoice)
    {
        PlayerManager._Players[1].characterChoice = characterChoice;
    }

    /// <summary>
    /// Set Player 3's character texture
    /// </summary>
    public void SetPlayer3Character(Texture characterChoice)
    {
        PlayerManager._Players[2].characterChoice = characterChoice;
    }

    /// <summary>
    /// Set Player 4's character texture
    /// </summary>
    public void SetPlayer4Character(Texture characterChoice)
    {
        PlayerManager._Players[3].characterChoice = characterChoice;
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
                Debug.Log(p.characterColour);
                Debug.Log(p.characterChoice + "\n");
                Debug.Log(p.playerObject.name);
            }
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