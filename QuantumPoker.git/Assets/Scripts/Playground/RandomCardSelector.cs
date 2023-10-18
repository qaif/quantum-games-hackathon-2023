using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCardSelector : MonoBehaviour
{
    public Image cardDisplay;

    // Update is called once per frame
    public void RandomizeCard()
    {
        Card card = Card.Random();

        Debug.Log($"Fetching sprite: Images/Deck/{card.rank}_{card.suite}");

        var sprite = Resources.Load<Sprite>($"Images/Deck/{card.rank}_{card.suite}");

        cardDisplay.sprite = sprite;
    }
}
