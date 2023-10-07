using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Table table;
    public PersistentState state;

    public GameObject purchasableGatePrefab;
    public GameObject gatesToBuyContainer;

    public TMP_Text moneyDisplay;

    public void BuyGate(GateToBuy gateToBuy)
    {
        state.moneyToSpend -= gateToBuy.price;

        var gatesList = state.gates.Where(gate => gate.type == gateToBuy.gateType);
        if (gatesList.Count() == 0)
        {
            state.gates.Add(new GateCount() { type = gateToBuy.gateType, count = 1 });
        }
        else
        {
            gatesList.First().count += 1;
        }

        UpdateMoney();

        DisableTooExpensiveGates();
        // TODO: sound effect
    }

    public void DisableTooExpensiveGates()
    {
        foreach (Transform child in gatesToBuyContainer.transform)
        {
            if (child.GetComponent<PurchasableGate>().gateToBuy.price > state.moneyToSpend)
            {
                child.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Init(GateToBuy[] gates)
    {
        foreach (Transform child in gatesToBuyContainer.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (var gate in gates)
        {
            var purchasableGateObject = Instantiate(purchasableGatePrefab, gatesToBuyContainer.transform);
            purchasableGateObject.GetComponent<PurchasableGate>().Init(this, gate, gate.price < state.moneyToSpend);
        }

        UpdateMoney();
    }

    public void UpdateMoney()
    {
        moneyDisplay.text = state.moneyToSpend.ToString();
    }

    public void Continue()
    {
        this.gameObject.SetActive(false);
        table.GoToNextLevel();
    }
}
