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
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(1);

        if (_inputMock is null)
            _inputMock = new PlayerInputMock();
        
        // Enable required components
        GameObject.Find("GameManager").SetActive(true);
        GameObject.Find("Main Camera").SetActive(true);

        // Wait for components to become active
        yield return new WaitForSeconds(0.5f);
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Find("Main Camera").SetActive(false);
        GameObject.Find("GameManager").SetActive(false);

        yield return new WaitForSeconds(0.5f);
    }
    
    /*
     * Basic Combat Test Scenarios
     */

    [UnityTest]
    public IEnumerator PlayerCombat_MeleePlayerCanDamageWithPrimaryAndSecondaryAttacks()
    {
        GameObject player = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerInput>().enabled = false;

        GameObject dummy = Object.Instantiate(TestResourceManager.Instance.GetResource("Dummy Enemy"), new Vector3(0, 0, 2.5f), Quaternion.identity);
        float initialHealth = dummy.GetComponent<EnemyStatsController>().health.CurrentValue;
        
        yield return new WaitForSeconds(0.5f);
        
        // Player attacks with primary attack
        _inputMock.Press(_inputMock.Gamepad.rightShoulder);
        yield return new WaitForSeconds(0.1f);
        _inputMock.Release(_inputMock.Gamepad.rightShoulder);
        
        yield return new WaitForSeconds(1f);

        Assert.Less(dummy.GetComponent<EnemyStatsController>().health.CurrentValue, initialHealth, "Player was unable to damage the enemy!");
    }
}
