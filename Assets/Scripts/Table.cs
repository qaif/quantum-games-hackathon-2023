using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    public Image[] cardsOnTable;

    public HumanPlayer humanPlayer;
    public AIPlayer[] robotPlayers;

    public Sprite cardBack;

    [Header("After game screen")]
    public GameObject gameFinishedWindow;

    public TMP_Text title;

    public GameObject[] oponentResults;
    public GameObject playerResults;

    Game currentGame;
    int startingPlayer = -1;

    void StartNewRound()
    {
        var money = new List<int> { humanPlayer.currentMoney };
        var activeRobotPlayers = robotPlayers.Where(player => player.currentMoney > 0).ToList();

        money.AddRange(activeRobotPlayers.Select(player => player.currentMoney));

        startingPlayer = (startingPlayer + 1) % (1 + money.Count());

        currentGame = new Game(new RandomDeck(), money.ToArray(), startingPlayer);
        UpdateCardsDisplay();

        humanPlayer.EnterGame(currentGame, 0);
        for (int i = 0; i < activeRobotPlayers.Count(); i++)
        {
            activeRobotPlayers[i].EnterGame(currentGame, i + 1);
        }

        currentGame.newCardsOnBoard += UpdateCardsDisplay;
        currentGame.gameFinished += GameFinished;

        currentGame.Start();
    }

    void ShowResults(GameObject results, int moneyChange, Card[] cards)
    {
        GameObject moneyLabel = results.transform.Find("Money").gameObject;
        moneyLabel.GetComponent<TMP_Text>().text = moneyChange.ToString();

        GameObject cardsContainer = results.transform.Find("Cards").gameObject;
        int i = 0;
        foreach (Transform child in cardsContainer.transform)
        {
            cards[i].Display(child.GetComponent<Image>());
            i++;
        }
    }

    bool isWinner(int index)
    {
        return currentGame.GetWinningPlayers().Select(player => player.index).Contains(index);
    }

    void SetTitle()
    {
        var winners = currentGame.GetWinningPlayers();
        if (!isWinner(0))
        {
            title.text = "You lost!";
            return;
        }

        if (winners.Length > 1)
        {
            title.text = "A draw!";
            return;
        }

        title.text = "You won!";
    }

    void GameFinished()
    {
        var winners = currentGame.GetWinningPlayers();


        SetTitle();

        // Show player's results

        var playerSeat = currentGame.players[0];
        var playerMoneyDelta = playerSeat.currentMoney - humanPlayer.currentMoney;
        Figure playerFigure = Figures.DetectBestFigure(currentGame.cardsOnTable.ToArray(), playerSeat.cards.ToArray());
        humanPlayer.currentMoney = playerSeat.currentMoney;

        ShowResults(playerResults, playerMoneyDelta, playerFigure.Cards());

        // Show robots results
        // TODO : fix in case of lower number of active players
        for (int i = 0; i < 3; i++)
        {
            if (robotPlayers[i].currentMoney <= 0)
            {
                // Do not show
                oponentResults[i].SetActive(false);
                continue;
            }

            var seat = currentGame.players[i + 1];
            var moneyDelta = seat.currentMoney - robotPlayers[i].currentMoney;

            Figure f = Figures.DetectBestFigure(currentGame.cardsOnTable.ToArray(), seat.cards.ToArray());
            ShowResults(oponentResults[i], moneyDelta, f.Cards());

            robotPlayers[i].currentMoney = seat.currentMoney;
        }

        gameFinishedWindow.SetActive(true);
    }

    public void GoToNextGame()
    {
        // TODO: finishing the level
        foreach (var player in robotPlayers)
        {
            player.ExitGame(currentGame);
        }
        humanPlayer.ExitGame(currentGame);

        gameFinishedWindow.SetActive(false);
        StartNewRound();
    }

    void UpdateCardsDisplay()
    {
        int i = 0;

        foreach (var card in currentGame.cardsOnTable)
        {
            card.Display(cardsOnTable[i]);
            i++;
        }
        while (i < 5)
        {
            cardsOnTable[i].sprite = cardBack;
            i++;
        }
    }

    void Start()
    {
        StartNewRound();
    }
}
