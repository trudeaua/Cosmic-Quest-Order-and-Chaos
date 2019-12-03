using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerCharacterCombatTests
{
    private PlayerInputMock _inputMock = null;

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

        if (_inputMock is null)
            _inputMock = new PlayerInputMock();

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
     * Basic Combat Test Scenarios
     */

    [UnityTest]
    public IEnumerator PlayerCombat_MeleePlayerCanDamageEnemyWithPrimaryAndSecondaryAttacks()
    {
        GameObject meleePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController meleePlayerStats = meleePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        meleePlayer.SetActive(true);
        enemy.SetActive(true);

        var input = meleePlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        _inputMock.SetInputToMockGamepad(input);

        meleePlayer.transform.position = attackPos;

        float initialHealth = enemyStats.health.CurrentValue;
        enemyStats.characterColour = meleePlayerStats.characterColour;

        yield return new WaitForSeconds(0.5f);

        // Player attacks with primary attack
        _inputMock.Press(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Melee player was unable to damage the enemy with primary attack!");

        initialHealth = enemyStats.health.CurrentValue;

        // Player attacks with secondary attack
        _inputMock.Press(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Melee player was unable to damage the enemy with secondary attack!");
    }
    
    [UnityTest]
    public IEnumerator PlayerCombat_MagePlayerCanDamageEnemyWithPrimaryAndSecondaryAttacks()
    {
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        magePlayer.SetActive(true);
        enemy.SetActive(true);

        var input = magePlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        _inputMock.SetInputToMockGamepad(input);

        magePlayer.transform.position = attackPos;

        float initialHealth = enemyStats.health.CurrentValue;
        enemyStats.characterColour = magePlayerStats.characterColour;

        yield return new WaitForSeconds(0.5f);

        // Player attacks with primary attack
        _inputMock.Press(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Mage player was unable to damage the enemy with primary attack!");

        initialHealth = enemyStats.health.CurrentValue;

        // Player attacks with secondary attack
        _inputMock.Press(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Mage player was unable to damage the enemy with secondary attack!");
    }

    [UnityTest]
    public IEnumerator PlayerCombat_HealerPlayerCanDamageEnemyWithPrimaryAndSecondaryAttacks()
    {
        GameObject healerPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Healer Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController healerPlayerStats = healerPlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        healerPlayer.SetActive(true);
        enemy.SetActive(true);

        var input = healerPlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        _inputMock.SetInputToMockGamepad(input);

        healerPlayer.transform.position = attackPos;

        float initialHealth = enemyStats.health.CurrentValue;
        enemyStats.characterColour = healerPlayerStats.characterColour;

        yield return new WaitForSeconds(0.5f);

        // Player attacks with primary attack
        _inputMock.Press(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Healer player was unable to damage the enemy with primary attack!");

        initialHealth = enemyStats.health.CurrentValue;

        // Player attacks with secondary attack
        _inputMock.Press(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Healer player was unable to damage the enemy with secondary attack!");
    }


    [UnityTest]
    public IEnumerator PlayerCombat_RangedPlayerCanDamageEnemyWithPrimaryAndSecondaryAttacks()
    {
        GameObject rangedPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Ranged Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController rangedPlayerStats = rangedPlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        rangedPlayer.SetActive(true);
        enemy.SetActive(true);

        var input = rangedPlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        _inputMock.SetInputToMockGamepad(input);

        rangedPlayer.transform.position = attackPos;

        float initialHealth = enemyStats.health.CurrentValue;
        enemyStats.characterColour = rangedPlayerStats.characterColour;

        yield return new WaitForSeconds(0.5f);

        // Player attacks with primary attack
        _inputMock.Press(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Ranged player was unable to damage the enemy with primary attack!");

        initialHealth = enemyStats.health.CurrentValue;

        // Player attacks with secondary attack
        _inputMock.Press(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightTrigger);
        yield return new WaitForSeconds(1f);
        Assert.Less(enemyStats.health.CurrentValue, initialHealth, "Ranged player was unable to damage the enemy with secondary attack!");
    }
}
