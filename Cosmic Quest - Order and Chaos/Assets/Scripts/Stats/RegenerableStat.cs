using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegenerableStat
{
    public float maxValue;
    public float minValue;
    public float currentValue { get; private set; }
    [SerializeField] private float regenAmount = 0f;
    [SerializeField] private float regenRate = 0f;
    private float _lastRegenTime;

    public void Init()
    {
        // Initialize current value to the maximum value
        currentValue = maxValue;
    }

    public void Add(float amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }
    
    public void Subtract(float amount)
    {
        currentValue -= amount;
        if (currentValue < minValue)
        {
            currentValue = minValue;
        }
    }

    public void Regen()
    {
        if (Mathf.Approximately(currentValue, maxValue) || Time.time - _lastRegenTime < regenRate)
            return;

        _lastRegenTime = Time.time;
        currentValue += regenAmount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }
}
