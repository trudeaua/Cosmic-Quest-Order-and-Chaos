using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class CombatTestSuite
    {
        
        public class CombatPlayTest
        {
            private GameObject player1;
            private GameObject player2;
            private GameObject player3;
            private GameObject player4;
            private GameObject enemy;
            private PlayerStatsController player1Stats;
            private PlayerStatsController player2Stats;
            private PlayerStatsController player3Stats;
            private PlayerStatsController player4Stats;
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

                yield return new WaitUntil(() => sceneLoading.isDone);

                Assert.IsTrue(sceneLoading.isDone, "Scene load timed out! The scene could not be loaded.");
                

                player1 = GameObject.Find("Mage Player");
                player1Stats = player1.GetComponent<PlayerStatsController>();
                player1.SetActive(false);

                player2 = GameObject.Find("Melee Player");
                player2Stats = player2.GetComponent<PlayerStatsController>();
                player2.SetActive(false);

                player3 = GameObject.Find("Healer Player");
                player3Stats = player3.GetComponent<PlayerStatsController>();
                player3.SetActive(false);

                player4 = GameObject.Find("Ranged Player");
                player4Stats = player4.GetComponent<PlayerStatsController>();
                player4.SetActive(false);

                enemy = GameObject.Find("Enemy");
                enemyStats = enemy.GetComponent<EnemyStatsController>();
                enemy.SetActive(false);

                player1Stats.characterColour = CharacterColour.Green;
                player2Stats.characterColour = CharacterColour.Red;
                player3Stats.characterColour = CharacterColour.Purple;
                player4Stats.characterColour = CharacterColour.Yellow;
                enemyStats.characterColour = CharacterColour.Green;
            }

            [UnityTearDown]
            public IEnumerator TearDown()
            {
                PlayerManager.DeregisterPlayer(player1);
                PlayerManager.DeregisterPlayer(player2);
                PlayerManager.DeregisterPlayer(player3);
                PlayerManager.DeregisterPlayer(player4);
                yield return null;
            }

            // FR-C16
            [UnityTest]
            public IEnumerator Combat_PlayerShouldTakeDamageFromEnemy()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);
                float initialHp = player1Stats.health.CurrentValue;

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();


                yield return new WaitForSeconds(timeToWait);

                Assert.Less(player1Stats.health.CurrentValue, initialHp, "Player did not lose health");

                yield return null;
            }

            // FR-C17
            [UnityTest]
            public IEnumerator Combat_PlayerShouldBeRemovedFromSceneWhenItDies()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);

                player1Stats.health.maxValue = 1;
                player1Stats.health.Init();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();


                // Wait for player to despawn
                float timer = 0;
                while (player1.activeSelf)
                {
                    if (timer > 15f)
                    {
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                Assert.IsTrue(player1.activeSelf == false, "Player was not removed from scene");

                yield return null;
            }

            // FR-C14
            [UnityTest]
            public IEnumerator Combat_EnemyShouldBeRemovedFromSceneWhenItDies()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                enemyStats.health.maxValue = 1;
                enemyStats.health.Init();

                enemyStats.TakeDamage(player1Stats, 50f);

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
            public IEnumerator Combat_EnemyShouldTakeDamageFromPlayer()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                float initialHp = enemyStats.health.CurrentValue;

                enemyStats.TakeDamage(player1Stats, 10f);
                yield return new WaitForSeconds(timeToWait);

                Assert.Less(enemyStats.health.CurrentValue, initialHp, "Enemy did not lose health");

                yield return null;
            }

            // FR-C15
            [UnityTest]
            public IEnumerator Combat_PlayerShouldBeDamagedByEnemiesOfAnyColour()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);
                enemyStats.characterColour = CharacterColour.Red;
                float initialHp = player1Stats.health.CurrentValue;

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                player1Stats.TakeDamage(player1Stats, 10f);
                yield return new WaitForSeconds(timeToWait);

                Assert.Less(player1Stats.health.CurrentValue, initialHp, "Player did not lose health");

                yield return null;
            }

            // FR-C12
            [UnityTest]
            public IEnumerator Combat_EnemyShouldOnlyBeDamagedByPlayersOfSameColour()
            {

                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(true);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                enemyStats.characterColour = CharacterColour.Red;
                float initialHp = enemyStats.health.CurrentValue;

                enemyStats.TakeDamage(player1Stats, 10f);
                yield return new WaitForSeconds(timeToWait);
                Assert.AreEqual(enemyStats.health.CurrentValue, initialHp, "Enemy did not lose health");
                
                enemyStats.TakeDamage(player2Stats, 10f);
                yield return new WaitForSeconds(timeToWait);
                Assert.Less(enemyStats.health.CurrentValue, initialHp, "Enemy did not lose health");

                yield return null;
            }

            //FR-C11
            [UnityTest]
            public IEnumerator Combat_EachPlayerShouldHaveAUniqueColour()
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                Assert.IsTrue(player1Stats.characterColour != player2Stats.characterColour);
                Assert.IsTrue(player1Stats.characterColour != player3Stats.characterColour);
                Assert.IsTrue(player1Stats.characterColour != player4Stats.characterColour);
                Assert.IsTrue(player2Stats.characterColour != player3Stats.characterColour);
                Assert.IsTrue(player2Stats.characterColour != player4Stats.characterColour);
                Assert.IsTrue(player3Stats.characterColour != player4Stats.characterColour);

                yield return null;
            }

            //FR-C18
            [UnityTest]
            public IEnumerator Combat_HealthBarShouldBeVisibleAboveAllEntities()
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);
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
            public IEnumerator Combat_EnemiesShouldFollowAPlayerWithinTheirVicinity()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                enemy.SetActive(true);
                Vector3 originalEnemyPos = enemy.transform.position;
                for (int i = 0; i < 60; i++)
                {
                    player1.transform.position += new Vector3(0.1f, 0, 0);
                    yield return new WaitForEndOfFrame();
                }

                Assert.Greater(enemy.transform.position.x, originalEnemyPos.x);
                
                yield return null;
            }

            //FR-C20
            [UnityTest]
            public IEnumerator Combat_PlayersShouldBeAbleToInteractWithObjects()
            {
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(false);

                GameObject lever = GameObject.Find("VRLever");
                lever.transform.position = new Vector3(0, 0, 0);
                Interactable handle = lever.GetComponentInChildren<Interactable>();
                handle.colour = CharacterColour.Green;

                player1.transform.position = new Vector3(1, 0, 0);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                Assert.IsFalse(handle.CanInteract(player1.transform));

                yield return null;
            }

            //FR-C21
            [UnityTest]
            public IEnumerator Combat_PlayersShouldOnlyBeAbleToInteractWithObjectsOfTheirColour()
            {
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(false);
                player4.SetActive(false);
                enemy.SetActive(false);

                GameObject lever = GameObject.Find("VRLever");
                lever.transform.position = new Vector3(0, 0, 0);
                Interactable handle = lever.GetComponentInChildren<Interactable>();
                handle.colour = CharacterColour.Red;

                player1.transform.position = new Vector3(1, 0, 0);
                player2.transform.position = new Vector3(1, 0, 0);

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();

                Assert.IsFalse(handle.CanInteract(player1.transform));
                Assert.IsTrue(handle.CanInteract(player2.transform));

                yield return null;
            }
        }
    }
}
