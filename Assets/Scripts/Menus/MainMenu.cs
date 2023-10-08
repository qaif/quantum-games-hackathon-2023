using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PersistentState state;

    public string gameplayScene = "Table";

    public GameObject playSelect;
    public GameObject helpSelect;

    public GameObject pokerRulesHelp;
    public GameObject cardHelp;
    public GameObject quantumHelp;

    public void Start()
    {
        SoundManager.Instance.Menu();
    }

    public void Play()
    {
        playSelect.SetActive(true);
    }

    public void Educational()
    {
        state.mode = Mode.Educational;
        SceneManager.LoadScene(gameplayScene);
    }

    public void Challenging()
    {
        state.mode = Mode.Challenging;
        SceneManager.LoadScene(gameplayScene);
    }

    public void HelpSelect()
    {
        helpSelect.SetActive(true);
    }

    public void CardHelp()
    {
        cardHelp.SetActive(true);
    }

    public void PokerHelp()
    {
        pokerRulesHelp.SetActive(true);
    }

    public void QuantumHelp()
    {
        quantumHelp.SetActive(true);
    }

    public void CloseHelp()
    {
        helpSelect.SetActive(false);
        pokerRulesHelp.SetActive(false);
        cardHelp.SetActive(false);
        quantumHelp.SetActive(false);
        Debug.Log("Called close");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
