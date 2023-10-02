using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestFigures
{
    [Test]
    public void TestHighCardIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.Nine),
            new(Suite.Diamonds, Rank.Ace),
            new(Suite.Spades, Rank.Six),
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.Jack),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.Ten),
            new(Suite.Hearts, Rank.Queen)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[4], hand[0], hand[1] }, figure.Cards());
    }

    [Test]
    public void TestPairIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.Six),
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.Nine),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Queen)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[4], hand[0], hand[1] }, figure.Cards());

    }

    [Test]
    public void TestTwoPairsAreProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.Six),
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.Four),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Queen)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[3], table[4], hand[0], hand[1] }, figure.Cards());
    }

    [Test]
    public void TestTripleIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.One),
            new(Suite.Diamonds, Rank.Six),
            new(Suite.Hearts, Rank.Four),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Queen)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.Cards());
    }

    [Test]
    public void TestStraightIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Three),
            new(Suite.Spades, Rank.Five),
            new(Suite.Diamonds, Rank.Ten),
            new(Suite.Hearts, Rank.Twelve),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.Two)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.Cards());
    }

    [Test]
    public void TestFlushIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Clubs, Rank.Eleven),
            new(Suite.Clubs, Rank.Jack),
            new(Suite.Diamonds, Rank.Ten),
            new(Suite.Clubs, Rank.Twelve),
        };
        Card[] hand = new Card[] {
            new(Suite.Clubs, Rank.Four),
            new(Suite.Clubs, Rank.Two)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[1], table[2], table[4], hand[0], hand[1] }, figure.Cards());
    }


    [Test]
    public void TestFullIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.One),
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.Four),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Queen)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[2], table[3], table[4], hand[0] }, figure.Cards());
    }

    [Test]
    public void TestFourIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.One),
            new(Suite.Diamonds, Rank.Four),
            new(Suite.Hearts, Rank.One),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Ten)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], table[4], hand[0] }, figure.Cards());
    }

    [Test]
    public void TestPokerIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.Two),
            new(Suite.Clubs, Rank.Three),
            new(Suite.Clubs, Rank.Five),
            new(Suite.Diamonds, Rank.Ten),
            new(Suite.Clubs, Rank.Twelve),
        };
        Card[] hand = new Card[] {
            new(Suite.Clubs, Rank.Four),
            new(Suite.Clubs, Rank.Six)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.Cards());
    }

    [Test]
    public void TestFiveIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new(Suite.Clubs, Rank.One),
            new(Suite.Diamonds, Rank.Twelve),
            new(Suite.Spades, Rank.One),
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.One),
        };
        Card[] hand = new Card[] {
            new(Suite.Diamonds, Rank.One),
            new(Suite.Hearts, Rank.Ten)
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[2], table[3], table[4], hand[0] }, figure.Cards());
    }


    [Test]
    public void TestStrengthsAreOrderedProperly()
    {
        // given
        Figure highCardTen = HighCard.Create(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Five), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.Ten) });
        Figure highCardKing = HighCard.Create(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Five), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.King) });

        Figure pairThrees = Pair.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Three), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.King) });
        Figure pairNines = Pair.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Three), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.Nine) });

        Figure twoPairsLow = TwoPairs.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.One), new(Suite.Diamonds, Rank.Three), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.Nine) });
        Figure twoPairsHigh = TwoPairs.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.One), new(Suite.Diamonds, Rank.Three), new(Suite.Clubs, Rank.King), new(Suite.Clubs, Rank.King) });

        Figure tripleOnes = Triple.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.One), new(Suite.Diamonds, Rank.One), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.King) });
        Figure tripleKings = Triple.TryCreate(new Card[] { new(Suite.Clubs, Rank.One), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.King), new(Suite.Clubs, Rank.King), new(Suite.Clubs, Rank.King) });

        Figure lowStraight = Straight.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Four), new(Suite.Clubs, Rank.Five), new(Suite.Clubs, Rank.Six) });
        Figure highStraight = Straight.TryCreate(new Card[] { new(Suite.Clubs, Rank.Eleven), new(Suite.Clubs, Rank.Twelve), new(Suite.Diamonds, Rank.Jack), new(Suite.Clubs, Rank.Queen), new(Suite.Clubs, Rank.King) });

        Figure lowFlush = Flush.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Three), new(Suite.Clubs, Rank.Seven), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.Eleven) });
        Figure mediumFlush = Flush.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Three), new(Suite.Clubs, Rank.Seven), new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.King) });
        Figure highFlush = Flush.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Three), new(Suite.Clubs, Rank.Seven), new(Suite.Clubs, Rank.Ten), new(Suite.Clubs, Rank.King) });

        Figure lowFull = Full.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Diamonds, Rank.Seven), new(Suite.Clubs, Rank.Seven), new(Suite.Clubs, Rank.Seven) });
        Figure mediumFull = Full.TryCreate(new Card[] { new(Suite.Clubs, Rank.King), new(Suite.Clubs, Rank.King), new(Suite.Diamonds, Rank.Seven), new(Suite.Clubs, Rank.Seven), new(Suite.Clubs, Rank.Seven) });
        Figure highFull = Full.TryCreate(new Card[] { new(Suite.Clubs, Rank.Three), new(Suite.Clubs, Rank.Three), new(Suite.Diamonds, Rank.Eight), new(Suite.Clubs, Rank.Eight), new(Suite.Clubs, Rank.Eight) });

        Figure lowFour = Four.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Diamonds, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.King) });
        Figure mediumFour = Four.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Diamonds, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Ace) });
        Figure highFour = Four.TryCreate(new Card[] { new(Suite.Clubs, Rank.Four), new(Suite.Clubs, Rank.Four), new(Suite.Diamonds, Rank.Four), new(Suite.Clubs, Rank.Four), new(Suite.Clubs, Rank.Two) });

        Figure lowPoker = Poker.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Three), new(Suite.Clubs, Rank.Four), new(Suite.Clubs, Rank.Five), new(Suite.Clubs, Rank.Six) });
        Figure highPoker = Poker.TryCreate(new Card[] { new(Suite.Clubs, Rank.Nine), new(Suite.Clubs, Rank.Ten), new(Suite.Clubs, Rank.Eleven), new(Suite.Clubs, Rank.Twelve), new(Suite.Clubs, Rank.Jack) });

        Figure lowFive = Five.TryCreate(new Card[] { new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Diamonds, Rank.Two), new(Suite.Clubs, Rank.Two), new(Suite.Clubs, Rank.Two) });
        Figure highFive = Five.TryCreate(new Card[] { new(Suite.Clubs, Rank.Queen), new(Suite.Clubs, Rank.Queen), new(Suite.Diamonds, Rank.Queen), new(Suite.Clubs, Rank.Queen), new(Suite.Clubs, Rank.Queen) });

        List<Figure> figuresOrderedByStrength = new List<Figure>() {
            highCardTen, highCardKing,
            pairThrees, pairNines,
            twoPairsLow, twoPairsHigh,
            tripleOnes, tripleKings,
            lowStraight, highStraight,
            lowFlush, mediumFlush, highFlush,
            lowFull, mediumFull, highFull,
            lowFour, mediumFour, highFour,
            lowPoker, highPoker,
            lowFive, highFive
        };

        // then
        for (int i = 1; i < figuresOrderedByStrength.Count; i++)
        {
            Assert.Greater(figuresOrderedByStrength[i].Strength(), figuresOrderedByStrength[i - 1].Strength());
        }
    }
}
