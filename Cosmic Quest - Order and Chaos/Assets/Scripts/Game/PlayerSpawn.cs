using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Events;

public class PlayerSpawn : MonoBehaviour
{
    public float radius;
    public static UnityEvent onPlayersSpawned = new UnityEvent();
    
=======
using UnityEngine.Events;

public class PlayerSpawn : MonoBehaviour
{
    public float radius;
>>>>>>> 23058c66ae117a3642b7b3c0850286029b17584d
    private void Start()
    {
        Vector3 spawnPos = transform.position;
        int numPlayers = PlayerManager.Instance.NumPlayers;

        PlayerManager.Instance.InitializePlayers();

        // Instantiate and place player characters evenly around spawn point
        for (int i = 0; i < numPlayers; i++)
        {
            Vector3 offset = new Vector3(Mathf.Cos((2*i*Mathf.PI) / numPlayers), 0f, Mathf.Sin((2*i*Mathf.PI) / numPlayers));
            GameObject player = PlayerManager.Instance.InstantiatePlayer(i);
            player.transform.position = new Vector3(spawnPos.x, 0f, spawnPos.z) + radius * offset;
            PlayerManager.Instance.RegisterPlayer(player);
        }

        onPlayersSpawned.Invoke();
    }
}