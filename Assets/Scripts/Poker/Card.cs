using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Suite
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public static class CardExtensions
{
    static Dictionary<string, Suite> suites = new Dictionary<string, Suite>() {
        {"00", Suite.Clubs},
        {"01", Suite.Diamonds},
        {"10", Suite.Hearts},
        {"11", Suite.Spades},
    };

    public static Suite RandomSuite()
    {
        var keys = suites.Keys.ToList();
        var randomIndex = UnityEngine.Random.Range(0, keys.Count);
        return suites[keys[randomIndex]];
    }

    public static string ToString(this Suite suite)
    {
        switch (suite)
        {
            case Suite.Clubs:
                return "clubs";
            case Suite.Diamonds:
                return "diamonds";
            case Suite.Hearts:
                return "heart";
            case Suite.Spades:
                return "spades";
        }
        return "";
    }

    static Dictionary<int, Rank> intToRank = new Dictionary<int, Rank>() {
        {0, Rank.One},
        {1, Rank.Two},
        {2, Rank.Three},
        {3, Rank.Four},
        {4, Rank.Five},
        {5, Rank.Six},
        {6, Rank.Seven},
        {7, Rank.Eight},
        {8, Rank.Nine},
        {9, Rank.Ten},
        {10, Rank.Eleven},
        {11, Rank.Twelve},
        {12, Rank.Jack},
        {13, Rank.Queen},
        {14, Rank.King},
        {15, Rank.Ace},
    };
    static Dictionary<Rank, int> rankToInt = intToRank.ToDictionary((i) => i.Value, (i) => i.Key);

    public static Rank RandomRank()
    {
        var keys = intToRank.Keys.ToList();
        var randomIndex = UnityEngine.Random.Range(0, keys.Count);
        return intToRank[keys[randomIndex]];
    }

    public static string ToString(this Rank rank)
    {
        switch (rank)
        {
            case Rank.One:
                return "one";
            case Rank.Two:
                return "two";
            case Rank.Three:
                return "three";
            case Rank.Four:
                return "four";
            case Rank.Five:
                return "five";
            case Rank.Six:
                return "six";
            case Rank.Seven:
                return "seven";
            case Rank.Eight:
                return "eight";
            case Rank.Nine:
                return "nine";
            case Rank.Ten:
                return "ten";
            case Rank.Eleven:
                return "eleven";
            case Rank.Twelve:
                return "twelve";
            case Rank.Jack:
                return "jack";
            case Rank.Queen:
                return "queen";
            case Rank.King:
                return "king";
            case Rank.Ace:
                return "ace";
        }
        return "";
    }

    public static int ToInt(this Rank rank)
    {
        return rankToInt[rank];
    }
}

public enum Rank
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Eleven,
    Twelve,
    Jack,
    Queen,
    King,
    Ace
}

public class Card
{
    public static Card Random()
    {
        var randomSuite = CardExtensions.RandomSuite();
        var randomRank = CardExtensions.RandomRank();

        return new Card(randomSuite, randomRank);
    }

    public Suite suite;
    public Rank rank;
    public int rankInt;

    public Card(Suite suite, Rank rank)
    {
        this.suite = suite;
        this.rank = rank;
        this.rankInt = rank.ToInt();
    }

    public string Name()
    {
        return $"{this.rank.ToString()} of {this.suite.ToString()}";
    }

    public override string ToString()
    {
        return Name();
    }
}
