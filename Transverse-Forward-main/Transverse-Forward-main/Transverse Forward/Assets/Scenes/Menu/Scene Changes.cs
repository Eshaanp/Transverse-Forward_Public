using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanges : MonoBehaviour
{
    //used to handle scene changes and menu ui interactions
    public GameObject optionsCollection;
    public VolumeSettings volumeSettings;
    public GameObject eventSystem;
    public GameObject optionController;

    public void Update()
    {
        //opens or closes options menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsCollection.activeInHierarchy == false)
            {
                openSettings();
            } else
            {
                optionsCollection.SetActive(false);
                Time.timeScale = 1f;
            }
       
        }
    }
    public void startGame()
    {
        //when play button is pressed, Level 1 is loaded and options menu is moved to level 1
        moveSettings("Testing");
    }

    public void quitGame()
    {
        //quits application when quit button on main menu or pause menu is pressed
        Debug.Log("Application Quit!");
        Application.Quit();
    }

    public void openSettings()
    {
        //when settings button is pressed, opens options menu
        optionsCollection.SetActive(true);
        Time.timeScale = 0f;
    }

    public void moveSettings(string sceneName)
    {
        //used to move to a new scene
        //loads the scene, and moves the options menu object collection to the new scene
        // USE THIS TO LOAD NEW SCENES (otherwise settings will not be moved to and fro)
        DontDestroyOnLoad(optionsCollection.gameObject);
        DontDestroyOnLoad(eventSystem.gameObject);
        DontDestroyOnLoad(optionController.gameObject);
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(optionsCollection, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(eventSystem, SceneManager.GetSceneByName(sceneName)); 
        SceneManager.MoveGameObjectToScene(optionController, SceneManager.GetSceneByName(sceneName));

    }


}
