using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GateCount
{
    public GateType type;
    public int count;
}

public enum Mode
{
    Educational,
    Challenging,
    Cheat
}

[CreateAssetMenu]
public class PersistentState : ScriptableObject
{
    public int initialMoneyToSpend = 0;
    public int initialGatesLimit = 2;
    public List<GateCount> initialGates;

    [HideInInspector] public int moneyToSpend;
    [HideInInspector] public int gatesLimit;
    [HideInInspector] public List<GateCount> gates;

    [HideInInspector] public Mode mode = Mode.Educational;

    internal void Reset()
    {
        moneyToSpend = initialMoneyToSpend;
        gatesLimit = initialGatesLimit;
        gates = new List<GateCount>();
        foreach (var gc in initialGates)
        {
            gates.Add(new GateCount() { type = gc.type, count = gc.count });
        }
    }
}
