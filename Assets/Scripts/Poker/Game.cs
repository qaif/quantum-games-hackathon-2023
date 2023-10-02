using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum GamePhase
{
    PreFlop,
    Flop,
    Turn,
    River,
}

public struct Seat
{
    public int index;

    public int currentMoney;
    public int currentBet;
    public bool folded;

    public List<Card> cards;
}

public class Game
{
    PokerEvents pokerEvents;

    Deck deck;
    List<Card> cardsOnTable;
    GamePhase phase;

    Seat[] players;
    int currentMaxBet = 0;

    int currentlyBettingPlayer;
    int lastRaisingPlayer;

    bool acceptingBets;

    const int smallBlind = 100;
    const int bigBlind = 200;

    void StartBettingRound()
    {
        acceptingBets = true;
    }

    void ProgressBetting(int playerIndex, int value)
    {
        int activePlayers = players.Where(seat => !seat.folded).Count();
        if (activePlayers <= 1)
        {
            Debug.Log("Only one remaining player");
            EndGame();
            return;
        }

        while (true)
        {
            currentlyBettingPlayer = (currentlyBettingPlayer + 1) % players.Length;
            if (!players[currentlyBettingPlayer].folded) break;
        }

        if (currentlyBettingPlayer == lastRaisingPlayer)
        {
            Debug.Log("Whole betting round went around");
            EndBettingRound();
            return;
        }

        pokerEvents.betFromPlayerRequested(currentlyBettingPlayer, currentMaxBet);
    }

    void EndBettingRound()
    {
        acceptingBets = false;
        GoToNextPhase();
    }

    private void GoToNextPhase()
    {
        switch (phase)
        {
            case GamePhase.PreFlop:
                Flop();
                return;
            case GamePhase.Flop:
                Turn();
                return;
            case GamePhase.Turn:
                River();
                return;
            case GamePhase.River:
                EndGame();
                return;
        }
        return;
    }

    private void EndGame()
    {
        var activePlayers = players.Where(player => !player.folded);
        var handStrengths = activePlayers.Select(seat => Tuple.Create(seat, Figures.DetectBestFigure(cardsOnTable.ToArray(), seat.cards.ToArray())));

        var orderedHandStrengths = handStrengths.OrderByDescending(tup => tup.Item2.Strength());

        var biggestStrength = orderedHandStrengths.First().Item2.Strength();
        var bestHands = orderedHandStrengths.Where(tup => (tup.Item2.Strength() == biggestStrength)).Select(tup => tup.Item1.index);

        var pot = players.Select(player => player.currentBet).Sum();
        var potPerPlayer = pot / bestHands.Count();

        foreach (var winningPlayerIndex in bestHands)
        {
            players[winningPlayerIndex].currentMoney += potPerPlayer;
        }

        pokerEvents.FinishGame(bestHands.ToArray(), players.Select(player => player.currentMoney).ToArray());
    }

    void DealCard()
    {
        cardsOnTable.Add(deck.NextCard());
    }

    void PreFlop()
    {
        for (int i = 0; i < players.Length; i++)
        {
            var card = deck.NextCard();
            players[i].cards.Add(card);
            pokerEvents.DealCard(i, card);

            card = deck.NextCard();
            players[i].cards.Add(card);
            pokerEvents.DealCard(i, card);
        }

        HandleSubmittedBet(1, smallBlind);
        HandleSubmittedBet(2 % players.Length, bigBlind);

        currentlyBettingPlayer = 3 % players.Length; // Starting from player one away from big blind
        lastRaisingPlayer = currentlyBettingPlayer;

        StartBettingRound();
    }

    void Flop()
    {
        phase = GamePhase.Flop;
        for (int i = 0; i < 3; i++)
        {
            DealCard();
        }

        // Finding first player who didn't fold, starting from small blind
        currentlyBettingPlayer = 1;
        while (true)
        {
            if (!players[currentlyBettingPlayer].folded)
            {
                break;
            }
            currentlyBettingPlayer = (currentlyBettingPlayer + 1) % players.Length;
        }

        StartBettingRound();
    }

    void Turn()
    {
        phase = GamePhase.Turn;
        DealCard();
        StartBettingRound();
    }

    void River()
    {
        phase = GamePhase.River;
        DealCard();
        StartBettingRound();
    }

    void ProcessBet(Seat player, int raiseAmount)
    {
        player.currentMoney -= raiseAmount;
        player.currentBet += raiseAmount;

        if (raiseAmount > 0)
        {
            currentMaxBet = player.currentBet;
        }
    }

    void HandleSubmittedBet(int playerIndex, int bet)
    {
        if (!acceptingBets)
        {
            Debug.LogWarning("Invalid bet - not accepting bets at this time");
            return;
        }

        if (currentlyBettingPlayer != playerIndex)
        {
            Debug.LogWarning("Invalid bet - out of turn");
            return;
        }
        Seat seat = players[playerIndex];

        if (bet == -1)
        {
            if (seat.currentBet == currentMaxBet)
            {
                Debug.LogWarning("Invalid bet, folding without a raise");
                return;
            }

            seat.folded = true;
            pokerEvents.AcceptBet(playerIndex, bet);
            return;
        }

        var raiseAmount = bet - seat.currentBet;
        if (raiseAmount < 0)
        {
            Debug.LogWarning("Invalid bet: negative raise");
            return;
        }

        if (raiseAmount > seat.currentMoney)
        {
            Debug.LogWarning("Invalid bet: not enough money left");
            return;
        }

        ProcessBet(seat, raiseAmount);

        if (raiseAmount > 0)
        {
            lastRaisingPlayer = playerIndex;
        }

        pokerEvents.AcceptBet(playerIndex, bet);
        return;
    }

    public void Start()
    {
        PreFlop();
    }

    public Game(PokerEvents pokerEvents, Deck deck, int[] playersMoney)
    {
        // Init players
        this.players = new Seat[playersMoney.Length];
        for (int i = 0; i < playersMoney.Length; i++)
        {
            players[i].index = i;
            players[i].currentMoney = playersMoney[i];
            players[i].currentBet = 0;
            players[i].folded = false;
            players[i].cards = new List<Card>();
        }

        this.pokerEvents = pokerEvents;

        this.pokerEvents.betFromPlayerSubmitted += HandleSubmittedBet;
        this.pokerEvents.betFromPlayerAccepted += ProgressBetting;

        this.deck = deck;
        this.phase = GamePhase.PreFlop;
    }
}
