using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public Table table;
    public string mainMenuScene = "MainMenu";

    public void TryAgain()
    {
        table.ResetLevel();
        this.gameObject.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
