using UnityEngine;

public class PlayerUIControlSpawn : MonoBehaviour
{
    private void Start()
    {
        int numPlayers = PlayerManager.Instance.NumPlayers;

        // Instantiate and place player characters evenly around spawn point
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject playerUIControl = PlayerManager.Instance.InstantiatePlayerUIControl(i);
        }
    }
}