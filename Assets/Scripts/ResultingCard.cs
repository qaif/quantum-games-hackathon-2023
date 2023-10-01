using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ResultingCard : QuantumCard
{
    public Events events;

    void OnEnable()
    {
        events.transformViewUpdated += UpdateTransformView;
    }

    void OnDisable()
    {
        events.transformViewUpdated -= UpdateTransformView;
    }

    void UpdateTransformView()
    {
        InitCircuit();

        List<DraggableGate> gates = FindObjectsOfType<DraggableGate>().OrderBy(gate => gate.transform.position.x).ToList();

        foreach (var gate in gates)
        {
            int i = gate.FindOverlapingLine();
            if (i == -1) continue;

            int qubitIndex = 5 - i;

            switch (gate.type)
            {
                case GateType.H:
                    this.qc.H(qubitIndex);
                    break;
                case GateType.X:
                    this.qc.X(qubitIndex);
                    break;
                case GateType.Y:
                    this.qc.Y(qubitIndex);
                    break;
                case GateType.Z:
                    this.qc.Z(qubitIndex);
                    break;
                case GateType.RY:
                    float randomAngle = 2 * math.PI * UnityEngine.Random.value;
                    this.qc.RY(qubitIndex, randomAngle);
                    break;
            }
        }

        UpdateCard();
    }
}
