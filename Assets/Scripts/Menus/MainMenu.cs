using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameplayScene = "Table";

    public void Start()
    {
        SoundManager.Instance.Menu();
    }

    public void Play()
    {
        SceneManager.LoadScene(gameplayScene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
