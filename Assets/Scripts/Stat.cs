using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public List<float> modifiers;

    
    public float GetValue()
    {
        return baseValue + modifiers.Sum();
    }

    public void SetDefaultValue(float value)
    {
        if (baseValue == 0)
            baseValue = value;
    }
    
    public void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier) // need test "int"
    {
        modifiers.RemoveAt(modifier);
    }
}
