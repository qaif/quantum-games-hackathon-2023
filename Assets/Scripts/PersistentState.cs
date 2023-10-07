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
    public int moneyToSpend = 0;

    public int gatesLimit = 2;

    public GateCount[] gates;
}
