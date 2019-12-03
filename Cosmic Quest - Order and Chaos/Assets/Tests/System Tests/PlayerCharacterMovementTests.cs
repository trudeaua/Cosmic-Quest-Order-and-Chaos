using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

public class PlayerCharacterMovementTests
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

        // Enable required components
        GameObject.Find("GameManager").SetActive(true);
        GameObject.Find("Main Camera").SetActive(true);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(GameObject.FindWithTag("Player"));
        GameObject.Find("Main Camera").SetActive(false);
        GameObject.Find("GameManager").SetActive(false);

        yield return null;
    }

    // Basic Player Movement Tests

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheRightWithLeftJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);

        Vector3 initialPos = player.transform.position;
        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);


        _inputMock.Press(_inputMock.Gamepad.leftStick, Vector2.right);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);

        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 90f, 1f, "Player did not look in the direction of travel");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheLeftWithLeftJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, Vector2.left);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);

        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 270f, 1f, "Player did not look in the direction of travel");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardWithLeftJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        player.transform.Rotate(new Vector3(0, 180f, 0), Space.World);
        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, Vector2.up);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 0f, 1f, "Player did not look in the direction of travel");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardsWithLeftJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, Vector2.down);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backwards on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 180f, 1f, "Player did not look in the direction of travel");
    }
    
    // Basic Player Look Direction Tests

    [UnityTest]
    public IEnumerator PlayerCharacter_LookRightWithRightJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.rightStick, Vector2.right);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.AreEqual(player.transform.eulerAngles.y, 90f, 1f, "Player did not look right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookLeftWithRightJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.rightStick, Vector2.left);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.AreEqual(player.transform.eulerAngles.y, 270f, 1f, "Player did not look left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookForwardWithRightJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        player.transform.Rotate(new Vector3(0, 180f, 0), Space.World);
        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.rightStick, Vector2.up);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.AreEqual(player.transform.eulerAngles.y, 0f, 1f, "Player did not look forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookBackwardWithRightJoystick()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.rightStick, Vector2.down);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.AreEqual(player.transform.eulerAngles.y, 180f, 1f, "Player did not look backwards on input");
    }
    
    // Movement and Look Direction Combined Tests
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardLeftLookingDownRight()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, (Vector2.up + Vector2.left).normalized);
        _inputMock.Press(_inputMock.Gamepad.rightStick, (Vector2.down + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 135f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardRightLookingDownLeft()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        
        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, (Vector2.up + Vector2.right).normalized);
        _inputMock.Press(_inputMock.Gamepad.rightStick, (Vector2.down + Vector2.left).normalized);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 225f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardLeftLookingUpRight()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);

        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, (Vector2.down + Vector2.left).normalized);
        _inputMock.Press(_inputMock.Gamepad.rightStick, (Vector2.up + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 45f, 1f, "Player did not look in the correct direction on input");
    }

    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardRightLookingUpLeft()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);

        Vector3 initialPos = player.transform.position;

        PlayerInput input = player.GetComponent<PlayerInput>();
        _inputMock.SetInputToMockGamepad(input);

        _inputMock.Press(_inputMock.Gamepad.leftStick, (Vector2.down + Vector2.right).normalized);
        _inputMock.Press(_inputMock.Gamepad.rightStick, (Vector2.up + Vector2.left).normalized);
        yield return new WaitForSeconds(1f);
        _inputMock.Release(_inputMock.Gamepad.leftStick, Vector2.zero);
        _inputMock.Release(_inputMock.Gamepad.rightStick, Vector2.zero);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backward on input");
        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 315f, 1f, "Player did not look in the correct direction on input");
    }
}
