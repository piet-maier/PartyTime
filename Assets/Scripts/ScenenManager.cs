using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenenManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject highscoreMenu;

   

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GoToOptions()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void GoToHighscore()
    {
        mainMenu.SetActive(false);
        highscoreMenu.SetActive(true);
    }

    public void GoToMainMenu()
    {
        highscoreMenu.SetActive(false);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();                 //Erst bei der Exe Datei ausführbar
        Debug.Log("Quitting Game...");
    }
}
