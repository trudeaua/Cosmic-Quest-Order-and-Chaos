using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    #region Singleton
    public static PauseMenuController _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one pause menu controller should be in the scene!");
    }
    #endregion

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        // Set pause menu active
        // Switch player input action map to UI
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        // Set pause menu inactive
        // Switch player input action map to Player
    }
}
