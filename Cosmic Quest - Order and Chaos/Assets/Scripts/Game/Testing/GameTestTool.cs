using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TestPlayerClass
{
    Mage,
    Melee,
    Healer,
    Ranger
}

[Serializable]
public struct TestPlayer
{
    public TestPlayerClass combatClass;
}

public class GameTestTool : MonoBehaviour
{
    public List<TestPlayer> testPlayers;

    public GameObject gameManagerPrefab;
    
    /// <summary>
    /// Creates a mocked up GameManager instance (if one doesn't already exist) and registers
    /// some mock players for ease of testing levels during development.
    /// </summary>
    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            // If the GameManager exists, then players and inputs are already setup (i.e. ran from main menu).
            // Simply disable this test tool
            gameObject.SetActive(false);
            return;
        }
        
        // Mock setup of GameManager and players
        GameObject mockGameManager = Instantiate(gameManagerPrefab);

        // Cannot assume setup of singletons have finished, so keep direct references
        GameManager gameManager = mockGameManager.GetComponent<GameManager>();
        PlayerManager playerManager = mockGameManager.GetComponent<PlayerManager>();
        
        gameManager.SetPlayState();
        gameManager.isTestInstance = true;

        // Manually add players to PlayerManager
        foreach (TestPlayer player in testPlayers)
        {
            int playerNumber = playerManager.AddTestPlayer();

            // Assign class choice
            string[] classNames = playerManager.GetClassNames();
            for (int i = 0; i < classNames.Length; i++)
            {
                // This is a hacky comparison, but it doesn't really matter
                if (classNames[i].Equals(player.combatClass.ToString()))
                {
                    playerManager.AssignPrefab(playerNumber, i);
                    break;
                }
            }
        }
    }
}
