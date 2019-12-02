using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerCharacterMovementTests
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

        magePlayer = GameObject.Find("Mage Player");
        magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        magePlayer.SetActive(true);

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

    // Basic Player Movement Tests

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheRightWithLeftJoystick()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.right);
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheLeftWithLeftJoystick()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.left);
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardWithLeftJoystick()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.up);
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardsWithLeftJoystick()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.down);
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backwards on input");
    }
    
    // Basic Player Look Direction Tests

    [UnityTest]
    public IEnumerator PlayerCharacter_LookRightWithRightJoystick()
    {
        GameObject player = magePlayer;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.right);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 90f, 1f, "Player did not look right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookLeftWithRightJoystick()
    {
        GameObject player = magePlayer;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.left);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 270f, 1f, "Player did not look left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookForwardWithRightJoystick()
    {
        GameObject player = magePlayer;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 0f, 1f, "Player did not look forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookBackwardWithRightJoystick()
    {
        GameObject player = magePlayer;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.down);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 180f, 1f, "Player did not look backwards on input");
    }
    
    // Movement and Look Direction Combined Tests
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardAndLeftWhileLookingDownToTheRightUsingBothJoysticks()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, (Vector2.up + Vector2.left).normalized);
        yield return null;
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, (Vector2.down + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 135f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardAndRightWhileLookingDownToTheLeftUsingBothJoysticks()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, (Vector2.up + Vector2.right).normalized);
        yield return null;
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, (Vector2.down + Vector2.left).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 225f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardAndLeftWhileLookingUpToTheRightUsingBothJoysticks()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, (Vector2.down + Vector2.left).normalized);
        yield return null;
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, (Vector2.up + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 45f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardAndRightWhileLookingUpToTheLeftUsingBothJoysticks()
    {
        GameObject player = magePlayer;
        Vector3 initialPos = player.transform.position;

        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad")
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                player.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }

        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, (Vector2.down + Vector2.right).normalized);
        yield return null;
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, (Vector2.up + Vector2.left).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backward on input");
        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 315f, 1f, "Player did not look in the correct direction on input");
    }
}
