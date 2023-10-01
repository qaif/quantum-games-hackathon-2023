using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QiskitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var qc = new Qiskit.QuantumCircuit(2, 2, true);
        qc.H(0);
        qc.H(1);
        // qc.CX(0, 1);

        var simulator = new Qiskit.MicroQiskitSimulator();

        // var probabilities = simulator.GetProbabilities(qc);
        // foreach (var prob in probabilities)
        // {
        //     Debug.Log($"Probability: {prob}");
        // }

        var amps = simulator.Simulate(qc);
        foreach (var amp in amps)
        {
            Debug.Log($"Amplitude: {amp}");
        }

        // var counts = new Dictionary<int, int>();
        // var rs = new RandomSelector();
        // for (int i = 0; i < 1000; i++)
        // {
        //     var index = rs.GetRandomElementIndex(probabilities);
        //     if (!counts.ContainsKey(index)) counts[index] = 0;
        //     counts[index] += 1;
        // }

        // foreach (var key in counts.Keys)
        // {
        //     Debug.Log($"{key}: {counts[key]}");
        // }
    }
}
