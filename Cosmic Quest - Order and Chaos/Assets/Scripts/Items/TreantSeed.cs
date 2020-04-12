using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSeed : DamageProjectile
{
    [Range(0, 1)] public float spawnPlantProbability = 0.1f;
    public List<GameObject> plantPrefabs;
    public int maxIndex = 1;

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        // if collision is with a player then deal damage
        if (col.CompareTag("Player"))
        {
            EntityStatsController target = col.GetComponent<EntityStatsController>();
            target.StartCoroutine(other.transform.GetComponent<PlayerMotorController>().ApplyTimedMovementModifier(0.5f, 1.5f));
            target.TakeDamage(Damage);
            gameObject.SetActive(false);
        }
        else if (col.CompareTag("Ground"))
        {
            // Possibly spawn a random plant
            if (Random.Range(0f, 1f) < spawnPlantProbability)
            {
                int maxIndex = 1;
                float healthPercentage = LauncherStats.health.CurrentValue/LauncherStats.health.maxValue;
                if (healthPercentage < 0.35f)
                    maxIndex = 4;
                else if (healthPercentage < 0.7f)
                    maxIndex = 3;
                TreantBossCombatController treatCombat = LauncherStats.GetComponent<TreantBossCombatController>();
                if (treatCombat && treatCombat.CanSpawnNewPlant())
                {
                    GameObject plantPrefab = plantPrefabs[Random.Range(0, maxIndex)];
                    GameObject plant = Instantiate(plantPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
                    treatCombat.AddPlantToSpawnList(plant);
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}
