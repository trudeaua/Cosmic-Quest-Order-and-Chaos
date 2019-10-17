using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Transform player;
    
    [Header("Internal UI References")]
    public Image healthBar;
    public Text healthNumber;
    public Text playerName;

    private PlayerStats stats;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO This is temporary!!!
        stats = player.GetComponent<PlayerStats>();
        playerName.text = player.name;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = stats.currentHealth / stats.maxHealth;
        healthNumber.text = stats.currentHealth.ToString();
    }
}
