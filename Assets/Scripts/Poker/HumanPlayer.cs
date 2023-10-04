using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayer : MonoBehaviour
{
    int playerIndex;

    public int startingMoney = 1000;
    public int currentMoney;

    public Button foldButton;
    public Button checkButton;
    public Button callButton;
    public Button raiseButton;

    public TMP_Text moneyDisplay;
    public TMP_Text betDisplay;

    public Image card1;
    public Image card2;

    Game enteredGame;

    public HumanPlayer()
    {
        currentMoney = startingMoney;
    }

    public void EnterGame(Game game, int playerIndex)
    {
        this.playerIndex = playerIndex;
        enteredGame = game;
        enteredGame.betFromPlayerRequested += EnableBettingControls;
    }

    public void ExitGame(Game game)
    {
        enteredGame = null;
    }

    private void EnableBettingControls(Seat player, int currentBet)
    {
        Debug.Log($"EnableBettingControls called with {player.index} and {currentBet}");

        if (player.index != this.playerIndex) return;
        UpdateCards();

        raiseButton.interactable = true;

        if (player.currentBet < currentBet)
        {
            // Raise
            callButton.interactable = true;
            foldButton.interactable = true;
        }
        else
        {
            checkButton.interactable = true;
        }
    }

    private void DisableBettingControls()
    {
        foldButton.interactable = false;
        callButton.interactable = false;
        checkButton.interactable = false;
        raiseButton.interactable = false;
    }

    public void UpdateCards()
    {
        var c1 = enteredGame.players[playerIndex].cards[0];
        var sprite = Resources.Load<Sprite>($"Images/Deck/{c1.rank}_{c1.suite}");
        card1.sprite = sprite;

        var c2 = enteredGame.players[playerIndex].cards[1];
        var sprite2 = Resources.Load<Sprite>($"Images/Deck/{c2.rank}_{c2.suite}");
        card2.sprite = sprite2;
    }

    public void UpdateMoney()
    {
        if (enteredGame == null) return;
        moneyDisplay.text = $"Money: {enteredGame.players[playerIndex].currentMoney}";
    }

    public void UpdateBet()
    {
        if (enteredGame == null) return;
        betDisplay.text = $"Bet: {enteredGame.players[playerIndex].currentBet}";
    }

    public void Fold()
    {
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, -1);
    }

    public void Check()
    {
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet);
    }

    public void Call()
    {
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet);
    }

    public void Raise()
    {
        // TODO: check limits
        DisableBettingControls();
        // const raise amount for now;
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet + 200);
    }

    void Update()
    {
        UpdateMoney();
        UpdateBet();
    }
}
