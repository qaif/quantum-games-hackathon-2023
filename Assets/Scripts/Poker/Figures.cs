using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public enum FigureType
{
    HighCard,
    Pair,
    TwoPairs,
    Triple,
    Straight,
    Flush,
    Full,
    Four,
    Poker,
    Five
}

public class Figure
{
    public FigureType type;
    public Card[] cards;

    public Figure(FigureType type, Card[] cards)
    {
        Assert.AreEqual(cards.Length, 5);

        this.type = type;
        this.cards = cards;
    }
}

public class Figures
{
    static Figure DetectFive(Card[] cards)
    {
        var fives = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 5);
        if (fives.Count() < 1) return null;

        var five = fives.First();

        return new Figure(FigureType.Five, five.ToArray());
    }

    static Figure DetectPoker(Card[] cards)
    {
        var colorWithFlush = DetectFiveCardColor(cards);
        if (colorWithFlush == null)
        {
            return null;
        }

        var straightFlush = DetectFiveInARow(colorWithFlush);
        if (straightFlush == null)
        {
            return null;
        }

        return new Figure(FigureType.Poker, straightFlush);
    }

    static Figure DetectFour(Card[] cards)
    {
        var fours = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 4);
        if (fours.Count() < 1) return null;

        var four = fours.First();
        var highCards = TakeHighestCards(cards.Where(card => card.rankInt != four.Key).ToArray(), 1);

        return new Figure(FigureType.Four, four.Concat(highCards).ToArray());
    }

    static Figure DetectFull(Card[] cards)
    {
        var triples = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 3).OrderByDescending(group => group.Key);
        if (triples.Count() < 1) return null;
        var triple = triples.First();

        var pairs = cards.Where(card => card.rankInt != triple.Key).GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 1) return null;

        var pair = pairs.First();
        return new Figure(FigureType.Full, triple.Concat(pair).ToArray());
    }

    static Card[] DetectFiveCardColor(Card[] cards)
    {
        var colorWithFlush = cards.GroupBy(card => card.suite).Where(group => group.Count() >= 5);

        if (colorWithFlush.Count() == 0)
        {
            return null;
        }

        return colorWithFlush.First().ToArray();
    }

    static Figure DetectFlush(Card[] cards)
    {

        var colorWithFlush = DetectFiveCardColor(cards);

        if (colorWithFlush == null)
        {
            return null;
        }

        var highestColorCards = colorWithFlush.OrderByDescending(card => card.rankInt).Take(5);
        return new Figure(FigureType.Flush, highestColorCards.ToArray());
    }

    static Card[] DetectFiveInARow(Card[] cards)
    {
        var cardsGroupedByRank = cards.GroupBy(card => card.rankInt).ToDictionary(group => group.Key, group => cards.Where(card => card.rankInt == group.Key));

        for (int i = 15; i >= 4; i--)
        {
            // Check if there is a straight with highest card at rank i
            var keysToCheck = new List<int>() { i, i - 1, i - 2, i - 3, i - 4 };

            if (keysToCheck.All(key => cardsGroupedByRank.ContainsKey(key)))
            {
                return keysToCheck.Select(key => cardsGroupedByRank[key].First()).ToArray();
            }
        }

        return null;
    }

    static Figure DetectStraight(Card[] cards)
    {
        var straight = DetectFiveInARow(cards);
        if (straight == null)
        {
            return null;
        }

        return new Figure(FigureType.Straight, straight);
    }

    static Figure DetectTriple(Card[] cards)
    {
        var triples = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 3).OrderByDescending(group => group.Key);
        if (triples.Count() < 1) return null;

        var triple = triples.First();
        var highCards = TakeHighestCards(cards.Where(card => card.rankInt != triple.Key).ToArray(), 2);

        return new Figure(FigureType.Triple, triple.Concat(highCards).ToArray());
    }

    static Figure DetectTwoHighestPairs(Card[] cards)
    {
        var pairs = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 2) return null;

        var twoPairs = pairs.Take(2).ToArray();

        var firstPair = twoPairs[0];
        var secondPair = twoPairs[1];

        var highCards = TakeHighestCards(cards.Where(card => card.rankInt != firstPair.Key).Where(card => card.rankInt != secondPair.Key).ToArray(), 1);

        return new Figure(FigureType.TwoPairs, firstPair.Concat(secondPair).Concat(highCards).ToArray());
    }

    static Figure DetectHighestPair(Card[] cards)
    {
        var pairs = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 1) return null;

        var pair = pairs.First();
        var highCards = TakeHighestCards(cards.Where(card => card.rankInt != pair.Key).ToArray(), 3);

        return new Figure(FigureType.Pair, pair.Concat(highCards).ToArray());
    }

    static Figure DetectHighestCard(Card[] cards)
    {
        return new Figure(FigureType.HighCard, TakeHighestCards(cards, 5));
    }

    static Card[] TakeHighestCards(Card[] cards, int n)
    {
        var sortedCards = cards.OrderByDescending(card => card.rankInt);
        return sortedCards.Take(n).ToArray();
    }

    public static Figure DetectBestFigure(Card[] table, Card[] hand)
    {
        Assert.AreEqual(table.Length, 5);
        Assert.AreEqual(hand.Length, 2);

        var cards = table.Concat(hand).ToArray();

        Figure bestFigure;

        bestFigure = DetectFive(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectPoker(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectFour(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectFull(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectFlush(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectStraight(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectTriple(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectTwoHighestPairs(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        bestFigure = DetectHighestPair(cards);
        if (bestFigure != null)
        {
            return bestFigure;
        }

        return DetectHighestCard(cards);
    }
}