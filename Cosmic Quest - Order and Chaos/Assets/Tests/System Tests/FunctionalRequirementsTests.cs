using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FunctionalRequirementsTests
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

    // FR-C16
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayerShouldTakeDamageFromEnemy()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);
        float initialHp = magePlayerStats.health.CurrentValue;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        magePlayerStats.TakeDamage(enemyStats, 10f);

        yield return new WaitForSeconds(timeToWait);

        Assert.Less(magePlayerStats.health.CurrentValue, initialHp, "Player did not lose health");

        yield return null;
    }

    // FR-C17
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayerShouldBeRemovedFromSceneWhenItDies()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);

        magePlayerStats.health.maxValue = 1;
        magePlayerStats.health.Init();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        // Wait for player to despawn
        float timer = 0;
        while (magePlayer.activeSelf)
        {
            if (timer > 15f)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Assert.IsTrue(magePlayer.activeSelf == false, "Player was not removed from scene");

        yield return null;
    }

    // FR-C14
    [UnityTest]
    public IEnumerator FunctionalRequirements_EnemyShouldBeRemovedFromSceneWhenItDies()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        enemyStats.health.maxValue = 1;
        enemyStats.health.Init();

        enemyStats.TakeDamage(magePlayerStats, 50f);

        // Wait for enemy to despawn
        float timer = 0;
        while (enemy.activeSelf)
        {
            if (timer > 15f)
            {
                break;
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Assert.IsTrue(enemy.activeSelf == false, "Enemy was not removed from scene");

        yield return null;
    }

    // FR-C13
    [UnityTest]
    public IEnumerator FunctionalRequirements_EnemyShouldTakeDamageFromPlayer()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        float initialHp = enemyStats.health.CurrentValue;

        enemyStats.TakeDamage(magePlayerStats, 10f);
        yield return new WaitForSeconds(timeToWait);

        Assert.Less(enemyStats.health.CurrentValue, initialHp, "Enemy did not lose health");

        yield return null;
    }

    // FR-C15
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayerShouldBeDamagedByEnemiesOfAnyColour()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);
        enemyStats.characterColour = CharacterColour.Red;
        float initialHp = magePlayerStats.health.CurrentValue;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        magePlayerStats.TakeDamage(enemyStats, 10f);
        yield return new WaitForSeconds(timeToWait);

        Assert.Less(magePlayerStats.health.CurrentValue, initialHp, "Player did not lose health");

        yield return null;
    }

    // FR-C12
    [UnityTest]
    public IEnumerator FunctionalRequirements_EnemyShouldOnlyBeDamagedByPlayersOfSameColour()
    {

        magePlayer.SetActive(true);
        meleePlayer.SetActive(true);
        enemy.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        enemyStats.characterColour = CharacterColour.Red;
        float initialHp = enemyStats.health.CurrentValue;

        enemyStats.TakeDamage(magePlayerStats, 10f);
        yield return new WaitForSeconds(timeToWait);
        Assert.AreEqual(enemyStats.health.CurrentValue, initialHp, "Enemy lost health when player colour was different");
                
        enemyStats.TakeDamage(meleePlayerStats, 10f);
        yield return new WaitForSeconds(timeToWait);
        Assert.Less(enemyStats.health.CurrentValue, initialHp, "Enemy did not lose health when player colour was same");

        yield return null;
    }

    //FR-C11
    [UnityTest]
    public IEnumerator FunctionalRequirements_EachPlayerShouldHaveAUniqueColour()
    {
        magePlayer.SetActive(true);
        meleePlayer.SetActive(true);
        healerPlayer.SetActive(true);
        rangedPlayer.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.IsTrue(magePlayerStats.characterColour != meleePlayerStats.characterColour);
        Assert.IsTrue(magePlayerStats.characterColour != healerPlayerStats.characterColour);
        Assert.IsTrue(magePlayerStats.characterColour != rangedPlayerStats.characterColour);
        Assert.IsTrue(meleePlayerStats.characterColour != healerPlayerStats.characterColour);
        Assert.IsTrue(meleePlayerStats.characterColour != rangedPlayerStats.characterColour);
        Assert.IsTrue(healerPlayerStats.characterColour != rangedPlayerStats.characterColour);

        yield return null;
    }

    //FR-C18
    [UnityTest]
    public IEnumerator FunctionalRequirements_HealthBarShouldBeVisibleAboveAllEntities()
    {
        magePlayer.SetActive(true);
        meleePlayer.SetActive(true);
        healerPlayer.SetActive(true);
        rangedPlayer.SetActive(true);
        enemy.SetActive(true);

        yield return new WaitForEndOfFrame();
        EntityStatsController[] entities = Object.FindObjectsOfType<EntityStatsController>();
        Debug.Log(entities.Length);
        foreach(EntityStatsController entity in entities)
        {
            StatBar bar = entity.GetComponentInChildren<StatBar>();
            Assert.IsNotNull(bar, "Health bar was not found on " + entity.gameObject.name);
        }

        yield return null;
    }

    //FR-C27
    [UnityTest]
    public IEnumerator FunctionalRequirements_EnemiesShouldFollowAPlayerWithinTheirVicinity()
    {
        magePlayer.SetActive(true);
        enemy.SetActive(true);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Vector3 originalEnemyPos = enemy.transform.position;
        for (int i = 0; i < 60; i++)
        {
            magePlayer.transform.position += new Vector3(0.1f, 0, 0);
            yield return new WaitForEndOfFrame();
        }

        Assert.Greater(enemy.transform.position.x, originalEnemyPos.x);
                
        yield return null;
    }

    //FR-C20
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayersShouldBeAbleToInteractWithObjects()
    {
        magePlayer.SetActive(true);

        GameObject lever = GameObject.Find("VRLever");
        lever.transform.position = new Vector3(0, 0, 0);
        Interactable handle = lever.GetComponentInChildren<Interactable>();
        handle.colour = CharacterColour.Green;

        magePlayer.transform.position = new Vector3(1, 0, 0);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.IsTrue(handle.CanInteract(magePlayer.transform));

        yield return null;
    }

    //FR-C21
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayersShouldOnlyBeAbleToInteractWithObjectsOfTheirColour()
    {
        magePlayer.SetActive(true);
        meleePlayer.SetActive(true);

        GameObject lever = GameObject.Find("VRLever");
        lever.transform.position = new Vector3(0, 0, 0);
        Interactable handle = lever.GetComponentInChildren<Interactable>();
        handle.colour = CharacterColour.Red;

        magePlayer.transform.position = new Vector3(1, 0, 0);
        meleePlayer.transform.position = new Vector3(-1, 0, 0);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.IsFalse(handle.CanInteract(magePlayer.transform));
        Assert.IsTrue(handle.CanInteract(meleePlayer.transform));

        yield return null;
    }


}
