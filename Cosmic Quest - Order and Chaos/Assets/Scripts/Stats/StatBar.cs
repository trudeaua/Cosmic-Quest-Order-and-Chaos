using System;
using UnityEngine;
using UnityEngine.UI;

public enum BarType {
    Player,
    Enemy
}

public class StatBar : MonoBehaviour
{
    public bool alwaysShow;
    [Tooltip("The amount of time after receiving damage to hide the bars (ignored if \"alwaysShow\" is set true)")]
    public float timeout = 3f;
    public Image healthBar;
    public Image manaBar;
    public BarType barType;
    
    private RegenerableStat _healthStat;
    private RegenerableStat _manaStat;
    private Canvas _canvas;
    private float _timer;

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

        _canvas = GetComponent<Canvas>();
        
        // Start the bars hidden if not set to always show them
        if (!alwaysShow)
            _canvas.enabled = false;
    }

    private void Start()
    {
        _healthStat.onCurrentValueChanged += UpdateHealthValue;
        if (barType == BarType.Player)
        {
            _manaStat.onCurrentValueChanged += UpdateManaValue;
        }
    }

    private void Update()
    {
        if (alwaysShow || _timer <= 0f)
            return;
        
        _timer -= Time.deltaTime;

        // Hide the bars
        if (_timer <= 0f)
        {
            _timer = 0;
            _canvas.enabled = false;
        }
    }

    /// <summary>
    /// Update the fill amount of the health bar
    /// </summary>
    /// <param name="value">Current health value</param>
    private void UpdateHealthValue(float value)
    {
        float prevAmount = healthBar.fillAmount;
        healthBar.fillAmount = value / _healthStat.maxValue;
        
        // If damage taken, make sure the health bar is shown
        if (!alwaysShow && healthBar.fillAmount < prevAmount)
        {
            _timer = timeout;
            if (!_canvas.enabled)
                _canvas.enabled = true;
        }
    }
    
    /// <summary>
    /// Update the fill amount of the mana bar
    /// </summary>
    /// <param name="value">Current mana value</param>
    private void UpdateManaValue(float value)
    {
        manaBar.fillAmount = value / _manaStat.maxValue;
    }
}
