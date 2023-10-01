using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card
{
    static Dictionary<string, string> suites = new Dictionary<string, string>() {
        {"00", "clubs"},
        {"01", "diamonds"},
        {"10", "hearts"},
        {"11", "spades"},
    };

    static Dictionary<string, string> ranks = new Dictionary<string, string>() {
        {"0000", "one"},
        {"0001", "two"},
        {"0010", "three"},
        {"0011", "four"},
        {"0100", "five"},
        {"0101", "six"},
        {"0110", "seven"},
        {"0111", "eight"},
        {"1000", "nine"},
        {"1001", "ten"},
        {"1010", "eleven"},
        {"1011", "tweleve"},
        {"1100", "jack"},
        {"1101", "queen"},
        {"1110", "king"},
        {"1111", "ace"},
    };

    static string RandomKeyFromDict(Dictionary<string, string> dict)
    {
        var keys = dict.Keys.ToList();
        var randomIndex = UnityEngine.Random.Range(0, keys.Count - 1);
        return keys[randomIndex];
    }

    public static Card Random()
    {
        var randomSuite = RandomKeyFromDict(Card.suites);
        var randomRank = RandomKeyFromDict(Card.ranks);

        return new Card(randomSuite, randomRank);
    }

    public string suite;
    public string rank;
    public int rankInt;

    public Card(string suite, string rank)
    {
        this.suite = suite;
        this.rank = rank;
        this.rankInt = Convert.ToInt32(rank, 2);
    }

    public string Name()
    {
        return $"{Card.ranks[this.rank]} of {Card.suites[this.suite]}";
    }

    public override string ToString()
    {
        return Name();
    }
}
