using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPokerLogic : MonoBehaviour
{
    Game game;

    public Image[] cardDisplays;
    public HumanPlayer humanPlayer;
    List<AIPlayer> players = new List<AIPlayer>();

    // // Start is called before the first frame update
    // void Start()
    // {
    //     var initialMoney = new int[4] { 1000, 1000, 1000, 1000 };
    //     game = new Game(new RandomDeck(), initialMoney, 0);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         var player = new AIPlayer();
    //         player.EnterGame(game, i);
    //         players.Add(player);
    //     }

    //     humanPlayer.EnterGame(game, 3);

    //     game.newCardsOnBoard += UpdateCardsDisplay;
    //     game.gameFinished += GameFinished;

    //     game.Start();
    // }

    // void GameFinished()
    // {
    //     Debug.Log($"Game finished");
    //     var winners = game.GetWinningPlayers();
    //     foreach (var winner in winners)
    //     {
    //         Debug.Log($"Winner: {winner.index}");
    //     }
    //     Debug.Log("Money after the round:");
    //     foreach (var player in game.players)
    //     {
    //         Debug.Log($"Player {player.index}: {player.currentMoney}");
    //     }

    //     players.ForEach(player => player.ExitGame(game));
    //     humanPlayer.ExitGame(game);
    // }

    // void UpdateCardsDisplay()
    // {
    //     int i = 0;
    //     foreach (var card in game.cardsOnTable)
    //     {
    //         card.Display(cardDisplays[i]);
    //         i++;
    //     }
    // }
}
