using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier
{
    public Guid ID { get; private set; } = Guid.NewGuid();
    public int priority = 1;

    public abstract int ApplyModifier(int value);
}

public class LumpSumModifier : Modifier
{
    private int amount;

    public LumpSumModifier(int amount)
    {
        this.amount = amount;
    }

    public override int ApplyModifier(int value)
    {
        return value + amount;
    }
}

public class PercentageModifier : Modifier
{
    private int perc;

    public PercentageModifier(int perc)
    {
        this.perc = perc;
    }

    public override int ApplyModifier(int value)
    {
        return value * perc;
    }
}

public class ModifiableValue
{
    private int baseValue;
    private List<Modifier> modifiers = new List<Modifier>();

    public int CurrentValue
    {
        get
        {
            int f = baseValue;

            foreach(var m in modifiers)
            {
                f = m.ApplyModifier(f);
            }

            return f;
        }
    }

    public ModifiableValue(int baseValue = 0)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(Guid id)
    {
        modifiers.RemoveAll(x => x.ID == id);
    }

    public void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
    }

    public void Reset()
    {
        modifiers.Clear();
    }

    public override string ToString()
    {
        return string.Format("Current: {0}; Base: {1}; ModCount: {2}", CurrentValue, baseValue, modifiers.Count);
    }
}
