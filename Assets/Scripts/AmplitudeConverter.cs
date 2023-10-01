using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplitudeConverter
{
    string[] allBinaryStrings = new string[64];

    public AmplitudeConverter()
    {
        for (int i = 0; i < 64; i++)
        {
            allBinaryStrings[i] = Convert.ToString(i, 2).PadLeft(6, '0');
        }
    }

    public float BitAlpha(Qiskit.ComplexNumber[] amplitudes, int bitIndex)
    {
        float sum = 0;
        for (int i = 0; i < 64; i++)
        {
            if (allBinaryStrings[i][bitIndex] != '1') continue;

            sum += (float)(amplitudes[i].Real * amplitudes[i].Real + amplitudes[i].Complex * amplitudes[i].Complex);
        }
        return Mathf.Sqrt(sum);
    }
}
