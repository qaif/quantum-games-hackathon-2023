using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    [HideInInspector] public int currentMoney;

    public TMP_Text moneyDisplay;
    public TMP_Text betDisplay;

    Game enteredGame;
    int playerIndex;

    public void EnterGame(Game game, int playerIndex)
    {
        this.playerIndex = playerIndex;
        enteredGame = game;
        enteredGame.betFromPlayerRequested += Bet;
    }

    public bool IsPlayingInTheGame()
    {
        return enteredGame != null;
    }

    public void ExitGame(Game game)
    {
        if (enteredGame != null) enteredGame.betFromPlayerRequested -= Bet;
        enteredGame = null;
    }

    void Bet(Seat player, int currentBet)
    {
        if (player.index != playerIndex) return;
        var callAmount = Math.Min(currentBet, player.currentMoney + player.currentBet);

        // Always check for now
        enteredGame.SubmitBet(playerIndex, callAmount);
    }

    public void UpdateMoney()
    {
        if (enteredGame == null) return;
        moneyDisplay.text = $"{enteredGame.players[playerIndex].currentMoney}";
    }

    public void UpdateBet()
    {
        if (enteredGame == null) return;
        betDisplay.text = $"{enteredGame.players[playerIndex].currentBet}";
    }

    void Update()
    {
        UpdateMoney();
        UpdateBet();
    }
}

