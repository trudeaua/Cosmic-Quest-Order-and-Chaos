using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

[System.Serializable]
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

public enum ClassChoice
{
    MAGE,
    MELEE,
    HEALER,
    RANGER,
    NONE
}
public enum CharacterChoice
{
    ALIEN_A,
    ALIEN_B,
    ALIEN_C,
    ROBOT,
    NONE
}
public class Player
{
    // Which input device is the player connected to
    public PlayerInput playerInput;

    // Which chaarcter has the player chosen
    public CharacterChoice characterChoice;

    // Which colour is the player
    public CharacterColour characterColour;
    
    // Which ui control is assigned to the player
    public GameObject playerUIControl;

    // Which class has the player selected
    public GameObject playerObject;

    public int deviceId;

    public Player(PlayerInput _playerInput, CharacterColour _characterColour, CharacterChoice _characterChoice, int _deviceId)
    {
        playerInput = _playerInput;
        characterColour = _characterColour;
        //characterChoice = PlayerManager.LookupTexture(_characterChoice);
        characterChoice = _characterChoice;
        deviceId = _deviceId;
    }

}

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    private static PlayerManager _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one player manager should be in the scene!");

        TexturePool = texturePool;
        PlayerPrefabPool = playerPrefabPool;
    }
    #endregion

    public static readonly List<GameObject> Players = new List<GameObject>();

    public static PlayerColours colours = new PlayerColours();
    
    // TODO change this to a pool of textures, or assigned to a player at class selection
    public Texture testPlayerTexture;

    // Pool of textures
    [SerializeField] private Texture[] texturePool;
    public static Texture[] TexturePool;

    // Pool of player prefabs
    [SerializeField] private GameObject[] playerPrefabPool;
    public static GameObject[] PlayerPrefabPool;

    public static List<CharacterColour> availableColours = new List<CharacterColour> { CharacterColour.Purple, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };
    public static List<CharacterColour> playerColours = new List<CharacterColour>();

    // Maintains the players that have joined the game
    public static readonly Player[] _Players = { null, null ,null, null };
    public static readonly CharacterColour[] _PlayerColours = { CharacterColour.Purple, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };

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
    public static GameObject InstantiatePlayer(int whichPlayer)
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
            playerMaterial.SetTexture("_MainTex", LookupTexture(_Players[whichPlayer].characterChoice));
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
    public static int AssignUIControlToPlayer(GameObject playerUIControl)
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
    /// Find a texture in the texture pool
    /// </summary>
    /// <param name="characterChoice">A character choice representing which texture to select</param>
    /// <returns>A texture from the texture pool</returns>
    public static Texture LookupTexture(CharacterChoice characterChoice)
    {
        if (TexturePool.Length >= (int)characterChoice)
        {
            return TexturePool[(int)characterChoice];
        }
        else
        {
            return TexturePool[TexturePool.Length - 1];
        }
    }

    /// <summary>
    /// Assign a texture to a player
    /// </summary>
    /// <param name="player">Number of the player to apply the texture to (0-3)</param>
    /// <param name="characterChoice">A character choice representing which texture to select</param>
    public static void AssignCharacterChoice(int player, CharacterChoice characterChoice)
    {
        if (_Players[player] != null)
        {
            _Players[player].characterChoice = characterChoice;
        }
    }

    /// <summary>
    /// Find a player prefab in the player prefab pool
    /// </summary>
    /// <param name="classChoice">A class choice representing which player prefab to select</param>
    /// <returns>A player prefab from the player prefab pool</returns>
    public static GameObject LookupPrefab(ClassChoice classChoice)
    {
        if (PlayerPrefabPool.Length >= (int)classChoice)
        {
            return PlayerPrefabPool[(int)classChoice];
        }
        else
        {
            return PlayerPrefabPool[PlayerPrefabPool.Length - 1];
        }
    }

    /// <summary>
    /// Assign a player prefab to a player
    /// </summary>
    /// <param name="player">Number of the player to apply the plaeyer prefab to (0-3)</param>
    /// <param name="classChoice">A class choice representing which player prefab to select</param>
    public static void AssignPrefab(int player, ClassChoice classChoice)
    {
        if (_Players[player] != null)
        {
            _Players[player].playerObject = LookupPrefab(classChoice);
        }
    }

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
                // if (p.playerInput.user.id == playerInput.user.id)
                if (p.deviceId == inputDevice.deviceId)
                {
                    isNewPlayer = false;
                    break;
                }
            }
        }
        if (isNewPlayer)
        {
            Player newPlayer = new Player(playerInput, CharacterColour.None, CharacterChoice.NONE, inputDevice.deviceId);
            // Assign the new player a colour
            int index = 0;
            for (int i = 0; i < _Players.Length; i++)
            {
                if (_Players[i] == null)
                {
                    index = i;
                    break;
                }
            }
            newPlayer.characterColour = _PlayerColours[index];
            _Players[index] = newPlayer;
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
            GameObject MenuCanvas = GameObject.Find("MenuCanvas");
            if (MenuCanvas != null)
            {
                MenuCanvas.BroadcastMessage("PlayerJoined", index);
            }
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Left");
        Debug.Log(InputSystem.disconnectedDevices.Count);
        playerInput.user.UnpairDevicesAndRemoveUser();
    }

    public static void RemovePlayer(int playerNumber)
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
