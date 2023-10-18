using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase
{
    PreFlop,
    Flop,
    Turn,
    River,
}

public enum GameState
{
    Initialised,
    AwaitingBets,
    GameFinished,
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

    public bool nextPhaseReady = false;
    const float dealCardDelay = 0.5f;

    // PokerEvents pokerEvents;
    public List<Card> cardsOnTable = new List<Card>();
    public Seat[] players;
    public int currentMaxBet = 0;
    public int currentPot => players.Sum(player => player.currentBet);

    public UnityAction<Seat, int> betFromPlayerRequested;
    public UnityAction gameFinished;
    public UnityAction newCardsOnBoard;

    Deck deck;
    public GamePhase phase = GamePhase.PreFlop;

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
        lastRaisingPlayer = -1;

        betFromPlayerRequested?.Invoke(players[currentlyBettingPlayer], currentMaxBet);
    }

    void ProgressBetting()
    {
        var activePlayers = players.Where(seat => (!seat.folded && seat.currentMoney > 0));
        if (activePlayers.Count() <= 0)
        {
            Debug.Log("No remaining players");
            EndBettingRound();
            return;
        }

        // There is at least one player who didn't fold and have no zero money 
        while (true)
        {
            currentlyBettingPlayer = (currentlyBettingPlayer + 1) % players.Length;
            if (!players[currentlyBettingPlayer].folded && (players[currentlyBettingPlayer].currentMoney > 0)) break;
        }

        if (currentlyBettingPlayer == lastRaisingPlayer)
        {
            Debug.Log("Whole betting round went around after a raise");
            EndBettingRound();
            return;
        }

        if (activePlayers.All(player => (player.alreadyBetThisRound && (player.currentBet == currentMaxBet))))
        {
            Debug.Log("All active players already bet in this round");
            EndBettingRound();
            return;
        }

        betFromPlayerRequested(players[currentlyBettingPlayer], currentMaxBet);
    }

    void EndBettingRound()
    {
        acceptingBets = false;
        nextPhaseReady = true;
    }

    public IEnumerator GoToNextPhase()
    {
        nextPhaseReady = false;
        switch (phase)
        {
            case GamePhase.PreFlop:
                yield return Flop();
                break;
            case GamePhase.Flop:
                yield return Turn();
                break;
            case GamePhase.Turn:
                yield return River();
                break;
            case GamePhase.River:
                yield return EndGame();
                break;
        }
    }

    public Seat[] GetWinningPlayersForPot(int betSize)
    {
        var activePlayers = players.Where(player => !player.folded).Where(player => player.currentBet >= betSize);

        var handStrengths = activePlayers.Select(seat => Tuple.Create(seat, Figures.DetectBestFigure(cardsOnTable.ToArray(), seat.cards.ToArray())));

        var orderedHandStrengths = handStrengths.OrderByDescending(tup => tup.Item2.Strength());

        var biggestStrength = orderedHandStrengths.First().Item2.Strength();
        var bestHands = orderedHandStrengths.Where(tup => (tup.Item2.Strength() == biggestStrength)).Select(tup => tup.Item1.index);

        return bestHands.Select(hs => players[hs]).ToArray();
    }

    private IEnumerator EndGame()
    {
        while (cardsOnTable.Count() < 5)
        {
            yield return DealCard();
        }

        var betSizes = players.Where(player => !player.folded).Select(player => player.currentBet).Distinct().OrderBy(bet => bet);

        int previousBetSize = 0;
        foreach (int betSize in betSizes)
        {
            var winners = GetWinningPlayersForPot(betSize);
            var pot = players.Where(player => player.currentBet > previousBetSize).Sum(player => Math.Min(player.currentBet, betSize) - previousBetSize);
            var potPerPlayer = pot / winners.Count();

            var winnersList = winners.Select(player => player.index.ToString()).Aggregate("", (current, next) => current + ", " + next);
            var betsList = players.Select(player => player.currentBet.ToString()).Aggregate("", (current, next) => current + ", " + next);

            Debug.Log($"Distributing victories for bet size {betSize} to winners: {winnersList}, pot per player: {potPerPlayer}, all bets: {betsList}");

            previousBetSize = betSize;
            foreach (var player in winners)
            {
                player.currentMoney += potPerPlayer;
            }
        }

        gameFinished?.Invoke();
    }

    IEnumerator DealCard()
    {
        cardsOnTable.Add(deck.NextCard());
        newCardsOnBoard?.Invoke();
        SoundManager.Instance.Card();
        yield return new WaitForSeconds(dealCardDelay);
    }

    IEnumerator PreFlop()
    {
        for (int i = 0; i < players.Length; i++)
        {
            SoundManager.Instance.Card();
            var card = deck.NextCard();
            players[i].cards.Add(card);
            yield return new WaitForSeconds(0.2f);


            SoundManager.Instance.Card();
            card = deck.NextCard();
            players[i].cards.Add(card);
            yield return new WaitForSeconds(0.2f);
        }

        ProcessBet(players[(startingPlayer + 1) % players.Length], smallBlind);
        ProcessBet(players[(startingPlayer + 2) % players.Length], bigBlind);

        lastRaisingPlayer = -1;
        currentlyBettingPlayer = (startingPlayer + 3) % players.Length; // Starting from player one away from big blind

        StartBettingRound();
    }

    public IEnumerator Flop()
    {
        phase = GamePhase.Flop;
        for (int i = 0; i < 3; i++)
        {
            yield return DealCard();
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

    public IEnumerator Turn()
    {
        phase = GamePhase.Turn;
        yield return DealCard();
        StartBettingRound();
    }

    public IEnumerator River()
    {
        phase = GamePhase.River;
        yield return DealCard();
        StartBettingRound();
    }

    void ProcessBet(Seat player, int raiseAmount)
    {
        player.currentMoney -= raiseAmount;
        player.currentBet += raiseAmount;

        if (player.currentBet > currentMaxBet)
        {
            currentMaxBet = player.currentBet;
            lastRaisingPlayer = player.index;
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

        ProgressBetting();

        return;
    }

    public IEnumerator Start()
    {
        return PreFlop();
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
