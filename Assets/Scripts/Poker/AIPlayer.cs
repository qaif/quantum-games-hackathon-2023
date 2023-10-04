using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer
{
    Game enteredGame;
    int playerIndex;

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
}

