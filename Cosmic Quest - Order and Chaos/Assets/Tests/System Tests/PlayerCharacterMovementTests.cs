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
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(1);

        if (_inputMock is null)
            _inputMock = new PlayerInputMock();
        
        // Enable required components
        GameObject.Find("GameManager").SetActive(true);
        GameObject.Find("Main Camera").SetActive(true);
        
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Test Player"), Vector3.zero, Quaternion.identity);

        // Wait for components to become active
        yield return new WaitForSeconds(0.5f);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(GameObject.FindWithTag("Player"));
        GameObject.Find("Main Camera").SetActive(false);
        GameObject.Find("GameManager").SetActive(false);

        yield return new WaitForSeconds(0.5f);
    }
    
    // Basic Player Movement Tests
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheRightWithLeftJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;
        
        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.right);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheLeftWithLeftJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;
        
        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.left);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardWithLeftJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;
        
        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardsWithLeftJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;
        
        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, Vector2.down);
        yield return new WaitForSeconds(1f);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backwards on input");
    }
    
    // Basic Player Look Direction Tests

    [UnityTest]
    public IEnumerator PlayerCharacter_LookRightWithRightJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.right);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 90f, 1f, "Player did not look right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookLeftWithRightJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.left);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 270f, 1f, "Player did not look left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookForwardWithRightJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 0f, 1f, "Player did not look forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookBackwardWithRightJoystick()
    {
        GameObject player = GameObject.FindWithTag("Player");
        
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, Vector2.down);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 180f, 1f, "Player did not look backwards on input");
    }
    
    // Movement and Look Direction Combined Tests
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardAndLeftWhileLookingDownToTheRightUsingBothJoysticks()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 initialPos = player.transform.position;
        
        _inputMock.Set(PlayerInputMock.MockInput.LeftStick, (Vector2.up + Vector2.left).normalized);
        yield return null;
        _inputMock.Set(PlayerInputMock.MockInput.RightStick, (Vector2.down + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 135f, 1f, "Player did not look in the correct direction on input");
    }
}
