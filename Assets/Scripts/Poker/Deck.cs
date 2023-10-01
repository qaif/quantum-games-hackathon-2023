using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Deck
{
    Card NextCard();
}

public class RandomDeck : Deck
{
    public Card NextCard()
    {
        return Card.Random();
    }
}
