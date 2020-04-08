using System;
using UnityEngine;
using UnityEngine.UI;

public enum BarType {
    Player,
    Enemy,
    Boss
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
        switch (barType)
        {
            case BarType.Player:
            {
                PlayerStatsController stats = transform.parent.GetComponent<PlayerStatsController>();
                _healthStat = stats.health;
                _manaStat = stats.mana;
                break;
            }
            case BarType.Enemy:
            case BarType.Boss:
            {
                EnemyStatsController stats = transform.parent.GetComponent<EnemyStatsController>();
                _healthStat = stats.health;

                // Handle boss specific setup
                if (barType == BarType.Boss)
                {
                    // Use the boss' gameObject name for the label
                    string label = stats.gameObject.name;
                    Text labelText = gameObject.GetComponentInChildren<Text>();
                    labelText.text = label;
                    
                    // Only show the bar if in a boss fight state
                    /*GameManager.Instance.onGameStateChanged += state =>
                    {
                        if (state == GameManager.GameState.BossFight)
                            Show();
                        else
                            Hide();
                    };
                    
                    // Hide the bar if we're not starting in a boss fight state
                    if (GameManager.Instance.CurrentState != GameManager.GameState.BossFight)
                        Hide();*/
                }
                
                break;
            }
        }

        _canvas = GetComponent<Canvas>();
        
        // Start the bars hidden if not set to always show them
        if (!alwaysShow)
            Hide();
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
            Hide();
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
            Show();
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

    public void Hide()
    {
        if (_canvas.enabled)
            _canvas.enabled = false;
    }

    public void Show()
    {
        if (!_canvas.enabled)
            _canvas.enabled = true;
    }
}
