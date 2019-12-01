using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Image healthBar;
    public Image manaBar;
    
    private RegenerableStat _healthStat;
    private RegenerableStat _manaStat;

    private void Awake()
    {
        PlayerStatsController stats = transform.root.GetComponent<PlayerStatsController>();
        _healthStat = stats.health as RegenerableStat;
        _manaStat = stats.mana;
    }

    private void Start()
    {
        _healthStat.onCurrentValueChanged += UpdateHealthValue;
        _manaStat.onCurrentValueChanged += UpdateManaValue;
    }

    private void UpdateHealthValue(float value)
    {
        healthBar.fillAmount = value / _healthStat.maxValue;
    }
    
    private void UpdateManaValue(float value)
    {
        manaBar.fillAmount = value / _manaStat.maxValue;
    }
}
