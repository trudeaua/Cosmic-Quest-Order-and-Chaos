using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FunctionalRequirementsTests
{
    readonly float timeToWait = 1;

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
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // destroy all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Object.Destroy(player);
            // destroy all players
        }
        // destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Object.Destroy(enemy);
        }
        yield return null;
    }

    // FR-C16
    [UnityTest]
    public IEnumerator FunctionalRequirements_PlayerShouldTakeDamageFromEnemy()
    {
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject meleePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController meleePlayerStats = meleePlayer.GetComponent<PlayerStatsController>();
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);
        EnemyStatsController enemyStats = enemy.GetComponent<EnemyStatsController>();

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

        magePlayerStats.characterColour = CharacterColour.Green;
        meleePlayerStats.characterColour = CharacterColour.Red;

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject meleePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController meleePlayerStats = meleePlayer.GetComponent<PlayerStatsController>();
        GameObject healerPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Healer Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController healerPlayerStats = healerPlayer.GetComponent<PlayerStatsController>();
        GameObject rangedPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Ranged Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController rangedPlayerStats = rangedPlayer.GetComponent<PlayerStatsController>();

        magePlayer.transform.position = new Vector3(-4, 0, -2);
        meleePlayer.transform.position = new Vector3(-2, 0, -2);
        healerPlayer.transform.position = new Vector3(0, 0, -2);
        rangedPlayer.transform.position = new Vector3(2, 0, -2);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        GameObject meleePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        GameObject healerPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Healer Player"), Vector3.zero, Quaternion.identity);
        GameObject rangedPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Ranged Player"), Vector3.zero, Quaternion.identity);
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);

        magePlayer.transform.position = new Vector3(-4, 0, -2);
        meleePlayer.transform.position = new Vector3(-2, 0, -2);
        healerPlayer.transform.position = new Vector3(0, 0, -2);
        rangedPlayer.transform.position = new Vector3(2, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

        yield return new WaitForEndOfFrame();
        EntityStatsController[] entities = Object.FindObjectsOfType<EntityStatsController>();
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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        GameObject enemy = Object.Instantiate(TestResourceManager.Instance.GetResource("Enemy"), Vector3.zero, Quaternion.identity);

        magePlayer.transform.position = new Vector3(0, 0, -2);
        enemy.transform.Rotate(new Vector3(0, 1, 0), 180);

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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        magePlayerStats.characterColour = CharacterColour.Green;

        magePlayer.transform.position = new Vector3(0, 0, -2);

        GameObject lever = Object.Instantiate(TestResourceManager.Instance.GetResource("Lever"), Vector3.zero, Quaternion.identity);
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
        GameObject magePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Mage Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController magePlayerStats = magePlayer.GetComponent<PlayerStatsController>();
        GameObject meleePlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Melee Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController meleePlayerStats = meleePlayer.GetComponent<PlayerStatsController>();
        GameObject healerPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Healer Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController healerPlayerStats = healerPlayer.GetComponent<PlayerStatsController>();
        GameObject rangedPlayer = Object.Instantiate(TestResourceManager.Instance.GetResource("Ranged Player"), Vector3.zero, Quaternion.identity);
        PlayerStatsController rangedPlayerStats = rangedPlayer.GetComponent<PlayerStatsController>();
        magePlayerStats.characterColour = CharacterColour.Green;
        meleePlayerStats.characterColour = CharacterColour.Red;
        healerPlayerStats.characterColour = CharacterColour.Blue;
        rangedPlayerStats.characterColour = CharacterColour.Yellow;

        magePlayer.transform.position = new Vector3(0, 0, -2);
        meleePlayer.transform.position = new Vector3(-2, 0, -2);

        GameObject lever = Object.Instantiate(TestResourceManager.Instance.GetResource("Lever"), Vector3.zero, Quaternion.identity);
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
