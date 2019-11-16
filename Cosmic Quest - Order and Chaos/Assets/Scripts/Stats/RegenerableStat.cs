using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegenerableStat
{
    public float maxValue;
    public float minValue;
    public float CurrentValue { get; private set; }
    [SerializeField] private float regenAmount = 0f;
    [SerializeField] private float regenPeriod = 0f;
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
    }
    
    public void Subtract(float amount)
    {
        CurrentValue -= amount;
        if (CurrentValue < minValue)
        {
            CurrentValue = minValue;
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
