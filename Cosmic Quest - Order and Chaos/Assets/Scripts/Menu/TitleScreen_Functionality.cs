using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen_Functionality : MonoBehaviour
{
    public void StartTutorial()
    {
        // TODO: create Tutorial Scene and order it after MenuStaging scene
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

