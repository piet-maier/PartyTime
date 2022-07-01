using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject canvas;

    public static bool gameIsPaused;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            gameIsPaused = !gameIsPaused;
            Pause();
        }
    }
    public void Pause()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            canvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            canvas.SetActive(false);
        }
    }
}
