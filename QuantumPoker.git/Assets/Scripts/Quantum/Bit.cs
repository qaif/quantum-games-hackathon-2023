using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bit : MonoBehaviour
{
    public TMP_Text alphaDisplay;
    public TMP_Text betaDisplay;

    bool CloseTo(double var, double reference)
    {
        return Math.Abs(var - reference) < 0.01;
    }

    string RepresentComplexNumber(Qiskit.ComplexNumber number)
    {
        string repr = "";
        if (CloseTo(number.Real, 0) && CloseTo(number.Complex, 0))
        {
            return "0";
        }

        if (!CloseTo(number.Real, 0))
        {
            repr += number.Real.ToString("0.##");
        }

        if (!CloseTo(number.Complex, 0))
        {
            if (CloseTo(Math.Abs(number.Complex), 1))
            {
                repr += (number.Complex < 0) ? "-i" : "+i";
            }
            else
            {
                repr += String.Format("{0}{1}i", (number.Complex < 0) ? "-" : "+", Math.Abs(number.Complex).ToString("0.##"));
            }
        }

        return repr;
    }

    public void SetAmplitudes(Qiskit.ComplexNumber alpha, Qiskit.ComplexNumber beta)
    {
        alphaDisplay.text = RepresentComplexNumber(alpha);
        betaDisplay.text = RepresentComplexNumber(beta);
    }

    public void SetUnknownAmplitudes()
    {
        alphaDisplay.text = "?";
        betaDisplay.text = "?";
    }
}
