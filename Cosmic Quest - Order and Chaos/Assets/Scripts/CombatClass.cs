using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStyle
{
    Melee,
    Ranged,
    Magic
}

[System.Serializable]
public class CombatClass
{
    public string className;

    // Modifiers
    //public Stat[] statModifiers;
    public int baseDamageModifier;
    public int baseDefenseModifier;

    // Combat style configurations
    public CombatStyle combatStyle;

    // Melee combat style configurations TODO expand this
    public float attackRadius = 2f;

    // TODO Ranged combat style configurations


    // TODO Magic combat style configurations
}