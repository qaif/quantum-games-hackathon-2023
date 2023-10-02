using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu]
public class PokerEvents : ScriptableObject
{
    public UnityAction<int, int> betFromPlayerRequested;
    public UnityAction<int, int> betFromPlayerSubmitted;
    public UnityAction<int, int> betFromPlayerAccepted;

    public UnityAction<int, Card> cardDealt;

    public UnityAction<int[], int[]> gameFinished;


    public void RequestBetFromPlayer(int playerIndex, int currentValue)
    {
        betFromPlayerRequested?.Invoke(playerIndex, currentValue);
    }

    public void SubmitBet(int playerIndex, int value)
    {
        betFromPlayerSubmitted?.Invoke(playerIndex, value);
    }

    public void AcceptBet(int playerIndex, int value)
    {
        betFromPlayerAccepted?.Invoke(playerIndex, value);
    }

    public void DealCard(int playerIndex, Card card)
    {
        cardDealt?.Invoke(playerIndex, card);
    }

    public void FinishGame(int[] winningPlayers, int[] remainingMoney)
    {
        gameFinished?.Invoke(winningPlayers, remainingMoney);
    }
}
