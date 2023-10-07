using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qiskit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Table : MonoBehaviour
{
    public PersistentState state;

    public Image[] cardsOnTable;

    public HumanPlayer humanPlayer;
    public AIPlayer[] robotPlayers;

    public Sprite cardBack;

    [Header("After game screen")]
    public GameObject gameFinishedWindow;

    public TMP_Text title;

    public GameObject[] oponentResults;
    public GameObject playerResults;

    [Header("After level screen")]
    public GameObject loseMenu;
    public GameObject winMenu;

    [Header("Levels")]
    public Level[] levels;
    int currentLevel = 0;

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

    bool IsWinner(int index)
    {
        return currentGame.GetWinningPlayersForPot(currentGame.currentMaxBet).Select(player => player.index).Contains(index);
    }

    void SetTitle()
    {
        var winners = currentGame.GetWinningPlayersForPot(currentGame.currentMaxBet);
        if (!IsWinner(0))
        {
            title.text = "You lost the round!";
            return;
        }

        if (winners.Length > 1)
        {
            title.text = "A draw!";
            return;
        }

        title.text = "You won the round!";
    }

    void GameFinished()
    {
        SetTitle();

        // Show player's results
        var playerSeat = currentGame.players[0];
        var playerMoneyDelta = playerSeat.currentMoney - humanPlayer.currentMoney;
        Figure playerFigure = Figures.DetectBestFigure(currentGame.cardsOnTable.ToArray(), playerSeat.cards.ToArray());
        humanPlayer.currentMoney = playerSeat.currentMoney;

        ShowResults(playerResults, playerMoneyDelta, playerFigure.Cards());

        // Show robots results
        for (int i = 0; i < 3; i++)
        {
            // TODO: proper handling for lower number of players
            if (!robotPlayers[i].IsPlayingInTheGame())
            {
                oponentResults[i].SetActive(false);
                continue;
            }

            var seat = currentGame.players[robotPlayers[i].playerIndex];

            if (seat.folded)
            {
                oponentResults[i].SetActive(false);
                continue;
            }

            oponentResults[i].SetActive(true);

            var moneyDelta = seat.currentMoney - robotPlayers[i].currentMoney;

            Figure f = Figures.DetectBestFigure(currentGame.cardsOnTable.ToArray(), seat.cards.ToArray());
            ShowResults(oponentResults[i], moneyDelta, f.Cards());

            robotPlayers[i].currentMoney = seat.currentMoney;
        }

        gameFinishedWindow.SetActive(true);
    }

    public void GoToNextGame()
    {
        foreach (var player in robotPlayers)
        {
            player.ExitGame(currentGame);
            if (player.currentMoney < 200)
            {
                player.gameObject.SetActive(false);
            }
        }

        humanPlayer.ExitGame(currentGame);
        gameFinishedWindow.SetActive(false);

        if (humanPlayer.currentMoney < 200)
        {
            loseMenu.SetActive(true);
            return;
        }

        state.moneyToSpend += humanPlayer.currentMoney;

        if (robotPlayers.Where(player => player.gameObject.activeSelf).Count() == 0)
        {
            winMenu.GetComponent<WinMenu>().Init(levels[currentLevel].gatesToBuy);
            winMenu.SetActive(true);
            return;
        }

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
        state.Reset();
        ResetLevel();
        StartNewRound();
    }

    public void ResetLevel()
    {
        levels[currentLevel].Initialize(this);
        StartNewRound();
    }

    public void GoToNextLevel()
    {
        currentLevel += 1;
        if (currentLevel > 4) currentLevel = 4;
        levels[currentLevel].Initialize(this);
        StartNewRound();
    }
}
