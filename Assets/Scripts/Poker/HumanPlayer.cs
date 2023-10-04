using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayer : MonoBehaviour
{
    int playerIndex;

    public Button foldButton;
    public Button checkButton;
    public Button raiseButton;

    public TMP_Text money;
    public TMP_Text bet;

    public Image card1;
    public Image card2;

    Game enteredGame;
    List<Card> cards = new List<Card>();

    public void EnterGame(Game game, int playerIndex)
    {
        this.playerIndex = playerIndex;
        enteredGame = game;
        enteredGame.betFromPlayerRequested += EnableBettingControls;

        UpdateMoney();
        UpdateBet();
    }

    public void ExitGame(Game game)
    {
        enteredGame = null;
    }

    private void EnableBettingControls(Seat player, int currentBet)
    {
        Debug.Log($"EnableBettingControls called with {player.index} and {currentBet}");

        UpdateBet();
        UpdateMoney();
        if (player.index != this.playerIndex) return;
        UpdateCards();

        checkButton.interactable = true;
        raiseButton.interactable = true;

        if (player.currentBet < currentBet)
        {
            // Raise
            foldButton.interactable = true;
        }
    }

    private void DisableBettingControls()
    {
        foldButton.interactable = false;
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
        money.text = $"Money: {enteredGame.players[playerIndex].currentMoney}";
    }

    public void UpdateBet()
    {
        bet.text = $"Bet: {enteredGame.currentMaxBet}";
    }

    public void Fold()
    {
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, -1);
        UpdateMoney();
    }

    public void Check()
    {
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet);
        UpdateMoney();
    }

    public void Raise()
    {
        DisableBettingControls();
        // const raise amount for now;
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet + 200);
        UpdateMoney();
    }
}
