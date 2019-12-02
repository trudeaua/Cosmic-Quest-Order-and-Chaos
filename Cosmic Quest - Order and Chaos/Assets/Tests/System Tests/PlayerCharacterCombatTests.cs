using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerCharacterCombatTests
{
    private GameObject magePlayer;
    private GameObject meleePlayer;
    private GameObject healerPlayer;
    private GameObject rangedPlayer;
    private GameObject enemy;
    private PlayerStatsController magePlayerStats;
    private PlayerStatsController meleePlayerStats;
    private PlayerStatsController healerPlayerStats;
    private PlayerStatsController rangedPlayerStats;
    private EnemyStatsController enemyStats;
    readonly float timeToWait = 1;

    private PlayerInputMock _inputMock = null;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("TestScene_Alex", LoadSceneMode.Single);

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

        magePlayer = GameObject.Find("Mage Player");
        magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        magePlayer.SetActive(false);

        meleePlayer = GameObject.Find("Melee Player");
        meleePlayerStats = meleePlayer.GetComponent<PlayerStatsController>();
        meleePlayer.SetActive(false);

        healerPlayer = GameObject.Find("Healer Player");
        healerPlayerStats = healerPlayer.GetComponent<PlayerStatsController>();
        healerPlayer.SetActive(false);

        rangedPlayer = GameObject.Find("Ranged Player");
        rangedPlayerStats = rangedPlayer.GetComponent<PlayerStatsController>();
        rangedPlayer.SetActive(false);

        enemy = GameObject.Find("Enemy");
        enemyStats = enemy.GetComponent<EnemyStatsController>();
        enemy.SetActive(false);

        magePlayerStats.characterColour = CharacterColour.Green;
        meleePlayerStats.characterColour = CharacterColour.Red;
        healerPlayerStats.characterColour = CharacterColour.Purple;
        rangedPlayerStats.characterColour = CharacterColour.Yellow;
        enemyStats.characterColour = CharacterColour.Green;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        PlayerManager.DeregisterPlayer(magePlayer);
        PlayerManager.DeregisterPlayer(meleePlayer);
        PlayerManager.DeregisterPlayer(healerPlayer);
        PlayerManager.DeregisterPlayer(rangedPlayer);
        yield return null;
    }

    /*
     * Basic Combat Test Scenarios
     */

    [UnityTest]
    public IEnumerator PlayerCombat_MeleePlayerCanDamageEnemyWithPrimaryAndSecondaryAttacks()
    {
        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        meleePlayer.SetActive(true);
        enemy.SetActive(true);

        var input = meleePlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                input.SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }
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
        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        magePlayer.SetActive(true);
        enemy.SetActive(true);

        var input = magePlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                input.SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }
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
        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        healerPlayer.SetActive(true);
        enemy.SetActive(true);

        var input = healerPlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                input.SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }
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
        Vector3 attackPos = new Vector3(0, 0, -2.5f);

        rangedPlayer.SetActive(true);
        enemy.SetActive(true);

        var input = rangedPlayer.GetComponent<PlayerInput>();
        input.enabled = true;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                input.SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }
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
