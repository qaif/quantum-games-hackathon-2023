using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

enum GamePhase
{
    PreFlop,
    Flop,
    Turn,
    River,
}

public class Seat
{
    public int index;

    public int currentMoney;
    public int currentBet = 0;

    public bool folded = false;
    // TODO: fill this properly
    public bool alreadyBetThisRound = false;

    public List<Card> cards = new List<Card>();
}

public class Game
{
    const int smallBlind = 100;
    const int bigBlind = 200;

    // PokerEvents pokerEvents;
    public List<Card> cardsOnTable = new List<Card>();
    public Seat[] players;
    public int currentMaxBet = 0;
    public int currentPot => players.Sum(player => player.currentBet);

    public UnityAction<Seat, int> betFromPlayerRequested;
    public UnityAction gameFinished;
    public UnityAction newCardsOnBoard;

    Deck deck;
    GamePhase phase = GamePhase.PreFlop;

    int startingPlayer;
    int currentlyBettingPlayer;
    int lastRaisingPlayer;
    bool acceptingBets;

    void StartBettingRound()
    {
        acceptingBets = true;
        foreach (var player in players)
        {
            player.alreadyBetThisRound = false;
        }

        betFromPlayerRequested?.Invoke(players[currentlyBettingPlayer], currentMaxBet);
    }

    void ProgressBetting()
    {
        int activePlayers = players.Where(seat => (!seat.folded && seat.currentMoney > 0)).Count();
        if (activePlayers <= 1)
        {
            Debug.Log("Only one remaining player");
            EndGame();
            return;
        }

        while (true)
        {
            currentlyBettingPlayer = (currentlyBettingPlayer + 1) % players.Length;
            if (!players[currentlyBettingPlayer].folded && (players[currentlyBettingPlayer].currentMoney > 0)) break;
        }

        if ((currentlyBettingPlayer == lastRaisingPlayer) && (players.All(player => (player.folded || player.alreadyBetThisRound))))
        {
            Debug.Log("Whole betting round went around");
            EndBettingRound();
            return;
        }

        betFromPlayerRequested(players[currentlyBettingPlayer], currentMaxBet);
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

    public Seat[] GetWinningPlayers()
    {
        var activePlayers = players.Where(player => !player.folded);
        var handStrengths = activePlayers.Select(seat => Tuple.Create(seat, Figures.DetectBestFigure(cardsOnTable.ToArray(), seat.cards.ToArray())));

        var orderedHandStrengths = handStrengths.OrderByDescending(tup => tup.Item2.Strength());

        var biggestStrength = orderedHandStrengths.First().Item2.Strength();
        var bestHands = orderedHandStrengths.Where(tup => (tup.Item2.Strength() == biggestStrength)).Select(tup => tup.Item1.index);

        return bestHands.Select(hs => players[hs]).ToArray();
    }

    private void EndGame()
    {
        while (cardsOnTable.Count() < 5)
        {
            DealCard();
        }

        // TODO: check for multiple pots
        var winningPlayers = GetWinningPlayers();
        var potPerPlayer = currentPot / winningPlayers.Count();

        foreach (var player in winningPlayers)
        {
            player.currentMoney += potPerPlayer;
        }

        gameFinished?.Invoke();
    }

    void DealCard()
    {
        cardsOnTable.Add(deck.NextCard());
        newCardsOnBoard?.Invoke();
    }

    void PreFlop()
    {
        for (int i = 0; i < players.Length; i++)
        {
            var card = deck.NextCard();
            players[i].cards.Add(card);

            card = deck.NextCard();
            players[i].cards.Add(card);
        }

        ProcessBet(players[(startingPlayer + 1) % players.Length], smallBlind);
        ProcessBet(players[(startingPlayer + 2) % players.Length], bigBlind);

        currentlyBettingPlayer = 3 % players.Length; // Starting from player one away from big blind
        lastRaisingPlayer = currentlyBettingPlayer;

        StartBettingRound();
    }

    public void Flop()
    {
        phase = GamePhase.Flop;
        for (int i = 0; i < 3; i++)
        {
            DealCard();
        }

        // Finding first player who didn't fold, starting from small blind
        currentlyBettingPlayer = (startingPlayer + 1) % players.Length;
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

    public void Turn()
    {
        phase = GamePhase.Turn;
        DealCard();
        StartBettingRound();
    }

    public void River()
    {
        phase = GamePhase.River;
        DealCard();
        StartBettingRound();
    }

    void ProcessBet(Seat player, int raiseAmount)
    {
        player.currentMoney -= raiseAmount;
        player.currentBet += raiseAmount;

        if (player.currentBet > currentMaxBet)
        {
            currentMaxBet = player.currentBet;
        }
    }

    public void SubmitBet(int playerIndex, int bet)
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
        seat.alreadyBetThisRound = true;

        if (bet == -1)
        {
            if (seat.currentBet == currentMaxBet)
            {
                Debug.LogWarning("Invalid bet, folding without a raise");
                return;
            }

            seat.folded = true;

            ProgressBetting();
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

        if ((bet < currentMaxBet) && (seat.currentMoney != raiseAmount))
        {
            Debug.LogWarning("Invalid bet: all in bets only possible when no money is left");
            return;
        }

        ProcessBet(seat, raiseAmount);

        if (bet > currentMaxBet)
        {
            lastRaisingPlayer = playerIndex;
        }

        ProgressBetting();

        return;
    }

    public void Start()
    {
        PreFlop();
    }

    public Game(Deck deck, int[] playersMoney, int startingPlayer)
    {
        // Init players
        this.startingPlayer = startingPlayer;

        this.players = new Seat[playersMoney.Length];
        for (int i = 0; i < playersMoney.Length; i++)
        {
            players[i] = new Seat()
            {
                index = i,
                currentMoney = playersMoney[i],
            };
        }

        this.deck = deck;
    }
}
