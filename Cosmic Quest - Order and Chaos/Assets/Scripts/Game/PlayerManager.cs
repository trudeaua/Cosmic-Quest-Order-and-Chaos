using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Utilities;

[Serializable]
public class PlayerColours
{
    public Color red = Color.red;
    public Color green = Color.green;
    public Color blue = Color.blue;
    public Color yellow = Color.yellow;

    public Color GetColour(CharacterColour colour)
    {
        switch (colour)
        {
            case CharacterColour.Red: return red;
            case CharacterColour.Green: return green;
            case CharacterColour.Blue: return blue;
            case CharacterColour.Yellow: return yellow;
            default: return Color.gray;
        }
    }

    public string GetColorHex(CharacterColour colour)
    {
        Color c = GetColour(colour);
        return string.Format("#{0:X2}{1:X2}{2:X2}", (int)(c.r * 255), (int)(c.g * 255), (int)(c.b * 255));
    }
}

[Serializable]
public class Player
{
    // Which input device is the player connected to
    public PlayerInput playerInput;

    // Which character has the player chosen
    public CharacterOption characterChoice;

    // Which colour is the player
    public CharacterColour characterColour;

    // name of the class
    public string classChoice;
    
    // Which ui control is assigned to the player
    public GameObject playerUIControl;

    // Which class has the player selected
    public GameObject playerObject;

    public int deviceId;

    public int primaryAttackActionId;
    public int secondaryAttackActionId;

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
        {
            Debug.LogWarning("Only one player manager should be in the scene!");
            Destroy(this);
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public readonly List<GameObject> Players = new List<GameObject>();
    
    public static PlayerColours colours = new PlayerColours();

    [Tooltip("Classes that the players can choose")]
    [SerializeField] private ClassOption[] classOptions;

    [Tooltip("Characters that the players can choose")]
    [SerializeField] private CharacterOption[] characterOptions;

    [Tooltip("Prefab object of the player UI control")]
    [SerializeField] private GameObject playerUIControlPrefab;

    // Maintains the players that have joined the game
    private readonly Player[] _playerSlots = { null, null ,null, null };
    public readonly CharacterColour[] PlayerColours = { CharacterColour.Blue, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };

    public int NumPlayers
    {
        get { return _playerSlots.Count(p => p != null); }
    }

    public List<GameObject> AlivePlayers
    {
        get { return Players.FindAll(p => !p.GetComponent<PlayerStatsController>().isDead); }
    }

    public CharacterColour[] CurrentPlayerColours 
    {
        get { return Instance.PlayerColours.Take(Instance.NumPlayers).ToArray(); }
    }


    // GameObject containing all selectables and submenus for the main menu
    private GameObject MenuCanvas;

    public int NumPlayersAlive()
    {
        return Players.Count(player => !player.GetComponent<PlayerStatsController>().isDead);
    }

    /// <summary>
    /// Initializes the player instance tracking for the current scene
    /// </summary>
    public void InitializePlayers()
    {
        Players.Clear();
    }
    
    /// <summary>
    /// Registers a player character instance for the current scene
    /// </summary>
    /// <param name="player">Player gameobject</param>
    public void RegisterPlayer(GameObject player)
    {
        if (Players.Count < NumPlayers)
            Players.Add(player);
        else
            Debug.LogError("Attempted to register more player instances than players!");
    }

    /// <summary>
    /// Deregister a player character instance for the current scene. Is this necessary?
    /// </summary>
    /// <param name="player">Player gameobject</param>
    public void DeregisterPlayer(GameObject player)
    {
        Players.Remove(player);
    }

    public GameObject InstantiatePlayer(int playerNumber)
    {
        if (_playerSlots[playerNumber] == null)
            return null;

        GameObject playerInstance;
        if (GameManager.Instance.isTestInstance)
        {
            // Simply instantiate the player and let it choose a device randomly
            playerInstance = Instantiate(_playerSlots[playerNumber].playerObject);
            PlayerInput playerInput = playerInstance.GetComponent<PlayerInput>();
            InputDevice device = playerInput.devices.FirstOrDefault();
            if (device != null)
            {
                _playerSlots[playerNumber].deviceId = device.deviceId;
            }
        }
        else
        {
            // Instantiate the player model and pair to their input device
            InputDevice device = InputDevice.all.First(d => d.deviceId == _playerSlots[playerNumber].deviceId);
            PlayerInput playerInstanceInput = PlayerInput.Instantiate(_playerSlots[playerNumber].playerObject, playerNumber, "Gamepad", -1, device);
            playerInstance = playerInstanceInput.gameObject;
        }

        // Assign the player their respective outline colour and texture
        SetPlayerLooksAndColour(playerInstance, playerNumber);

        return playerInstance;
    }

    public GameObject InstantiatePlayerUIControl(int playerNumber)
    {
        if (_playerSlots[playerNumber] == null)
            return null;

        GameObject playerUIControlInstance;
        if (GameManager.Instance.isTestInstance)
        {
            // Simply instantiate the player and let it choose a device randomly
            playerUIControlInstance = Instantiate(playerUIControlPrefab);
        }
        else
        {
            // Instantiate the player model and pair to their input device
            InputDevice device = InputDevice.all.First(d => d.deviceId == _playerSlots[playerNumber].deviceId);
            PlayerInput playerInstanceInput = PlayerInput.Instantiate(playerUIControlPrefab, playerNumber, "Gamepad", -1, device);
            playerUIControlInstance = playerInstanceInput.gameObject;
        }

        return playerUIControlInstance;
    }

    /// <summary>
    /// Instantiates a preview model of one of the players
    /// </summary>
    /// <param name="playerNumber">The number of the player to instantiate (0-3)</param>
    /// <returns></returns>
    public GameObject InstantiatePlayerPreview(int playerNumber)
    {
        if (_playerSlots[playerNumber] == null)
            return null;
        
        // PlayerInput is disabled then reenabled here because when a new instance of PlayerInput is added to the scene,
        // the PlayerInputManager treats it as a new player being connected to the scene. So disabling the PlayerInput 
        // in the prefab and then instantiating does not cause it to be treated as a new player
        _playerSlots[playerNumber].playerObject.GetComponent<PlayerInput>().enabled = false;
        GameObject playerInstance = Instantiate(_playerSlots[playerNumber].playerObject);
        //GameObject playerInstance = PlayerInput.Instantiate(_Players[whichPlayer].playerObject, whichPlayer, "player", -1, _Players[whichPlayer].deviceId);
        _playerSlots[playerNumber].playerObject.GetComponent<PlayerInput>().enabled = true;

        // Assign the player their respective outline colour and texture
        SetPlayerLooksAndColour(playerInstance, playerNumber);

        return playerInstance;
    }

    public void SetPlayerLooksAndColour(GameObject playerInstance, int playerNumber)
    {
        playerInstance.GetComponent<EntityStatsController>().characterColour = _playerSlots[playerNumber].characterColour;
        Material playerMaterial = new Material(Shader.Find("Custom/Outline"));
        playerMaterial.SetFloat("_Outline", 0.0005f);
        playerMaterial.SetColor("_OutlineColor", colours.GetColour(_playerSlots[playerNumber].characterColour));
        playerMaterial.SetTexture("_MainTex", _playerSlots[playerNumber].characterChoice.skin);
        playerInstance.GetComponentInChildren<Renderer>().sharedMaterial = playerMaterial;
    }
    
    /// <summary>
    /// Assign a Player UI Control game object to a player
    /// </summary>
    /// <param name="playerUIControl">Player UI Control game object</param>
    /// <returns>The player number that the control was assigned to, -1 if not assigned.</returns>
    public int AssignUIControlToPlayer(GameObject playerUIControl)
    {
        for (int i = 0; i < _playerSlots.Length; i++)
        {
            if (_playerSlots[i].playerUIControl == null)
            {
                _playerSlots[i].playerUIControl = playerUIControl;
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
        if (_playerSlots[player] != null)
        {
            _playerSlots[player].characterChoice = characterOptions[characterChoice];
        }
    }

    /// <summary>
    /// Assign a player prefab to a player
    /// </summary>
    /// <param name="player">Number of the player to apply the player prefab to (0-3)</param>
    /// <param name="classChoice">A class choice representing which player prefab to select</param>
    public void AssignPrefab(int player, int classChoice)
    {
        if (_playerSlots[player] != null)
        {
            _playerSlots[player].playerObject = classOptions[classChoice].prefab;
            _playerSlots[player].classChoice = classOptions[classChoice].name;
        }
    }

    /// <summary>
    /// Get the names of all available characters
    /// </summary>
    /// <returns>An array of character names</returns>
    public string[] GetCharacterNames()
    {
        return characterOptions.Select(option => option.name).ToArray();
    }

    /// <summary>
    /// Get the names of all available classes
    /// </summary>
    /// <returns>An array of class names</returns>
    public string[] GetClassNames()
    {
        return classOptions.Select(option => option.name).ToArray();
    }

    /// <summary>
    /// Switch a player's current action map.
    /// Useful for switching between "Player" and "UI" action maps.
    /// </summary>
    /// <param name="playerNumber">Number of the player (0-3)</param>
    /// <param name="actionMap">Name of the action map</param>
    public void SwitchActionMap(int playerNumber, string actionMap)
    {
        if (playerNumber >= 0 && playerNumber < _playerSlots.Length)
        {
            PlayerInput playerInput = _playerSlots[playerNumber].playerInput;
            playerInput.SwitchCurrentActionMap(actionMap);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
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
        foreach (Player p in _playerSlots)
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
            for (int i = 0; i < _playerSlots.Length; i++)
            {
                if (_playerSlots[i] == null)
                {
                    playerNumber = i;
                    break;
                }
            }
            newPlayer.characterColour = PlayerColours[playerNumber];
            _playerSlots[playerNumber] = newPlayer;
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
            
            if (MenuCanvas == null)
            {
                MainMenuController mainMenu = FindObjectOfType<MainMenuController>();
                if (mainMenu != null)
                {
                    MenuCanvas = mainMenu.gameObject;
                }
            }
            // i.e. If we're on the menu
            if (MenuCanvas != null)
            {
               // MenuCanvas.BroadcastMessage("PlayerJoined", playerNumber);
            }
            // If not in menu we must be in a level (right???)
            else
            {
                SwitchActionMap(playerNumber, "Player");
            }
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Left");
        playerInput.user.UnpairDevicesAndRemoveUser();
    }

    public IEnumerator RumbleGamepad(PlayerInput playerInput, float lowFrequency, float highFrequency, float duration)
    {
        InputDevice inputDevice = playerInput.devices.First();
        if (inputDevice is Gamepad)
        {
            Gamepad gamepad = inputDevice as Gamepad;
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            yield return new WaitForSeconds(duration);
            gamepad.SetMotorSpeeds(0, 0);
        }
    }

    /// <summary>
    /// Retrieves an action map from a Player's Input
    /// </summary>
    /// <param name="playerInput">Player's Input component</param>
    /// <param name="actionMapName">Name of the action map</param>
    /// <returns>The action map</returns>
    public InputActionMap GetActionMap(PlayerInput playerInput, string actionMapName)
    {
        ReadOnlyArray<InputActionMap> actionMaps = playerInput.actions.actionMaps;
        foreach (InputActionMap actionMap in actionMaps)
        {
            if (actionMap.name == actionMapName)
            {
                return actionMap;
            }
        }
        return null;
    }

    /// <summary>
    /// Get the number of a player by their input device id
    /// </summary>
    /// <param name="deviceId">ID of the player's primary input device</param>
    /// <returns></returns>
    public int GetPlayerNumber(int deviceId)
    {
        for (int i = 0; i < _playerSlots.Length; i++)
        {
            if (_playerSlots[i] != null && _playerSlots[i].deviceId == deviceId)
            {
                return i;
            }
        }
        return -1;
    }

    // Find a players GameObject in the scene
    public GameObject FindPlayer(int playerNumber)
    {
        PlayerStatsController[] players = FindObjectsOfType<PlayerStatsController>();
        for (int i = 0; i < players.Length; i++)
        {
            PlayerInput playerInput = players[i].GetComponent<PlayerInput>();
            int deviceId = playerInput.user.pairedDevices.First().deviceId;
            int playerNum = GetPlayerNumber(deviceId);
            if (playerNum == playerNumber)
            {
                return players[i].gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Remove a player
    /// </summary>
    /// <param name="playerNumber">Number of the player to remove (0-3)</param>
    public void RemovePlayer(int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < _playerSlots.Length)
        {
            _playerSlots[playerNumber] = null;
        }
    }

    public int AddTestPlayer()
    {
        Player newPlayer = new Player(null, CharacterColour.None, characterOptions[0], -1);
        // Assign the new player a colour
        int playerNumber = 0;
        for (int i = 0; i < _playerSlots.Length; i++)
        {
            if (_playerSlots[i] == null)
            {
                playerNumber = i;
                break;
            }
        }
        newPlayer.characterColour = PlayerColours[playerNumber];
        _playerSlots[playerNumber] = newPlayer;
        return playerNumber;
    }

    /// <summary>
    /// Get the class name of a player
    /// </summary>
    /// <param name="playerNumber">Number of the player</param>
    /// <returns>The player's class name, null if not found</returns>
    public string GetPlayerClassName(int playerNumber)
    {
        if (playerNumber >= 0 && playerNumber < NumPlayers)
        {
            return _playerSlots[playerNumber].classChoice;
        }
        return null;
    }
}
