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

[CreateAssetMenu]
public class PersistentState : ScriptableObject
{
    public int initialMoneyToSpend = 0;
    public int initialGatesLimit = 2;
    public List<GateCount> initialGates;

    public int moneyToSpend;
    public int gatesLimit;
    public List<GateCount> gates;

    internal void Reset()
    {
        moneyToSpend = initialMoneyToSpend;
        gatesLimit = initialGatesLimit;
        gates = new List<GateCount>(initialGates);
    }
}
