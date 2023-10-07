using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransformView : MonoBehaviour
{
    public TMP_Text remainingGates;
    public PersistentState state;

    public void UpdateRemainingGates(int usedGates)
    {
        var gatesToUse = state.gatesLimit - usedGates;
        remainingGates.text = gatesToUse.ToString();

        if (gatesToUse <= 0)
        {
            ToggleUnusedGates(false);
        }
        else
        {
            ToggleUnusedGates(true);
        }
    }

    void ToggleUnusedGates(bool value)
    {
        var gates = FindObjectsOfType<DraggableGate>();

        foreach (var gate in gates)
        {
            if (gate.FindOverlapingLine() == -1)
            {
                gate.interactable = value;
            }
        }
    }

}
