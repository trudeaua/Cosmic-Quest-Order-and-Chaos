using System.Collections.Generic;
using UnityEngine;

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

    public static List<CharacterColour> availableColours = new List<CharacterColour> { CharacterColour.Red, CharacterColour.Green, CharacterColour.Purple, CharacterColour.Yellow };
    public static List<CharacterColour> playerColours = new List<CharacterColour>();

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

    public static void RegisterPlayer(GameObject player)
    {
        Players.Add(player);

        // assign the player a colour as soon as they're registered
        CharacterColour characterColour = availableColours[Random.Range(0, availableColours.Count)];
        availableColours.Remove(characterColour);
        playerColours.Add(characterColour);
        player.GetComponent<EntityStatsController>().characterColour = characterColour;
    }

    public static void DeregisterPlayer(GameObject player)
    {
        Players.Remove(player);
    }
}
