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
    [SerializeField] private GameObject ActiveMenu;
    [SerializeField] private GameObject PlayerUIControlPrefab;

    private static GameObject activeMenu;
    private static GameObject playerUIControlPrefab;
    private int NumberOfPlayers;

    protected static List<MultiplayerEventSystem> multiplayerEventSystems = new List<MultiplayerEventSystem>();
    protected Stack<GameObject> menuStack = new Stack<GameObject>();
    
    protected void Awake()
    {
        activeMenu = ActiveMenu;
        playerUIControlPrefab = PlayerUIControlPrefab;

        menuStack = new Stack<GameObject>();
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);
    }

    protected void Start()
    {
        PlayerManager.EnableJoining();
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

    public void ToggleColor(GameObject btnObj)
    {
        Button btn = btnObj.GetComponent<Button>();
        if (btn.image.color == btn.colors.normalColor)
        {
            btn.image.color = btn.colors.highlightedColor;
        }
        else
        {
            btn.image.color = btn.colors.normalColor;
        }
    }


    public void SetP1Character(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignTexture(0, (CharacterChoice)i);
            }
        }
    }

    public void SetP2Character(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignTexture(1, (CharacterChoice)i);
            }
        }
    }

    public void SetP3Character(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignTexture(2, (CharacterChoice)i);
            }
        }
    }

    public void SetP4Character(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignTexture(3, (CharacterChoice)i);
            }
        }
    }

    public void SetP1Class(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignPrefab(0, (ClassChoice)i);
            }
        }
    }

    public void SetP2Class(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignPrefab(1, (ClassChoice)i);
            }
        }
    }

    public void SetP3Class(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignPrefab(2, (ClassChoice)i);
            }
        }
    }

    public void SetP4Class(GameObject toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                PlayerManager.AssignPrefab(3, (ClassChoice)i);
            }
        }
    }

    /// <summary>
    ///  Instantiate the players around a certain position
    /// </summary>
    /// <param name="positionObj">Game object in which to instantiate the players</param>
    public void PreviewPlayers(GameObject positionObj)
    {
        int numPlayers = PlayerManager._Players.Where(player => player != null).Count();
        Debug.Log(numPlayers);
        Vector3 position = positionObj.transform.position;
        PlayerManager.DisableJoining();
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject playerInstance = Instantiate(PlayerManager._Players[i].playerObject);
            playerInstance.transform.parent = positionObj.transform;
            // Dynamically assign each player their respective outline texture
            playerInstance.GetComponent<EntityStatsController>().characterColour = PlayerManager._Players[i].characterColour;
            Material playerMaterial = new Material(Shader.Find("Custom/Outline"));
            playerMaterial.SetFloat("_Outline", 0.0005f);
            playerMaterial.SetColor("_OutlineColor", PlayerManager.colours.GetColour(PlayerManager._Players[i].characterColour));
            playerMaterial.SetTexture("_MainTex", PlayerManager._Players[i].characterChoice);
            playerInstance.GetComponentInChildren<Renderer>().sharedMaterial = playerMaterial;

            playerInstance.transform.localPosition = new Vector3(((i - 1) * numPlayers / 2 + (numPlayers % 2 == 0 ? 0.5f : 0)) * 200, 0, 1);
            playerInstance.transform.Rotate(new Vector3(0, 180, 0));
            playerInstance.transform.localScale = new Vector3(45, 45, 45);

            playerInstance.GetComponent<Collider>().enabled = false;
            playerInstance.GetComponent<PlayerInput>().enabled = false;
            playerInstance.GetComponent<PlayerInteractionController>().enabled = false;
            playerInstance.GetComponent<EntityCombatController>().enabled = false;
            playerInstance.GetComponentInChildren<StatBar>().gameObject.SetActive(false);
            playerInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
        PlayerManager.EnableJoining();
    }

    /// <summary>
    /// Destroy the player preview objects
    /// </summary>
    /// <param name="positionObj">Parent game object of the player objects</param>
    public void DestroyPlayerPreview(GameObject positionObj)
    {
        EntityStatsController[] children = positionObj.GetComponentsInChildren<EntityStatsController>();
        Debug.Log(children.Length);
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