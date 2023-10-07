using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PersistentState state;

    public string gameplayScene = "Table";
    public GameObject playSelect;

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

    public void Exit()
    {
        Application.Quit();
    }
}
