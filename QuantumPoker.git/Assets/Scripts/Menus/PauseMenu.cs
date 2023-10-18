using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuWindow;

    public void Toggle()
    {
        if (menuWindow.activeSelf)
        {
            menuWindow.SetActive(false);
        }
        else
        {
            menuWindow.SetActive(true);
        }
    }

    public void Continue()
    {
        Toggle();
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Toggle();
        }
    }
}
