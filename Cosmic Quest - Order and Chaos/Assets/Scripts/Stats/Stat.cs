using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue = 0;

    private List<int> modifiers = new List<int>();
    public int BaseValue
    {
        get
        {
            return baseValue;
        }
        set
        {
            baseValue = value;
        }
    }

    /// <summary>
    /// Get the value of the stat, taking modifiers into account
    /// </summary>
    /// <returns>The modified value of the stat</returns>
    public int GetValue()
    {
        int value = baseValue;
        modifiers.ForEach(mod => value += mod);
        return value;
    }

    /// <summary>
    /// Get the base value of the stat
    /// </summary>
    /// <returns>The base value of the stat</returns>
    public int GetBaseValue()
    {
        return baseValue;
    }

    /// <summary>
    /// Add a modifier to the stat
    /// </summary>
    /// <param name="modifier">The modifer value to add</param>
    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
    }

    /// <summary>
    /// Remove a modifier from the stat
    /// </summary>
    /// <param name="modifier">The modifier to remove from the stat</param>
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
    }
}
