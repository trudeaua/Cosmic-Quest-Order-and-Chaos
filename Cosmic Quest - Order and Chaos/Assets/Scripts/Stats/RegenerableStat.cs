using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegenerableStat
{
    public float maxValue;
    public float minValue;
    [SerializeField] private float regenAmount = 0f;
    [SerializeField] private float regenPeriod = 0f;

    public delegate void OnValueChanged(float currentValue);
    public OnValueChanged onCurrentValueChanged;
    
    public float CurrentValue { get; private set; }
    
    private float _regenTimer = 0f;

    public void Init()
    {
        // Initialize current value to the maximum value
        CurrentValue = maxValue;
    }

    public void Add(float amount)
    {
        CurrentValue += amount;
        if (CurrentValue > maxValue)
        {
            CurrentValue = maxValue;
        }
        else
        {
            onCurrentValueChanged?.Invoke(CurrentValue);
        }
    }
    
    public void Subtract(float amount)
    {
        CurrentValue -= amount;
        if (CurrentValue < minValue)
        {
            CurrentValue = minValue;
        }
        else
        {
            onCurrentValueChanged?.Invoke(CurrentValue);
        }
    }

    public void Regen()
    {
        if (Mathf.Approximately(CurrentValue, maxValue))
            return;

        if (_regenTimer > 0f)
        {
            _regenTimer -= Time.deltaTime;
            return;
        }

        Add(regenAmount);
        _regenTimer = regenPeriod;
    }
}
