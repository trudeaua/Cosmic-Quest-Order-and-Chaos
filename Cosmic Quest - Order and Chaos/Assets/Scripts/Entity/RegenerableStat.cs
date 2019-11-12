using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegenerableStat
{
    public int maxValue { get; }
    public int minValue { get; }
    public int currentValue { get; private set; }
    [SerializeField] private int regenAmount;
    [SerializeField] private float regenRate;
    private float _lastRegenTime;

    public RegenerableStat(int maxValue, int minValue, int regenAmount, float regenRate)
    {
        this.maxValue = maxValue;
        this.minValue = minValue;
        this.regenAmount = regenAmount;
        this.regenRate = regenRate;

        // Initialize current value to the maximum value
        currentValue = maxValue;
    }

    public void Subtract(int amount)
    {
        currentValue -= amount;
        if (currentValue < minValue)
        {
            currentValue = minValue;
        }
    }

    public void Regen()
    {
        if (currentValue == maxValue || Time.time - _lastRegenTime < regenRate)
            return;

        _lastRegenTime = Time.time;
        currentValue += regenAmount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }
}
