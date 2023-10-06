using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    public int startingMoney = 1000;
    public int currentMoney;

    public TMP_Text moneyDisplay;
    public TMP_Text betDisplay;

    Game enteredGame;
    int playerIndex;

    public void Start()
    {
        currentMoney = startingMoney;
    }

    public void EnterGame(Game game, int playerIndex)
    {
        this.playerIndex = playerIndex;
        enteredGame = game;
        enteredGame.betFromPlayerRequested += Bet;
    }

    public void ExitGame(Game game)
    {
        enteredGame.betFromPlayerRequested -= Bet;
        enteredGame = null;
    }

    void Bet(Seat player, int currentBet)
    {
        if (player.index != playerIndex) return;

        // Always check for now
        enteredGame.SubmitBet(playerIndex, currentBet);
    }

    public void UpdateMoney()
    {
        if (enteredGame == null) return;
        moneyDisplay.text = $"Money: {enteredGame.players[playerIndex].currentMoney}";
    }

    public void UpdateBet()
    {
        if (enteredGame == null) return;
        betDisplay.text = $"Bet: {enteredGame.currentMaxBet}";
    }


    void Update()
    {
        UpdateMoney();
        UpdateBet();
    }
}

