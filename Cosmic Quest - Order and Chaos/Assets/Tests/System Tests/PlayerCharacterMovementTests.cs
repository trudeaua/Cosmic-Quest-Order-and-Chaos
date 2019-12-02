using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

public class PlayerCharacterMovementTests
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(1);

        // Enable required components
        GameObject.Find("GameManager").SetActive(true);
        GameObject.Find("Main Camera").SetActive(true);
        
        GameObject player = GameObject.Find("Test Player");
        player.GetComponent<PlayerInput>().enabled = false;
        player.SetActive(true);
        

        // Wait for components to become active
        yield return new WaitForSeconds(0.5f);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Find("Test Player").SetActive(false);
        GameObject.Find("Main Camera").SetActive(false);
        GameObject.Find("GameManager").SetActive(false);

        yield return new WaitForSeconds(0.5f);
    }

    /*
     * Basic Player Movement Tests
     */
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheRightWithLeftJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        Vector3 initialPos = player.transform.position;
        
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.right);
        yield return new WaitForSeconds(1f);
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.zero);

        Assert.Greater(player.transform.position.x, initialPos.x, "Player did not move to the right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveToTheLeftWithLeftJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        Vector3 initialPos = player.transform.position;
        
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.left);
        yield return new WaitForSeconds(1f);
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.zero);

        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardWithLeftJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        Vector3 initialPos = player.transform.position;
        
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.up);
        yield return new WaitForSeconds(1f);
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.zero);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveBackwardsWithLeftJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        Vector3 initialPos = player.transform.position;
        
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.down);
        yield return new WaitForSeconds(1f);
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, Vector2.zero);

        Assert.Less(player.transform.position.z, initialPos.z, "Player did not move backwards on input");
    }
    
    /*
     * Basic Player Look Direction Tests
     */
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookRightWithRightJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        
        inputMock.Set(PlayerInputMock.MockGamepad.RightStick, Vector2.right);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 90f, 1f, "Player did not look right on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookLeftWithRightJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        
        inputMock.Set(PlayerInputMock.MockGamepad.RightStick, Vector2.left);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 270f, 1f, "Player did not look left on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookForwardWithRightJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        
        inputMock.Set(PlayerInputMock.MockGamepad.RightStick, Vector2.up);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 0f, 1f, "Player did not look forward on input");
    }
    
    [UnityTest]
    public IEnumerator PlayerCharacter_LookBackwardWithRightJoystick()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        
        inputMock.Set(PlayerInputMock.MockGamepad.RightStick, Vector2.down);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(player.transform.eulerAngles.y, 180f, 1f, "Player did not look backwards on input");
    }
    
    /*
     * Movement and Look Direction Combined Tests
     */
    
    [UnityTest]
    public IEnumerator PlayerCharacter_MoveForwardAndLeftWhileLookingDownToTheRightUsingBothJoysticks()
    {
        GameObject player = GameObject.Find("Test Player");
        PlayerInputMock inputMock = player.GetComponent<PlayerInputMock>();
        Vector3 initialPos = player.transform.position;
        
        inputMock.Set(PlayerInputMock.MockGamepad.LeftStick, (Vector2.up + Vector2.left).normalized);
        inputMock.Set(PlayerInputMock.MockGamepad.RightStick, (Vector2.down + Vector2.right).normalized);
        yield return new WaitForSeconds(1f);

        Assert.Greater(player.transform.position.z, initialPos.z, "Player did not move forward on input");
        Assert.Less(player.transform.position.x, initialPos.x, "Player did not move to the left on input");
        Assert.AreEqual(player.transform.rotation.eulerAngles.y, 135f, 1f, "Player did not look in the correct direction on input");
    }
}
