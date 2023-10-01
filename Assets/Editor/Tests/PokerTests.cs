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
            new Card("00", "1000"),
            new Card("01", "1111"),
            new Card("11", "0101"),
            new Card("01", "0011"),
            new Card("10", "1100"),
        };
        Card[] hand = new Card[] {
            new Card("01", "1001"),
            new Card("10", "1101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.HighCard, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[4], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestPairIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0101"),
            new Card("01", "0011"),
            new Card("10", "1000"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Pair, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[4], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestTwoPairsAreProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0101"),
            new Card("01", "0011"),
            new Card("10", "0011"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.TwoPairs, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[3], table[4], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestTripleIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0000"),
            new Card("01", "0101"),
            new Card("10", "0011"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Triple, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestStraightIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "0010"),
            new Card("11", "0100"),
            new Card("01", "1001"),
            new Card("10", "1011"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0011"),
            new Card("10", "0001")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Straight, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestFlushIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("00", "1010"),
            new Card("00", "1100"),
            new Card("01", "1001"),
            new Card("00", "1011"),
        };
        Card[] hand = new Card[] {
            new Card("00", "0011"),
            new Card("00", "0001")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Flush, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[1], table[2], table[4], hand[0], hand[1] }, figure.cards);
    }


    [Test]
    public void TestFullIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0000"),
            new Card("01", "0011"),
            new Card("10", "0011"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Full, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[2], table[3], table[4], hand[0] }, figure.cards);
    }

    [Test]
    public void TestFourIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0000"),
            new Card("01", "0011"),
            new Card("10", "0000"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1001")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Four, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], table[4], hand[0] }, figure.cards);
    }

    [Test]
    public void TestPokerIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0001"),
            new Card("00", "0010"),
            new Card("00", "0100"),
            new Card("01", "1001"),
            new Card("00", "1011"),
        };
        Card[] hand = new Card[] {
            new Card("00", "0011"),
            new Card("00", "0101")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Poker, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[1], table[2], hand[0], hand[1] }, figure.cards);
    }

    [Test]
    public void TestFiveIsProperlyDetected()
    {
        // given
        Card[] table = new Card[] {
            new Card("00", "0000"),
            new Card("01", "1011"),
            new Card("11", "0000"),
            new Card("01", "0000"),
            new Card("10", "0000"),
        };
        Card[] hand = new Card[] {
            new Card("01", "0000"),
            new Card("10", "1001")
        };

        // when
        var figure = Figures.DetectBestFigure(table, hand);

        // then
        Assert.AreEqual(FigureType.Five, figure.type);
        CollectionAssert.AreEquivalent(new Card[] { table[0], table[2], table[3], table[4], hand[0] }, figure.cards);
    }

}
