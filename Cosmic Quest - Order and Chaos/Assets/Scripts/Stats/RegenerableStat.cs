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

    public void Init()
    {
        // Initialize current value to the maximum value
        CurrentValue = maxValue;
        _pauseRegen = false;
        _pauseTimer = 0f;
    }

    public void Add(float amount)
    {
        CurrentValue += amount;
        if (CurrentValue > maxValue)
        {
            CurrentValue = maxValue;
        }
        
        onCurrentValueChanged?.Invoke(CurrentValue);
    }
    
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

    public void PauseRegen()
    {
        _pauseRegen = true;
    }

    public void StartRegen()
    {
        _pauseRegen = false;
    }

    // This function should be called inside an Update loop
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
