using UnityEngine;

public class ColourCollider : MonoBehaviour
{
    public CharacterColour colour;

    private void Start()
    {
        foreach (GameObject player in PlayerManager.Instance.Players)
        {
            // Ignore collisions with any players who match the set colour
            if (player.GetComponent<PlayerStatsController>().characterColour == colour)
            {
                Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }
}
