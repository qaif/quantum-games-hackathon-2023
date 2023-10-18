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

class Utils
{
    public static IEnumerable<Card> TakeHighestCards(Card[] cards, int n)
    {
        var sortedCards = cards.OrderByDescending(card => card.rankInt);
        return sortedCards.Take(n);
    }

    public static Card[] DetectFiveInARow(Card[] cards)
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

    public static Card[] DetectFiveCardColor(Card[] cards)
    {
        var colorWithFlush = cards.GroupBy(card => card.suite).Where(group => group.Count() >= 5);

        if (colorWithFlush.Count() == 0)
        {
            return null;
        }

        return colorWithFlush.First().ToArray();
    }
}

public interface Figure
{
    Card[] Cards();
    long Strength();
}

public class HighCard : Figure
{
    IEnumerable<Card> highCards;

    public static HighCard Create(Card[] cards)
    {
        var figure = new HighCard
        {
            highCards = Utils.TakeHighestCards(cards, 5)
        };
        return figure;
    }

    public Card[] Cards()
    {
        return highCards.ToArray();
    }

    public long Strength()
    {
        long strength = 0;
        foreach (var card in highCards)
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Pair : Figure
{
    IEnumerable<Card> pair;
    IEnumerable<Card> highCards;

    public static Pair TryCreate(Card[] cards)
    {
        var pairs = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 1) return null;

        var figure = new Pair
        {
            pair = pairs.First(),
            highCards = Utils.TakeHighestCards(cards.Where(card => card.rankInt != pairs.First().Key).ToArray(), 3)
        };

        return figure;
    }

    public Card[] Cards()
    {
        return pair.Concat(highCards).ToArray();
    }

    public long Strength()
    {
        long strength = 1;
        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class TwoPairs : Figure
{
    IEnumerable<Card> firstPair;
    IEnumerable<Card> secondPair;
    IEnumerable<Card> highCards;

    public static TwoPairs TryCreate(Card[] cards)
    {
        var pairs = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 2) return null;

        var twoPairs = pairs.Take(2).ToArray();

        var firstPair = twoPairs[0];
        var secondPair = twoPairs[1];
        var highCards = Utils.TakeHighestCards(cards.Where(card => card.rankInt != firstPair.Key).Where(card => card.rankInt != secondPair.Key).ToArray(), 1);


        var figure = new TwoPairs
        {
            firstPair = firstPair,
            secondPair = secondPair,
            highCards = highCards,
        };

        return figure;
    }

    public Card[] Cards()
    {
        return firstPair.Concat(secondPair).Concat(highCards).ToArray();
    }

    public long Strength()
    {
        // Skip single pairs
        long strength = 1 << 1;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Triple : Figure
{
    IEnumerable<Card> triple;
    IEnumerable<Card> highCards;

    public static Triple TryCreate(Card[] cards)
    {
        var triples = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 3).OrderByDescending(group => group.Key);
        if (triples.Count() < 1) return null;

        var triple = triples.First();
        var highCards = Utils.TakeHighestCards(cards.Where(card => card.rankInt != triple.Key).ToArray(), 2);

        var figure = new Triple
        {
            triple = triple,
            highCards = highCards,
        };

        return figure;
    }

    public Card[] Cards()
    {
        return triple.Concat(highCards).ToArray();
    }

    public long Strength()
    {
        // Skip single and double pairs
        long strength = 1 << 2;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Straight : Figure
{
    IEnumerable<Card> cards;

    public static Straight TryCreate(Card[] cards)
    {
        var straight = Utils.DetectFiveInARow(cards);
        if (straight == null)
        {
            return null;
        }

        var figure = new Straight
        {
            cards = straight
        };
        return figure;
    }

    public Card[] Cards()
    {
        return cards.ToArray();
    }

    public long Strength()
    {
        // Skip triples, single and double pairs
        long strength = 1 << 3;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Flush : Figure
{
    IEnumerable<Card> cards;

    public static Flush TryCreate(Card[] cards)
    {
        var flush = Utils.DetectFiveCardColor(cards);
        if (flush == null)
        {
            return null;
        }

        var figure = new Flush
        {
            cards = flush.OrderByDescending(card => card.rankInt).Take(5)
        };
        return figure;
    }

    public Card[] Cards()
    {
        return cards.ToArray();
    }

    public long Strength()
    {
        // Skip straight, triples, single and double pairs
        long strength = 1 << 4;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Full : Figure
{
    IEnumerable<Card> triple;
    IEnumerable<Card> pair;

    public static Full TryCreate(Card[] cards)
    {
        var triples = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 3).OrderByDescending(group => group.Key);
        if (triples.Count() < 1) return null;
        var triple = triples.First();

        var pairs = cards.Where(card => card.rankInt != triple.Key).GroupBy(card => card.rankInt).Where(group => group.Count() >= 2).OrderByDescending(group => group.Key);
        if (pairs.Count() < 1) return null;
        var pair = pairs.First();

        var figure = new Full
        {
            triple = triple,
            pair = pair,
        };
        return figure;
    }

    public Card[] Cards()
    {
        return triple.Concat(pair).ToArray();
    }

    public long Strength()
    {
        // Skip flush, straight, triples, single and double pairs
        long strength = 1 << 5;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Four : Figure
{
    IEnumerable<Card> four;
    IEnumerable<Card> highCard;

    public static Four TryCreate(Card[] cards)
    {
        var fours = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 4);
        if (fours.Count() < 1) return null;

        var four = fours.First();
        var highCards = Utils.TakeHighestCards(cards.Where(card => card.rankInt != four.Key).ToArray(), 1);

        var figure = new Four
        {
            four = four,
            highCard = highCards,
        };
        return figure;
    }

    public Card[] Cards()
    {
        return four.Concat(highCard).ToArray();
    }

    public long Strength()
    {
        // Skip full, flush, straight, triples, single and double pairs
        long strength = 1 << 6;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Poker : Figure
{
    IEnumerable<Card> cards;

    public static Poker TryCreate(Card[] cards)
    {
        var colorWithFlush = Utils.DetectFiveCardColor(cards);
        if (colorWithFlush == null)
        {
            return null;
        }

        var straightFlush = Utils.DetectFiveInARow(colorWithFlush);
        if (straightFlush == null)
        {
            return null;
        }

        var figure = new Poker
        {
            cards = straightFlush,
        };
        return figure;
    }

    public Card[] Cards()
    {
        return cards.ToArray();
    }

    public long Strength()
    {
        // Skip four, full, flush, straight, triples, single and double pairs
        long strength = 1 << 7;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Five : Figure
{
    IEnumerable<Card> five;

    public static Five TryCreate(Card[] cards)
    {
        var fives = cards.GroupBy(card => card.rankInt).Where(group => group.Count() >= 5);
        if (fives.Count() < 1) return null;

        var five = fives.First();

        var figure = new Five
        {
            five = five,
        };
        return figure;
    }

    public Card[] Cards()
    {
        return five.ToArray();
    }

    public long Strength()
    {
        // Skip poker, four, full, flush, straight, triples, single and double pairs
        long strength = 1 << 8;

        foreach (var card in Cards())
        {
            strength <<= 4;
            strength += card.rankInt;
        }
        return strength;
    }
}

public class Figures
{
    public static Figure DetectBestFigure(Card[] table, Card[] hand)
    {
        Assert.AreEqual(table.Length, 5);
        Assert.AreEqual(hand.Length, 2);

        var cards = table.Concat(hand).ToArray();

        Figure bestFigure;

        bestFigure = Five.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Poker.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Four.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Full.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Flush.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Straight.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Triple.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = TwoPairs.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        bestFigure = Pair.TryCreate(cards);
        if (bestFigure != null) return bestFigure;

        return HighCard.Create(cards);
    }
}