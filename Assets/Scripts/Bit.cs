using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bit : MonoBehaviour
{
    public TMP_Text alphaDisplay;
    public TMP_Text betaDisplay;

    string RepresentComplexNumber(Qiskit.ComplexNumber number)
    {
        string repr = number.Real.ToString("n2");
        if (number.Complex != 0)
        {
            repr += String.Format("+ {}i", number.Complex.ToString("n2"));
        }
        return repr;
    }

    public void SetAmplitudes(float alpha, float beta)
    {
        alphaDisplay.text = alpha.ToString("n2");
        betaDisplay.text = beta.ToString("n2");
    }
}
