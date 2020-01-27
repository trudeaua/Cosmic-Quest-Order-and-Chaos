using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

[Serializable]
public class PlayerColours
{
    public Color red = Color.red;
    public Color green = Color.green;
    public Color purple = Color.magenta;
    public Color yellow = Color.yellow;

    public Color GetColour(CharacterColour colour)
    {
        switch (colour)
        {
            case CharacterColour.Red: return red;
            case CharacterColour.Green: return green;
            case CharacterColour.Purple: return purple;
            case CharacterColour.Yellow: return yellow;
            default: return Color.gray;
        }
    }
}

public class Player
{
    // Which input device is the player connected to
    public PlayerInput playerInput;

    // Which character has the player chosen
    public CharacterOption characterChoice;

    // Which colour is the player
    public CharacterColour characterColour;
    
    // Which ui control is assigned to the player
    public GameObject playerUIControl;

    // Which class has the player selected
    public GameObject playerObject;

    public int deviceId;

    public Player(PlayerInput _playerInput, CharacterColour _characterColour, CharacterOption _characterChoice, int _deviceId)
    {
        playerInput = _playerInput;
        characterColour = _characterColour;
        characterChoice = _characterChoice;
        deviceId = _deviceId;
    }

}

[Serializable]
public class ClassOption
{
    public string name;
    public GameObject prefab;
}

[Serializable]
public class CharacterOption
{
    public string name;
    public Texture skin;
}

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one player manager should be in the scene!");
    }
    #endregion

    public static readonly List<GameObject> Players = new List<GameObject>();

    public static PlayerColours colours = new PlayerColours();
    
    // TODO change this to a pool of textures, or assigned to a player at class selection
    public Texture testPlayerTexture;

    [Tooltip("Classes that the players can choose")]
    [SerializeField] private ClassOption[] classOptions;

    [Tooltip("Characters that the players can choose")]
    [SerializeField] private CharacterOption[] characterOptions;

    public static List<CharacterColour> availableColours = new List<CharacterColour> { CharacterColour.Purple, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };
    public static List<CharacterColour> playerColours = new List<CharacterColour>();

    // Maintains the players that have joined the game
    public static readonly Player[] _Players = { null, null ,null, null };
    public static readonly CharacterColour[] _PlayerColours = { CharacterColour.Purple, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };

    // GameObject containing all selectables and submenus for the main menu
    private GameObject MenuCanvas;

    private void Start()
    {
        // Assign players their correct colour outline
        // TODO Perhaps materials should be dynamically assigned elsewhere?
        foreach (GameObject player in Players)
        {
            // Dynamically assign each player their respective outline texture
            Color playerColour = colours.GetColour(player.GetComponent<EntityStatsController>().characterColour);
            Material playerMaterial = new Material(Shader.Find("Custom/Outline"));
            playerMaterial.SetFloat("_Outline", 0.0005f);
            playerMaterial.SetColor("_OutlineColor", playerColour);
            playerMaterial.SetTexture("_MainTex", testPlayerTexture);
            player.GetComponentInChildren<Renderer>().sharedMaterial = playerMaterial;
        }
    }

    /// <summary>
    /// Register a player
    /// </summary>
    /// <param name="player">Player gameobject</param>
    public static void RegisterPlayer(GameObject player)
    {
        Players.Add(player);
        // assign the player a colour as soon as they're registered
        CharacterColour characterColour = player.GetComponent<EntityStatsController>().characterColour;
        if (characterColour == CharacterColour.None) {
            // if they don't have a colour, give them one
            characterColour = availableColours[0];
            availableColours.Remove(characterColour);
        }
        playerColours.Add(characterColour);
        player.GetComponent<EntityStatsController>().characterColour = characterColour;
    }

    /// <summary>
    /// Deregister a player
    /// </summary>
    /// <param name="player">Player gameobject</param>
    public static void DeregisterPlayer(GameObject player)
    {
        Players.Remove(player);
    }

    /// <summary>
    /// Instantiate one of the players
    /// </summary>
    /// <param name="whichPlayer">The number of the player to instantiate (0-3)</param>
    /// <returns></returns>
    public GameObject InstantiatePlayer(int whichPlayer)
    {
        if (_Players[whichPlayer] != null)
        {
            // PlayerInput is disabled then reenabled here because when a new instance of PlayerInput is added to the scene,
            // the PlayerInputManager treats it as a new player being connected to the scene. So disabling the PlayerInput 
            // in the prefab and then instantiating does not cause it to be treated as a new player
            _Players[whichPlayer].playerObject.GetComponent<PlayerInput>().enabled = false;
            _Players[whichPlayer].playerObject.GetComponent<PlayerMotorController>().doRegister = false;
            GameObject playerInstance = Instantiate(_Players[whichPlayer].playerObject);
            _Players[whichPlayer].playerObject.GetComponent<PlayerInput>().enabled = true;
            _Players[whichPlayer].playerObject.GetComponent<PlayerMotorController>().doRegister = true;

            // Assign the player their respective outline texture
            playerInstance.GetComponent<EntityStatsController>().characterColour = _Players[whichPlayer].characterColour;
            Material playerMaterial = new Material(Shader.Find("Custom/Outline"));
            playerMaterial.SetFloat("_Outline", 0.0005f);
            playerMaterial.SetColor("_OutlineColor", colours.GetColour(_Players[whichPlayer].characterColour));
            playerMaterial.SetTexture("_MainTex", _Players[whichPlayer].characterChoice.skin);
            playerInstance.GetComponentInChildren<Renderer>().sharedMaterial = playerMaterial;

            return playerInstance;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Assign a Player UI Control game object to a player
    /// </summary>
    /// <param name="playerUIControl">Player UI Control game object</param>
    /// <returns>The player number that the control was assigned to, -1 if not assigned.</returns>
    public int AssignUIControlToPlayer(GameObject playerUIControl)
    {
        for(int i = 0; i < _Players.Length; i++)
        {
            if (!_Players[i].playerUIControl)
            {
                _Players[i].playerUIControl = playerUIControl;
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Assign a texture to a player
    /// </summary>
    /// <param name="player">Number of the player to apply the texture to (0-3)</param>
    /// <param name="characterChoice">A character choice representing which texture to select</param>
    public void AssignCharacterChoice(int player, int characterChoice)
    {
        if (_Players[player] != null)
        {
            _Players[player].characterChoice = characterOptions[characterChoice];
        }
    }

    /// <summary>
    /// Assign a player prefab to a player
    /// </summary>
    /// <param name="player">Number of the player to apply the plaeyer prefab to (0-3)</param>
    /// <param name="classChoice">A class choice representing which player prefab to select</param>
    public void AssignPrefab(int player, int classChoice)
    {
        if (_Players[player] != null)
        {
            _Players[player].playerObject = classOptions[classChoice].prefab;
        }
    }

    /// <summary>
    /// Get the names of all available characters
    /// </summary>
    /// <returns>An array of character names</returns>
    public string[] GetCharacterNames()
    {
        string[] names = new string[characterOptions.Length];
        for(int i = 0; i < names.Length; i++)
        {
            names[i] = characterOptions[i].name;
        }
        return names;
    }

    /// <summary>
    /// Get the names of all available classes
    /// </summary>
    /// <returns>An array of class names</returns>
    public string[] GetClassNames()
    {
        string[] names = new string[classOptions.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = classOptions[i].name;
        }
        return names;
    }

    /// <summary>
    /// Switch a player's current action map.
    /// Useful for switching between "Player" and "UI" action maps.
    /// </summary>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    /// <param name="actionMap">Name of the action map</param>
    public static void SwitchActionMap(int playerNumber, string actionMap)
    {
        if (playerNumber >= 0 && playerNumber < _Players.Length)
        {
            PlayerInput playerInput = _Players[playerNumber].playerInput;
            playerInput.SwitchCurrentActionMap(actionMap);
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Joined");
        // If not existing player, add new
        bool isNewPlayer = true;
        InputDevice inputDevice = playerInput.user.pairedDevices.First();
        if (inputDevice == null)
        {
            Debug.LogError("No input device detected for Player " + playerInput.user.id);
            return;
        }
        foreach (Player p in _Players)
        {
            if (p != null)
            {
                if (p.deviceId == inputDevice.deviceId)
                {
                    isNewPlayer = false;
                    break;
                }
            }
        }
        if (isNewPlayer)
        {
            Player newPlayer = new Player(playerInput, CharacterColour.None, characterOptions[0], inputDevice.deviceId);
            // Assign the new player a colour
            int playerNumber = 0;
            for (int i = 0; i < _Players.Length; i++)
            {
                if (_Players[i] == null)
                {
                    playerNumber = i;
                    break;
                }
            }
            newPlayer.characterColour = _PlayerColours[playerNumber];
            _Players[playerNumber] = newPlayer;
            if (inputDevice is Gamepad)
            {
                Gamepad gamepad = inputDevice as Gamepad;
                // If the device is a DualShock4, color the light bar as the player colour :)
                if (gamepad is DualShockGamepad)
                {
                    DualShockGamepad ds = gamepad as DualShockGamepad;
                    ds.SetLightBarColor(colours.GetColour(newPlayer.characterColour));
                }
            }
            // TODO do the rest of this method better
            if (MenuCanvas == null)
            {
                MenuCanvas = FindObjectOfType<MainMenuController>().gameObject;
            }
            // i.e. If we're on the menu
            if (MenuCanvas != null)
            {
                MenuCanvas.BroadcastMessage("PlayerJoined", playerNumber);
            }
            // If not in menu we must be in a level (right???)
            else
            {
                SwitchActionMap(playerNumber, "Player");
            }
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Left");
        playerInput.user.UnpairDevicesAndRemoveUser();
    }

    /// <summary>
    /// Get the number of a player by their input device id
    /// </summary>
    /// <param name="deviceId">ID of the player's primary input device</param>
    /// <returns></returns>
    public int GetPlayerNumber(int deviceId)
    {
        for(int i = 0; i < _Players.Length; i++)
        {
            if (_Players[i] != null && _Players[i].deviceId == deviceId)
            {
                return _Players[i].deviceId;
            }
        }
        return -1;
    }

    /// <summary>
    /// Remove a player
    /// </summary>
    /// <param name="playerNumber">Number of the player to remove (0-3)</param>
    public void RemovePlayer(int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < _Players.Length)
        {
            if (_Players[playerNumber] != null)
            {
                _Players[playerNumber] = null;
            }
        }
    }
}
