using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class EntityUnitTests
    {
        private EntityStatsController entityStats;

        [SetUp]
        public void Setup()
        {
            entityStats = new GameObject().AddComponent<EntityStatsController>();
            entityStats.damage = new Stat();
            entityStats.defense = new Stat();
            entityStats.health = new RegenerableStat();

            entityStats.damage.BaseValue = 10;
            entityStats.defense.BaseValue = 10;
            entityStats.health.maxValue = 100f;
            entityStats.health.Init();
        }

        [TearDown]
        public void Teardown()
        {
        }

        // A Test behaves as an ordinary method
        [Test]
        public void EntityStatsController_ComputeDamageModifer_ShouldCalculateDamageWithinCorrectRange()
        {
            entityStats.damage.BaseValue = 10;

            float damageModifier = entityStats.ComputeDamageModifer();
            float baseDamage = entityStats.damage.GetBaseValue();

            Assert.IsTrue(0f <= damageModifier && damageModifier <= baseDamage);
        }

        [Test]
        public void EntityStatsController_ComputeDamageModifer_ShouldNotCalculateSameValueEveryTime()
        {
            int runs = 15;
            float damageModifier;
            List<float> values = new List<float>();

            for (int i = 0; i < runs; i++)
            {
                damageModifier = entityStats.ComputeDamageModifer();
                values.Add(damageModifier);
            }
            values.Sort();

            Assert.IsTrue(values[0] != values[values.Count - 1]);
        }

        [Test]
        public void EntityStatsController_ComputeDefenseModifer_ShouldCalculateDefenseWithinCorrectRange()
        {
            float defenseModifier = entityStats.ComputeDefenseModifier();
            float baseDefense = entityStats.defense.GetBaseValue();

            Assert.IsTrue(0f <= defenseModifier && defenseModifier <= baseDefense);
        }

        [Test]
        public void EntityStatsController_ComputeDefenseModifer_ShouldNotCalculateSameValueEveryTime()
        {
            int runs = 15;
            float defenseModifier;
            List<float> values = new List<float>();

            for (int i = 0; i < runs; i++)
            {
                defenseModifier = entityStats.ComputeDamageModifer();
                values.Add(defenseModifier);
            }
            values.Sort();

            Assert.IsTrue(values[0] != values[values.Count - 1]);
        }

        [Test]
        public void EntityStatsController_TakeDamage_ShouldReducePlayerHealth()
        {
            float damage = 50f;
            EntityStatsController fakeAttacker = new GameObject().AddComponent<EntityStatsController>();
            float oldPlayerHealth = entityStats.health.CurrentValue;
            
            entityStats.TakeDamage(fakeAttacker, damage);
            float newPlayerHealth = entityStats.health.CurrentValue;

            Assert.IsTrue(oldPlayerHealth > newPlayerHealth);
        }

        [Test]
        public void EntityStatsController_TakeDamage_ShouldKillPlayerWhenDamageTakenIsGreaterThanCurrentHealth()
        {
            float damage = 200f;
            EntityStatsController fakeAttacker = new GameObject().AddComponent<EntityStatsController>();
            float oldPlayerHealth = entityStats.health.CurrentValue;

            entityStats.TakeDamage(fakeAttacker, damage);

            Assert.IsTrue(damage > oldPlayerHealth);
            Assert.IsTrue(entityStats.isDead);
        }

        [Test]
        public void EntityStatsController_TakeDamage_ShouldNotReduceHealthIfPlayerIsDead()
        {
            float damage = 200f;
            EntityStatsController fakeAttacker = new GameObject().AddComponent<EntityStatsController>();
            entityStats.TakeDamage(fakeAttacker, damage);

            float oldPlayerHealth = entityStats.health.CurrentValue;
            entityStats.TakeDamage(fakeAttacker, damage);
            float newPlayerHealth = entityStats.health.CurrentValue;

            Assert.IsTrue(entityStats.isDead);
            Assert.AreEqual(oldPlayerHealth, newPlayerHealth);
        }
    }
}