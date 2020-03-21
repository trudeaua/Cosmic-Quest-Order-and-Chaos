using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RespawnBeacon : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float respawnRate = 0.5f;
    public PlayerStatsController playerStatsController;
    public GameObject respawnEffectPrefab;

    private float respawnPercentage = 0f;
    private bool activated = false;
    private List<Collider> playerColliders = new List<Collider>();

    private void Update()
    {
        if (playerColliders.Count <= 0) UpdateRespawnPercentage(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && activated)
        {
            UpdateRespawnPercentage();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
            playerColliders.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (activated && other.tag == "Player")
        {
            playerColliders.Remove(other);
            if (playerColliders.Count <= 0)
            {
                activated = false;
                UpdateRespawnPercentage(false);
            }
        }
    }

    private void UpdateRespawnPercentage(bool playersOnBeacon = true)
    {
        if (playersOnBeacon)
        {
            respawnPercentage += respawnRate;
            if (respawnPercentage >= 100f)
            {
                playerStatsController.Respawn();
                StartCoroutine(VfxHelper.CreateVFX(respawnEffectPrefab, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, PlayerManager.colours.GetColour(playerStatsController.characterColour)));
                Destroy(gameObject);
            }
        }
        else
        {
            respawnPercentage = Mathf.Max(0, respawnPercentage - 2f * respawnRate);

            if (respawnPercentage <= 0)
            {
                text.text = "Please help!";
                return;
            }
        }
        text.text = respawnPercentage.ToString("N0") + "%";
    }
}
