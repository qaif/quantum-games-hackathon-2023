using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qiskit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UsedGate
{
    public GateType type;
    public float thetaValue;
}

public class CardInHand
{
    public bool measured = false;
    public Card measuredValue = null;
    // This could be stateless probably
    public QuantumCard quantumCard = new QuantumCard();
    public Dictionary<int, List<UsedGate>> usedGates = new Dictionary<int, List<UsedGate>>();

    public RandomSelector rs = new RandomSelector();

    public CardInHand()
    {
        for (int i = 0; i < 6; i++)
        {
            usedGates[i] = new List<UsedGate>();
        }
    }

    public Card Measure()
    {
        SoundManager.Instance.Measure();
        measured = true;
        quantumCard.InitCircuit();

        int suiteIndex = 0;
        int rankIndex = 0;

        for (int i = 0; i < 6; i++)
        {
            usedGates[i].ForEach(gate => quantumCard.Apply(gate.type, i, gate.thetaValue));

            var probs = quantumCard.SimulateProbability(i);
            var value = rs.GetRandomElementIndex(probs);

            if (i < 2)
            {
                suiteIndex = (suiteIndex << 1) + value;
            }
            else
            {
                rankIndex = (rankIndex << 1) + value;
            }
        }

        var suite = CardExtensions.intToSuite[suiteIndex];
        var rank = CardExtensions.intToRank[rankIndex];

        measuredValue = new Card(suite, rank);
        return measuredValue;
    }

    public void Reset()
    {
        measured = false;
        measuredValue = null;
        for (int i = 0; i < 6; i++)
        {
            usedGates[i].Clear();
        }
    }
}

public class HumanPlayer : MonoBehaviour
{
    public PersistentState state;
    public Events events;

    int playerIndex;

    [HideInInspector] public int currentMoney;

    [Header("Actions")]
    public Button foldButton;
    public Button checkButton;
    public Button callButton;
    public Button raiseButton;

    [Header("Stats")]
    public TMP_Text moneyDisplay;
    public TMP_Text betDisplay;

    [Header("Cards")]
    public Image card1;
    public Image card2;

    CardInHand left = new CardInHand();
    CardInHand right = new CardInHand();

    [Header("Measure")]
    public Sprite smallCardSprite;

    public GameObject leftSymbol;
    public GameObject measureLeftButton;
    public GameObject transformLeftButton;

    public GameObject rightSymbol;
    public GameObject measureRightButton;
    public GameObject transformRightButton;

    [Header("Transform")]
    int currentlyModifiedCard;

    public TransformView transformView;

    public GameObject transformWindow;
    public GameObject gatesContainer;

    public GameObject xGatePrefab;
    public GameObject zGatePrefab;
    public GameObject yGatePrefab;
    public GameObject hGatePrefab;
    public GameObject sGatePrefab;
    public GameObject tGatePrefab;
    public GameObject ryGatePrefab;
    public GameObject rxGatePrefab;

    public QuantumCardDisplay transformedCardSource;
    public QuantumCardDisplay transformedCardResult;

    Game enteredGame;
    float thetaThisGame;

    void OnEnable()
    {
        events.transformViewUpdated += UpdateTransformView;
    }

    void OnDisable()
    {
        events.transformViewUpdated -= UpdateTransformView;
    }

    public void EnterGame(Game game, int playerIndex)
    {
        thetaThisGame = UnityEngine.Random.Range(1, 6) * 30;
        left.Reset();
        right.Reset();
        UpdateCards();
        this.playerIndex = playerIndex;
        enteredGame = game;
        enteredGame.betFromPlayerRequested += EnableBettingControls;
    }

    public void ExitGame(Game game)
    {
        enteredGame = null;
    }

    private void EnableBettingControls(Seat player, int currentMaxBet)
    {
        if (player.index != this.playerIndex) return;
        ActivateButtons();
        UpdateCards();

        if (player.currentMoney + player.currentBet > currentMaxBet)
        {
            raiseButton.interactable = true;
        }

        if (player.currentBet < currentMaxBet)
        {
            // Raise
            callButton.interactable = true;
            foldButton.interactable = true;
        }
        else
        {
            checkButton.interactable = true;
        }
    }

    private void DisableBettingControls()
    {
        foldButton.interactable = false;
        callButton.interactable = false;
        checkButton.interactable = false;
        raiseButton.interactable = false;
    }

    public void UpdateCards()
    {
        if (left.measured)
        {
            var c1 = left.measuredValue;
            var sprite = Resources.Load<Sprite>($"Images/Deck/{c1.rank}_{c1.suite}");
            card1.sprite = sprite;
        }
        else
        {
            card1.sprite = smallCardSprite;
        }

        if (right.measured)
        {
            var c2 = right.measuredValue;
            var sprite2 = Resources.Load<Sprite>($"Images/Deck/{c2.rank}_{c2.suite}");
            card2.sprite = sprite2;
        }
        else
        {
            card2.sprite = smallCardSprite;
        }

        ToggleButtonsVisibility();
    }

    public void UpdateMoney()
    {
        if (enteredGame == null) return;
        moneyDisplay.text = $"{enteredGame.players[playerIndex].currentMoney}";
    }

    public void UpdateBet()
    {
        if (enteredGame == null) return;
        betDisplay.text = $"{enteredGame.players[playerIndex].currentBet}";
    }

    public void Fold()
    {
        SoundManager.Instance.Click();
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, -1);
    }

    public void Check()
    {
        SoundManager.Instance.Click();
        DisableBettingControls();
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet);
    }

    public void Call()
    {
        SoundManager.Instance.Call();
        DisableBettingControls();
        var mySeat = enteredGame.players[playerIndex];
        var callAmount = Math.Min(enteredGame.currentMaxBet, mySeat.currentMoney + mySeat.currentBet);
        enteredGame.SubmitBet(this.playerIndex, callAmount);
    }

    public void Raise()
    {
        SoundManager.Instance.Call();
        DisableBettingControls();
        var raiseAmount = Math.Min(200, enteredGame.players[playerIndex].currentMoney);
        enteredGame.SubmitBet(this.playerIndex, enteredGame.currentMaxBet + raiseAmount);
    }

    void Update()
    {
        UpdateMoney();
        UpdateBet();
    }

    void ToggleButtonsVisibility()
    {
        leftSymbol.SetActive(!left.measured);
        measureLeftButton.SetActive(!left.measured);
        transformLeftButton.SetActive(!left.measured);

        rightSymbol.SetActive(!right.measured);
        measureRightButton.SetActive(!right.measured);
        transformRightButton.SetActive(!right.measured);
    }

    void ActivateButtons()
    {
        measureLeftButton.GetComponent<Button>().interactable = true;
        transformLeftButton.GetComponent<Button>().interactable = true;
        measureRightButton.GetComponent<Button>().interactable = true;
        transformRightButton.GetComponent<Button>().interactable = true;
    }

    void DeativateButtons()
    {
        measureLeftButton.GetComponent<Button>().interactable = false;
        transformLeftButton.GetComponent<Button>().interactable = false;
        measureRightButton.GetComponent<Button>().interactable = false;
        transformRightButton.GetComponent<Button>().interactable = false;
    }


    public void Measure(int cardIndex)
    {
        if (cardIndex == 0)
        {
            enteredGame.players[playerIndex].cards[0] = left.Measure();
        }
        else if (cardIndex == 1)
        {
            enteredGame.players[playerIndex].cards[1] = right.Measure();
        }
        else
        {
            Debug.Log("Invalid measured card index");
        }

        UpdateCards();
        ToggleButtonsVisibility();
    }

    GameObject GetGatePrefab(GateType type)
    {
        switch (type)
        {
            case GateType.H:
                return hGatePrefab;
            case GateType.X:
                return xGatePrefab;
            case GateType.Y:
                return yGatePrefab;
            case GateType.Z:
                return zGatePrefab;
            case GateType.S:
                return sGatePrefab;
            case GateType.T:
                return tGatePrefab;
            case GateType.RX:
                return rxGatePrefab;
            case GateType.RY:
                return ryGatePrefab;
        }
        return null;
    }

    void FillUsedGates()
    {
        CardInHand displayedCard = (currentlyModifiedCard == 0) ? left : right;

        var lines = FindObjectsOfType<DroppableLine>().OrderBy(line => -line.transform.position.y).ToArray();
        for (int i = 0; i < 6; i++)
        {
            int j = 0;
            foreach (var gate in displayedCard.usedGates[i])
            {
                var prefab = GetGatePrefab(gate.type);
                var gateObject = Instantiate(prefab, gatesContainer.transform);
                gateObject.GetComponent<LayoutElement>().ignoreLayout = true;
                gateObject.GetComponent<DraggableGate>().interactable = false;

                // Allocate equal space for each used gate
                float xPosition;
                if (displayedCard.usedGates[i].Count() == 1)
                {
                    xPosition = 560 + 360 / 2;
                }
                else
                {
                    xPosition = 560 + 360 / (displayedCard.usedGates[i].Count() - 1) * j;
                }

                gateObject.transform.position = new Vector3(xPosition, lines[i].transform.position.y, gateObject.transform.position.z);
                j++;
            }
        }
    }

    void FillAvailableGates()
    {
        foreach (Transform child in gatesContainer.transform)
        {
            Destroy(child.gameObject);
        }

        var availableGates = state.gates.ToDictionary(gate => gate.type, gate => gate.count);

        for (int i = 0; i < 6; i++)
        {
            left.usedGates[i].ForEach(gate => availableGates[gate.type] -= 1);
            right.usedGates[i].ForEach(gate => availableGates[gate.type] -= 1);
        }
        foreach (var gate in availableGates)
        {
            var prefab = GetGatePrefab(gate.Key);
            for (int i = 0; i < gate.Value; i++)
            {
                var gateObject = Instantiate(prefab, gatesContainer.transform);
                var dg = gateObject.GetComponent<DraggableGate>();
                dg.initialGatesPanel = gatesContainer.GetComponent<RectTransform>();
                dg.thetaValue = thetaThisGame;
                if (dg.theta != null) dg.theta.text = dg.thetaValue.ToString();
            }
        }
    }

    public void UpdateTransformView()
    {
        // Left card - pre modifications
        var card = (currentlyModifiedCard == 0) ? left : right;
        card.quantumCard.InitCircuit();
        transformedCardSource.UpdateCard(card.quantumCard, true);

        // Right card - after modifications
        List<DraggableGate> gates = FindObjectsOfType<DraggableGate>().OrderBy(gate => gate.transform.position.x).ToList();
        int newlyAppliedGates = 0;
        foreach (var gate in gates)
        {
            int i = gate.FindOverlapingLine();
            if (i == -1) continue;

            card.quantumCard.Apply(gate.type, i, gate.thetaValue);

            if (gate.interactable) newlyAppliedGates++;
        }
        transformedCardResult.UpdateCard(card.quantumCard, state.mode == Mode.Educational);
        transformView.UpdateRemainingGates(newlyAppliedGates);
    }

    public IEnumerator DelayedUpdateTransformView()
    {
        yield return null;
        UpdateTransformView();
    }

    public void OpenTransformWindow(int cardIndex)
    {
        transformWindow.SetActive(true);
        currentlyModifiedCard = cardIndex;
        FillAvailableGates();
        FillUsedGates();
        transformView.SetCardSymbol(currentlyModifiedCard);
        StartCoroutine(DelayedUpdateTransformView());
    }

    void SaveUsedGates()
    {
        List<DraggableGate> gates = FindObjectsOfType<DraggableGate>().OrderBy(gate => gate.transform.position.x).ToList();

        foreach (var gate in gates)
        {
            int i = gate.FindOverlapingLine();
            if (i == -1) continue;
            if (!gate.interactable) continue;

            var usedGates = (currentlyModifiedCard == 0) ? left.usedGates : right.usedGates;

            usedGates[i].Add(new UsedGate { type = gate.type, thetaValue = thetaThisGame });
        }
    }

    public void ApplyTransformWindow()
    {
        DeativateButtons();
        SaveUsedGates();
        CloseTransformWindow();
    }

    public void CloseTransformWindow()
    {
        transformWindow.SetActive(false);
    }
}
