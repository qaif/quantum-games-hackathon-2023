using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int basevalue;
    private List<int> modifiers = new List<int>();
    public int GetValue()
    {
        int finalValue = basevalue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }
    public void AddModifier(int modifier)
    {
        if(modifier!=0)
        {
            modifiers.Add(modifier);
        }
    }
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}
