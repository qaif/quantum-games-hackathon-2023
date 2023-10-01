using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    List<Card> cardsOnTable;

    void DealCard()
    {
        cardsOnTable.Add(deck.NextCard());
    }

    void Flop()
    {
        for (int i = 0; i < 3; i++)
        {
            DealCard();
        }
    }

    void Turn()
    {
        DealCard();
    }

    void River()
    {
        DealCard();
    }

    Deck deck;

    public Game(Deck deck)
    {
        this.deck = deck;
    }
}
