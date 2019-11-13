using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int value = baseValue;
        modifiers.ForEach(mod => value += mod);
        return value;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
    }
}
