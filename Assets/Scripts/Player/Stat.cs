using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    //[SerializeField] private string statName;
    [SerializeField] private int currentValue;
    [SerializeField] private int maxValue;
    private List<int> modifiers = new List<int>();

    /*public string GetStatName()
    {
        return statName;
    }*/

    public int GetCurrentValue()
    {
        int finalVal = currentValue;
        finalVal += GetModifierValue();
        return finalVal;
    }

    public int GetMaxValue()
    {
        int currentMax = maxValue;
        currentMax += GetModifierValue();
        return currentMax;
    }

    public void IncreaseStat(int amnt)
    {
        currentValue += amnt;
    }

    public void DecreaseStat(int amnt)
    {
        currentValue -= amnt;
    }

    public int GetModifierValue()
    {
        int finalVal = 0;
        modifiers.ForEach(x => finalVal += x);
        return finalVal;
    }

    public void AddModifier(int newMod)
    {
        if (newMod != 0)
        {
            modifiers.Add(newMod);
        }
    }

    public void RemoveModifier(int newMod)
    {
        if (newMod != 0)
        {
            modifiers.Remove(newMod);
        }
    }

    public void SetToMax()
    {
        currentValue = maxValue;
    }

    public void SetToZero()
    {
        currentValue = 0;
    }

    public void ResetStat()
    {
        currentValue = maxValue;
        modifiers = new List<int>();
    }


}
