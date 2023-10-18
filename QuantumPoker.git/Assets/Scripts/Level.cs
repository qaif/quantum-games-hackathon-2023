using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GateToBuy
{
    public GateType gateType;
    public int price;
}


[CreateAssetMenu]
public class Level : ScriptableObject
{
    public int[] oponentMoney;
    public int humanMoney;

    public bool isBossLevel = false;

    public GateToBuy[] gatesToBuy;

    public string dialogText;

    public Sprite bossSprite;

    public void Initialize(Table table)
    {
        table.humanPlayer.currentMoney = humanMoney;
        int i = 0;

        foreach (var player in table.robotPlayers)
        {
            player.gameObject.SetActive(true);
            player.currentMoney = oponentMoney[i];
            i++;
        }

        table.humanPlayer.isBossLevel = false;
        if (isBossLevel)
        {
            InitializeBoss(table);
        }
    }

    void InitializeBoss(Table table)
    {
        table.humanPlayer.isBossLevel = true;
        SoundManager.Instance.Boss();
        table.bossDialog.SetActive(true);
        table.robotPlayers[1].picture.sprite = bossSprite;
    }
}
