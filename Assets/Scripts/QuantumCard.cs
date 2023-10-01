using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumCard : MonoBehaviour
{
    public int numberOfBits;
    public Bit[] bits;

    protected AmplitudeConverter amplitudeConverter = new AmplitudeConverter();

    protected Qiskit.QuantumCircuit qc;
    protected Qiskit.MicroQiskitSimulator simulator = new Qiskit.MicroQiskitSimulator();

    protected void InitCircuit()
    {
        qc = new Qiskit.QuantumCircuit(numberOfBits, numberOfBits, true);
        for (int i = 0; i < numberOfBits; i++)
        {
            qc.H(i);
        }
    }

    void Start()
    {
        numberOfBits = bits.Length;
        InitCircuit();
        UpdateCard();
    }

    protected virtual void UpdateCard()
    {
        Qiskit.ComplexNumber[] amplitudes = simulator.Simulate(qc);

        for (int i = 0; i < numberOfBits; i++)
        {
            var alpha = amplitudeConverter.BitAlpha(amplitudes, i);
            var beta = Mathf.Sqrt(1 - alpha * alpha);

            bits[i].SetAmplitudes(alpha, beta);
        }
    }
}
