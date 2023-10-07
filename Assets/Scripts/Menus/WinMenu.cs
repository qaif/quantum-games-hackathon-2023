using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public Table table;

    public void BuyGate(int gateTypeIndex)
    {
        // TODO: gates buying
    }

    public void Continue()
    {
        this.gameObject.SetActive(false);
        table.GoToNextLevel();
    }
}
