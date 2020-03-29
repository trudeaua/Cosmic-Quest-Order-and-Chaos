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
    public string[] reviveTextList;

    private string reviveText;
    private float respawnPercentage = 0f;
    private List<Collider> playerColliders = new List<Collider>();

    void Start()
    {
        reviveText = reviveTextList[Random.Range(0, reviveTextList.Length - 1)];
        UpdateRespawnPercentage(playerColliders.Count > 0);
    }

    private void Update()
    {
        UpdateRespawnPercentage(playerColliders.Count > 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerColliders.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerColliders.Remove(other);
        }
    }

    private void UpdateRespawnPercentage(bool playersOnBeacon)
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
                text.text = reviveText;
                return;
            }
        }
        text.text = respawnPercentage.ToString("N0") + "%";
    }
}
