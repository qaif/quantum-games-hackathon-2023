using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumCard
{
    Qiskit.QuantumCircuit[] qcs;
    Qiskit.MicroQiskitSimulator simulator = new Qiskit.MicroQiskitSimulator();

    public QuantumCard()
    {
        InitCircuit();
    }

    public Qiskit.ComplexNumber[] Simulate(int bitIndex)
    {
        return simulator.Simulate(qcs[bitIndex]);
    }

    public double[] SimulateProbability(int bitIndex)
    {
        return simulator.GetProbabilities(qcs[bitIndex]);
    }

    public void InitCircuit()
    {
        qcs = new Qiskit.QuantumCircuit[6];

        for (int i = 0; i < 6; i++)
        {
            qcs[i] = new Qiskit.QuantumCircuit(1, 1, false);
            qcs[i].H(0);
        }
    }

    public void Apply(GateType type, int i, float theta)
    {
        float randomAngle = theta * Mathf.PI / 180;
        switch (type)
        {
            case GateType.H:
                this.qcs[i].H(0);
                break;
            case GateType.X:
                this.qcs[i].X(0);
                break;
            case GateType.Y:
                this.qcs[i].Y(0);
                break;
            case GateType.Z:
                this.qcs[i].Z(0);
                break;
            case GateType.RY:
                this.qcs[i].RY(0, randomAngle);
                break;
            case GateType.RX:
                this.qcs[i].RX(0, randomAngle);
                break;
            case GateType.S:
                this.qcs[i].RZ(0, Mathf.PI / 2);
                break;
            case GateType.T:
                this.qcs[i].RZ(0, Mathf.PI / 4);
                break;
        }
    }

}
