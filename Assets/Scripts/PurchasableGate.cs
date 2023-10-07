using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchasableGate : MonoBehaviour
{
    public GateToBuy gateToBuy;

    public TMP_Text gateNameDisplay;
    public TMP_Text priceDisplay;

    WinMenu menu;

    public void Init(WinMenu menu, GateToBuy gateToBuy, bool canAfford)
    {
        this.gateToBuy = gateToBuy;
        gateNameDisplay.text = gateToBuy.gateType.ToString();
        priceDisplay.text = gateToBuy.price.ToString();
        this.menu = menu;

        this.GetComponent<Button>().interactable = canAfford;
    }

    public void BuyGate()
    {
        menu.BuyGate(gateToBuy);
    }
}
