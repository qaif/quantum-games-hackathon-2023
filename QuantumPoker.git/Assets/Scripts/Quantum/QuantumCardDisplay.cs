using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumCardDisplay : MonoBehaviour
{
    public Bit[] bits;

    public void UpdateCard(QuantumCard card, bool explicitValues)
    {
        for (int i = 0; i < 6; i++)
        {
            if (explicitValues)
            {
                Qiskit.ComplexNumber[] amplitudes = card.Simulate(i);

                var alpha = amplitudes[0];
                var beta = amplitudes[1];

                bits[i].SetAmplitudes(alpha, beta);
            }
            else
            {
                bits[i].SetUnknownAmplitudes();
            }
        }
    }
}
