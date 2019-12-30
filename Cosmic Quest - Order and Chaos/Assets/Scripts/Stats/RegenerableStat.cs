using UnityEngine;

[System.Serializable]
public class RegenerableStat
{
    public float maxValue;
    public float minValue;
    public float regenAmount;
    public float CurrentValue { get; private set; }
    
    public delegate void OnValueChanged(float currentValue);
    public OnValueChanged onCurrentValueChanged;
    
    private bool _pauseRegen = false;
    private float _pauseTimer = 0f;

    /// <summary>
    /// Initialize the stat
    /// </summary>
    public void Init()
    {
        // Initialize current value to the maximum value
        CurrentValue = maxValue;
        _pauseRegen = false;
        _pauseTimer = 0f;
    }

    /// <summary>
    /// Add an amount to the stat
    /// </summary>
    /// <param name="amount">Amount to add to the stat</param>
    public void Add(float amount)
    {
        CurrentValue += amount;
        if (CurrentValue > maxValue)
        {
            CurrentValue = maxValue;
        }
        
        onCurrentValueChanged?.Invoke(CurrentValue);
    }
    
    /// <summary>
    /// Subtract an amount from the stat
    /// </summary>
    /// <param name="amount">Amount to subtract from the stat</param>
    /// <param name="delayRegen">Number of seconds to wait to regenerate the stat</param>
    public void Subtract(float amount, float delayRegen = 0f)
    {
        CurrentValue -= amount;
        if (CurrentValue < minValue)
        {
            CurrentValue = minValue;
        }
        
        onCurrentValueChanged?.Invoke(CurrentValue);

        if (delayRegen > 0f)
            _pauseTimer += delayRegen;
    }

    /// <summary>
    /// Pause stat regeneration
    /// </summary>
    public void PauseRegen()
    {
        _pauseRegen = true;
    }

    /// <summary>
    /// Start stat regeneration
    /// </summary>
    public void StartRegen()
    {
        _pauseRegen = false;
    }

    /// <summary>
    /// Regenerate the stat by `regenAmount`
    /// This function should be called inside an Update loop.
    /// </summary>
    public void Regen()
    {
        if (_pauseRegen || Mathf.Approximately(CurrentValue, maxValue))
            return;

        if (_pauseTimer > 0f)
        {
            _pauseTimer -= Time.deltaTime;
            return;
        }

        Add(regenAmount * Time.deltaTime);
    }
}
