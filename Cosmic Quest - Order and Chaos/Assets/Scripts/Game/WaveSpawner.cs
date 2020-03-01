using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyProbability {
    public GameObject enemyPrefab;
    public float probability;
}

public class WaveSpawner : MonoBehaviour
{
    [Tooltip("Indicates whether the wave spawner should automatically spawn waves or not")]
    [SerializeField] protected bool spawnWaves = true;

    [Tooltip("List of all enemies available to spawn along with a probability of spawning")]
    [SerializeField] protected EnemyProbability[] enemyProbabilities;
    [Tooltip("Roughly the number of seconds between individual enemy spawns")]
    [SerializeField] protected float spawnDelay = 0.1f;
    protected static float SpawnDelay;
    [Tooltip("Lower bound on time (in seconds) between waves")]
    [SerializeField] protected int waveIntervalStart = 60;
    [Tooltip("Upper bound on time (in seconds) between waves")]
    [SerializeField] protected int waveIntervalEnd = 120;

    protected static List<EnemyProbability> cumulativeEnemyProbabilities;
    protected static float totalProbability = 0;
    protected static Room[] rooms;
    protected float timeSinceLastWave = 0;
    protected int waveCooldown = 20;

    protected void Awake()
    {
        SpawnDelay = spawnDelay;
        // get cumulative distribution for spawning an enemy
        cumulativeEnemyProbabilities = new List<EnemyProbability>();
        foreach (EnemyProbability enemyProbability in enemyProbabilities)
        {
            totalProbability += enemyProbability.probability;
            var el = enemyProbability;
            el.probability = totalProbability;
            cumulativeEnemyProbabilities.Add(enemyProbability);
        }
        rooms = FindObjectsOfType<Room>();
    }

    protected void Start()
    {
    }

    protected void Update()
    {
        if (timeSinceLastWave > waveCooldown && spawnWaves)
        {
            StartCoroutine(SpawnWave(GetCurrentRoom()));
            timeSinceLastWave = 0;
            waveCooldown = Random.Range(waveIntervalStart, waveIntervalEnd);
        }
        timeSinceLastWave += Time.deltaTime;
    }

    /// <summary>
    /// Enables automatic wave spawning
    /// </summary>
    public void EnableWaveSpawning()
    {
        spawnWaves = true;
    }

    /// <summary>
    /// Disables automatic wave spawning
    /// </summary>
    public void DisableWaveSpawning()
    {
        spawnWaves = false;
    }

    /// <summary>
    /// Spawn a wave of enemies in a certain room
    /// </summary>
    /// <param name="room">Room in which to spawn enemies</param>
    /// <param name="maxRadius">Maximum radius that enemies will spawn from the players</param>
    /// <param name="numEnemies">Number of enemies to spawn, set to 0 to auto calculate the number of enemies to spawn</param>
    /// <param name="delay">Number of seconds to wait before spawning the wave</param>
    /// <returns>An IEnumerator</returns>
    public static IEnumerator SpawnWave(Room room, float maxRadius = 8f, int numEnemies = 0, float delay = 0)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        if (numEnemies == 0)
            numEnemies = (int)Mathf.Max(Mathf.Pow(PlayerManager.Instance.Players.Count, 2) * Random.Range(0.75f, 1.25f), 1);
        Vector3 playerAvgPosition = new Vector3(0, 0, 0);
        foreach(GameObject player in PlayerManager.Instance.Players)
        {
            playerAvgPosition += player.transform.position;
        }
        playerAvgPosition = playerAvgPosition / PlayerManager.Instance.Players.Count;
        for (int i = 0; i < numEnemies; i++)
        {
            float rand = Random.Range(0, totalProbability);
            GameObject randomEnemy = GetRandomEnemy(rand);
            GameObject enemy = Instantiate(randomEnemy);
            enemy.name = randomEnemy.name;
            enemy.transform.parent = room.transform.parent;
            //enemy.transform.position = playerAvgPosition * Random.Range(0.9f, 1.1f);
            Vector3 pt;
            RandomPoint(playerAvgPosition, maxRadius, out pt);
            enemy.transform.position = pt;
            yield return new WaitForSeconds(SpawnDelay * Random.Range(0.8f, 2f));
        }
    }

    /// <summary>
    /// Spawn a wave of enemies at a certain position
    /// </summary>
    /// <param name="position">Position to spawn the wave from</param>
    /// <param name="maxRadius">Maximum radius that enemies will spawn from the players</param>
    /// <param name="numEnemies">Number of enemies to spawn, set to 0 to auto calculate the number of enemies to spawn</param>
    /// <param name="delay">Number of seconds to wait before spawning the wave</param>
    /// <returns>An IEnumerator</returns>
    public static IEnumerator SpawnWave(Vector3 position, float maxRadius = 8f, int numEnemies = 0, float delay = 0)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        if (numEnemies == 0)
            numEnemies = (int)Mathf.Max(Mathf.Pow(PlayerManager.Instance.Players.Count, 2) * Random.Range(0.75f, 1.25f), 1);
        for (int i = 0; i < numEnemies; i++)
        {
            float rand = Random.Range(0, totalProbability);
            GameObject randomEnemy = GetRandomEnemy(rand);
            GameObject enemy = Instantiate(randomEnemy);
            enemy.name = randomEnemy.name;
            Vector3 pt;
            RandomPoint(position, maxRadius, out pt);
            enemy.transform.position = pt;
            yield return new WaitForSeconds(SpawnDelay * Random.Range(0.8f, 2f));
        }
    }

    /// <summary>
    /// Randomly get an enemy prefab using cumulative distribution
    /// </summary>
    /// <param name="seed">Input to the cumulative distribution<br><b>0 < seed < CumulativeProbability</b></br></param>
    /// <returns>An enemy prefab</returns>
    private static GameObject GetRandomEnemy(float seed)
    {
        float prevProbability = 0;
        GameObject obj = null;
        foreach (EnemyProbability enemyProbability in cumulativeEnemyProbabilities)
        {
            if (prevProbability <= seed && seed <= enemyProbability.probability)
            {
                obj = enemyProbability.enemyPrefab;
            }
            prevProbability = enemyProbability.probability;
        }
        return obj;
    }

    /// <summary>
    /// Get the room that the players are currently in
    /// </summary>
    /// <returns>The Room object closest to the average position of the players</returns>
    private static Room GetCurrentRoom()
    {
        // players' average position
        Vector3 avgPlayerPostion = new Vector3(0, 0, 0);
        foreach (GameObject player in PlayerManager.Instance.Players)
        {
            avgPlayerPostion += player.transform.position;
        }
        if (rooms.Length == 0)
            return null;
        Room closestRoom = rooms[0];
        for (int i = 1; i < rooms.Length; i++)
        {
            if (Vector3.Distance(rooms[i].transform.position, avgPlayerPostion) < Vector3.Distance(closestRoom.transform.position, avgPlayerPostion))
            {
                closestRoom = rooms[i];
            }
        }
        return closestRoom;
    }

    /// <summary>
    /// Select a random point on a NavMesh around a radius
    /// </summary>
    /// <param name="center">Center of the unit circle</param>
    /// <param name="range">Radius of the unit circle</param>
    /// <returns>A bool representing if a point was found or not</returns>
    private static bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Loop through until a point on the navmesh is found
        // https://docs.unity3d.com/540/Documentation/ScriptReference/NavMesh.SamplePosition.html
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = center;
        return false;
    }
}
