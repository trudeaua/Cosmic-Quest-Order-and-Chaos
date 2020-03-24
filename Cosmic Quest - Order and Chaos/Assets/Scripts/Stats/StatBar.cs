using UnityEngine;
using UnityEngine.UI;

public enum BarType {
    Player,
    Enemy
}

public class StatBar : MonoBehaviour
{
    public Image healthBar;
    public Image manaBar;
    public BarType barType;
    
    private RegenerableStat _healthStat;
    private RegenerableStat _manaStat;

    private void Awake()
    {
        if (barType == BarType.Player)
        {
            PlayerStatsController stats = transform.parent.GetComponent<PlayerStatsController>();
            _healthStat = stats.health;
            _manaStat = stats.mana;
        }
        else if (barType == BarType.Enemy)
        {
            EnemyStatsController stats = transform.parent.GetComponent<EnemyStatsController>();
            _healthStat = stats.health;
            
            // apply enemy name as label
            string label = gameObject.GetComponentInParent<EntityStatsController>().gameObject.name;
            Text labelText = gameObject.GetComponentInChildren<Text>();
            labelText.text = label;
        }
    }

    private void Start()
    {
        _healthStat.onCurrentValueChanged += UpdateHealthValue;
        if (barType == BarType.Player)
        {
            _manaStat.onCurrentValueChanged += UpdateManaValue;
        }
    }

    /// <summary>
    /// Update the fill amount of the health bar
    /// </summary>
    /// <param name="value">Current health value</param>
    private void UpdateHealthValue(float value)
    {
        healthBar.fillAmount = value / _healthStat.maxValue;
    }
    
    /// <summary>
    /// Update the fill amount of the mana bar
    /// </summary>
    /// <param name="value">Current mana value</param>
    private void UpdateManaValue(float value)
    {
        manaBar.fillAmount = value / _manaStat.maxValue;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
