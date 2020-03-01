using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public float radius;
    
    private void Start()
    {
        Vector3 spawnPos = transform.position;
        int numPlayers = PlayerManager.Instance.numPlayers;

        PlayerManager.Instance.InitializePlayers();

        // Instantiate and place player characters evenly around spawn point
        for (int i = 0; i < numPlayers; i++)
        {
            Vector3 offset = new Vector3(Mathf.Cos((2*i*Mathf.PI) / numPlayers), 0f, Mathf.Sin((2*i*Mathf.PI) / numPlayers));
            GameObject player = PlayerManager.Instance.InstantiatePlayer(i);
            player.transform.position = new Vector3(spawnPos.x, 0f, spawnPos.z) + radius * offset;
            PlayerManager.Instance.RegisterPlayer(player);
        }
    }
}