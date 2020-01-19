using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

public enum CharacterChoice
{
    NONE,
    MAGE,
    MELEE,
    HEALER,
    RANGER
}
public class Player
{
    public PlayerInput playerInput;
    public CharacterChoice characterChoice;
    public CharacterColour characterColour;
    public GameObject playerUIControl;
    public GameObject playerObject;

    public Player(PlayerInput _playerInput, CharacterChoice _characterChoice, CharacterColour _characterColour)
    {
        playerInput = _playerInput;
        characterChoice = _characterChoice;
        characterColour = _characterColour;
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
    }
    #endregion

    public static readonly List<GameObject> Players = new List<GameObject>();

    public static PlayerColours colours = new PlayerColours();
    
    // TODO change this to a pool of textures, or assigned to a player at class selection
    public Texture testPlayerTexture;

    public static List<CharacterColour> availableColours = new List<CharacterColour> { CharacterColour.Purple, CharacterColour.Green, CharacterColour.Red, CharacterColour.Yellow };
    public static List<CharacterColour> playerColours = new List<CharacterColour>();

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
                return i + 1;
            }
        }
        return -1;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Joined");
        // If not existing player, add new
        bool isNewPlayer = true;
        foreach(Player p in _Players)
        {
            if (p != null)
            {
                if (p.playerInput.user.id == playerInput.user.id)
                {
                    isNewPlayer = false;
                    break;
                }
            }
        }
        if (isNewPlayer)
        {
            Player newPlayer = new Player(playerInput, CharacterChoice.NONE, CharacterColour.None);
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
            Debug.Log(newPlayer.characterColour);
            // Add ui control for the player
            //MenuController.AddMultiplayerUIControl(index + 1);
        }
        // If existing player
        // Don't add ui control for player, already exists
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player " + playerInput.user.id + " Left");
    }

}
