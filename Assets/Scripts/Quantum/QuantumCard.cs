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

    public void Apply(GateType type, int i)
    {
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
                float randomAngle = 2 * Mathf.PI * UnityEngine.Random.value;
                this.qcs[i].RY(0, randomAngle);
                break;
        }
    }

}
