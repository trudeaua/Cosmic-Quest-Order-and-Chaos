using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerCombatSystemTests
{
    private const int MaxPlayers = 4;
    private PlayerInputMock[] _inputMocks = new PlayerInputMock[MaxPlayers];

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for scene to load
        float timer = 0;
        while (!sceneLoading.isDone)
        {
            if (timer > 15f)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Assert.IsTrue(sceneLoading.isDone, "Scene load timed out! The scene could not be loaded.");

        // Setup controllers for all possible players
        for (int i = 0; i < MaxPlayers; i++)
        {
            if (_inputMocks[i] is null)
                _inputMocks[i] = new PlayerInputMock(i);
        }
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // destroy all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Object.Destroy(player);
        }
        
        // destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Object.Destroy(enemy);
        }
        
        yield return null;
    }
    
    /*
     * Various multi-player combat scenarios
     */

    [UnityTest]
    public IEnumerator CombatScenario_TwoPlayersFightingOneEnemy()
    {
        // Set up entities
        GameObject meleePlayer1 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(-2.5f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats1 = meleePlayer1.GetComponent<PlayerStatsController>();
        
        GameObject meleePlayer2 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(2.5f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats2 = meleePlayer2.GetComponent<PlayerStatsController>();
        
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        meleePlayer1.SetActive(true);
        meleePlayer2.SetActive(true);
        enemy.SetActive(true);

        // Setup inputs
        PlayerInput input1 = meleePlayer1.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[0].SetInputToMockGamepad(input1);
        
        PlayerInput input2 = meleePlayer2.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[1].SetInputToMockGamepad(input2);

        // Setup enemy
        float initialHealth = enemyStats.health.CurrentValue;
        enemyStats.characterColour = CharacterColour.None;

        yield return new WaitForSeconds(0.5f);
        
        // Players face the enemy
        _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, Vector2.right);
        yield return new WaitForSeconds(0.2f);
        _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, Vector2.left);

        // Players attack with their primary attacks
        float timer = 0f;
        while (!enemyStats.isDead)
        {
            _inputMocks[0].Press(_inputMocks[0].Gamepad.rightShoulder);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[0].Release(_inputMocks[0].Gamepad.rightShoulder);
            
            yield return new WaitForSeconds(0.3f);
            
            _inputMocks[1].Press(_inputMocks[1].Gamepad.rightShoulder);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[1].Release(_inputMocks[1].Gamepad.rightShoulder);
            
            yield return new WaitForSeconds(1f);
            
            // Ensure that the test does not run longer than 15 seconds
            if (timer > 15f) break;
            timer += 1.5f;
        }

        Assert.True(enemyStats.isDead || enemyStats.health.CurrentValue < initialHealth, "Melee players failed to do any damage to the enemy");
        Assert.False(meleePlayerStats1.isDead, "Melee Player 1 should not have died in this scenario");
        Assert.False(meleePlayerStats2.isDead, "Melee Player 2 should not have died in this scenario");
    }
    
    [UnityTest]
    public IEnumerator CombatScenario_TwoPlayersFightingTwoEnemiesIndividually()
    {
        // Set up entities
        GameObject meleePlayer1 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(-2f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats1 = meleePlayer1.GetComponent<PlayerStatsController>();
        meleePlayerStats1.characterColour = CharacterColour.Green;
        
        GameObject meleePlayer2 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(2f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats2 = meleePlayer2.GetComponent<PlayerStatsController>();
        meleePlayerStats2.characterColour = CharacterColour.Red;
        
        GameObject enemy1 = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), new Vector3(-6f, 0, 6f), Quaternion.identity);
        EnemyStatsController enemyStats1 = enemy1.GetComponent<EnemyStatsController>();
        enemyStats1.characterColour = CharacterColour.Green;
        enemyStats1.health.maxValue = 10f;
        
        GameObject enemy2 = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), new Vector3(6f, 0, 6f), Quaternion.identity);
        EnemyStatsController enemyStats2 = enemy2.GetComponent<EnemyStatsController>();
        enemyStats2.characterColour = CharacterColour.Red;
        enemyStats2.health.maxValue = 10f;

        meleePlayer1.SetActive(true);
        meleePlayer2.SetActive(true);
        enemy1.SetActive(true);
        enemy2.SetActive(true);

        // Setup inputs
        PlayerInput input1 = meleePlayer1.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[0].SetInputToMockGamepad(input1);
        
        PlayerInput input2 = meleePlayer2.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[1].SetInputToMockGamepad(input2);

        // Wait for enemies to approach the players
        yield return new WaitForSeconds(1.5f);
        
        // Players each face their enemy
        _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, (new Vector2(enemy1.transform.position.x, enemy1.transform.position.z) -
                                                                 new Vector2(meleePlayer1.transform.position.x, meleePlayer1.transform.position.z)).normalized);
        yield return new WaitForSeconds(0.1f);
        _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, (new Vector2(enemy2.transform.position.x, enemy2.transform.position.z) -
                                                                 new Vector2(meleePlayer2.transform.position.x, meleePlayer2.transform.position.z)).normalized);

        // Players attack with both their attacks
        float timer = 0f;
        bool usePrimaryAttack = true;
        while (!enemyStats1.isDead || !enemyStats2.isDead)
        {
            if (usePrimaryAttack)
            {
                _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, (new Vector2(enemy1.transform.position.x, enemy1.transform.position.z) -
                                                                         new Vector2(meleePlayer1.transform.position.x, meleePlayer1.transform.position.z)).normalized);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[0].Press(_inputMocks[0].Gamepad.rightShoulder);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[0].Release(_inputMocks[0].Gamepad.rightShoulder);

                yield return new WaitForSeconds(0.3f);

                _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, (new Vector2(enemy2.transform.position.x, enemy2.transform.position.z) -
                                                                         new Vector2(meleePlayer2.transform.position.x, meleePlayer2.transform.position.z)).normalized);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[1].Press(_inputMocks[1].Gamepad.rightShoulder);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[1].Release(_inputMocks[1].Gamepad.rightShoulder);

                yield return new WaitForSeconds(1f);
            }
            else
            {
                _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, (new Vector2(enemy1.transform.position.x, enemy1.transform.position.z) -
                                                                         new Vector2(meleePlayer1.transform.position.x, meleePlayer1.transform.position.z)).normalized);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[0].Press(_inputMocks[0].Gamepad.rightTrigger);
                yield return new WaitForSeconds(0.2f);
                _inputMocks[0].Release(_inputMocks[0].Gamepad.rightTrigger);

                yield return new WaitForSeconds(0.3f);

                _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, (new Vector2(enemy2.transform.position.x, enemy2.transform.position.z) -
                                                                         new Vector2(meleePlayer2.transform.position.x, meleePlayer2.transform.position.z)).normalized);
                yield return new WaitForSeconds(0.1f);
                _inputMocks[1].Press(_inputMocks[1].Gamepad.rightTrigger);
                yield return new WaitForSeconds(0.2f);
                _inputMocks[1].Release(_inputMocks[1].Gamepad.rightTrigger);

                yield return new WaitForSeconds(2f);
            }
            
            // Alternate attack style
            usePrimaryAttack = !usePrimaryAttack;
            
            // Ensure that the test does not run longer than 15 seconds
            if (timer > 15f) break;
            timer += usePrimaryAttack ? 1.7f : 2.9f;
        }

        Assert.True(enemyStats1.isDead, "Melee player 1 failed to kill the enemy");
        Assert.True(enemyStats2.isDead, "Melee player 2 failed to kill the enemy");
        Assert.False(meleePlayerStats1.isDead, "Melee Player 1 should not have died in this scenario");
        Assert.False(meleePlayerStats2.isDead, "Melee Player 2 should not have died in this scenario");
    }
    
    [UnityTest]
    public IEnumerator CombatScenario_TwoPlayersDieToVeryStrongEnemy()
    {
        // Set up entities
        GameObject meleePlayer1 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(-2f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats1 = meleePlayer1.GetComponent<PlayerStatsController>();
        meleePlayerStats1.health.maxValue = 30f;
        meleePlayerStats1.characterColour = CharacterColour.Green;
        
        GameObject meleePlayer2 = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), new Vector3(2f, 0, 0), Quaternion.identity);
        PlayerStatsController meleePlayerStats2 = meleePlayer2.GetComponent<PlayerStatsController>();
        meleePlayerStats2.health.maxValue = 30f;
        meleePlayerStats2.characterColour = CharacterColour.Red;
        
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), new Vector3(0, 0, 6f), Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();
        enemyStats.characterColour = CharacterColour.Green;
        enemyStats.health.maxValue = 100f;
        enemyStats.damage.BaseValue = 20;

        meleePlayer1.SetActive(true);
        meleePlayer2.SetActive(true);
        enemy.SetActive(true);

        // Setup inputs
        PlayerInput input1 = meleePlayer1.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[0].SetInputToMockGamepad(input1);
        
        PlayerInput input2 = meleePlayer2.GetComponent<PlayerInput>();
        input1.enabled = true;
        _inputMocks[1].SetInputToMockGamepad(input2);

        // Wait for enemy to approach the players
        yield return new WaitForSeconds(0.5f);
        
        // Players each face the enemy
        _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, (new Vector2(enemy.transform.position.x, enemy.transform.position.z) -
                                                                 new Vector2(meleePlayer1.transform.position.x, meleePlayer1.transform.position.z)).normalized);
        yield return new WaitForSeconds(0.1f);
        _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, (new Vector2(enemy.transform.position.x, enemy.transform.position.z) -
                                                                 new Vector2(meleePlayer2.transform.position.x, meleePlayer2.transform.position.z)).normalized);

        // Players attack with their primary attack
        float timer = 0f;
        while (!meleePlayerStats1.isDead || !meleePlayerStats2.isDead)
        {
            if (enemyStats.isDead)
                break;
            
            _inputMocks[0].Press(_inputMocks[0].Gamepad.rightStick, (new Vector2(enemy.transform.position.x, enemy.transform.position.z) -
                                                                     new Vector2(meleePlayer1.transform.position.x, meleePlayer1.transform.position.z)).normalized);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[0].Press(_inputMocks[0].Gamepad.rightShoulder);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[0].Release(_inputMocks[0].Gamepad.rightShoulder);

            yield return new WaitForSeconds(0.3f);

            _inputMocks[1].Press(_inputMocks[1].Gamepad.rightStick, (new Vector2(enemy.transform.position.x, enemy.transform.position.z) -
                                                                     new Vector2(meleePlayer2.transform.position.x, meleePlayer2.transform.position.z)).normalized);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[1].Press(_inputMocks[1].Gamepad.rightShoulder);
            yield return new WaitForSeconds(0.1f);
            _inputMocks[1].Release(_inputMocks[1].Gamepad.rightShoulder);

            yield return new WaitForSeconds(1f);

            // Ensure that the test does not run longer than 15 seconds
            if (timer > 15f) break;
            timer += 1.7f;
        }

        Assert.False(enemyStats.isDead, "Enemy should have not died to the players in this scenario");
        Assert.True(meleePlayerStats1.isDead, "Melee Player 1 should have died in this scenario");
        Assert.True(meleePlayerStats2.isDead, "Melee Player 2 should have died in this scenario");
    }
}