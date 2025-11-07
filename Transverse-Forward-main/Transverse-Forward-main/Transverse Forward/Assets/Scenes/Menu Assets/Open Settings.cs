using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenSettings : MonoBehaviour
{
    //handles the opening of the pause and settings 

    public GameObject settingsMenu;
    public GameObject pauseMenu;
    private bool gamePaused;

    // Update is called once per frame
    void Update()
    {
        if (!(SceneManager.GetActiveScene().name.Equals("Title Screen")))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if ((settingsMenu.activeInHierarchy == true) && (gamePaused == true))
                {
                    closeSettings();
                } else
                {
                    pauseGame();
                }

            }

        }


    }

    public void openSettings()
    {
        if (settingsMenu.activeInHierarchy == false)
        {
            settingsMenu.SetActive(true);
        }
        else
        {
            closeSettings();
        }
    }

    public void closeSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void pauseGame()
    {
        if (pauseMenu.activeInHierarchy == false)
        {
            pauseMenu.SetActive(true);
            gamePaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1f;
        }
    }





}
