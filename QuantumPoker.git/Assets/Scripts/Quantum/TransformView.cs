using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransformView : MonoBehaviour
{
    public TMP_Text remainingGates;
    public PersistentState state;

    public Image cardSymbolDisplay;

    public Sprite leftCardSymbol;
    public Sprite rightCardSymbol;

    public GameObject cardHelp;

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

    public void SetCardSymbol(int modfiedCard)
    {
        cardSymbolDisplay.sprite = (modfiedCard == 0) ? leftCardSymbol : rightCardSymbol;
    }

    public void DisplayHelp()
    {
        cardHelp.SetActive(true);
    }

    public void CloseHelp()
    {
        cardHelp.SetActive(false);
    }
}
