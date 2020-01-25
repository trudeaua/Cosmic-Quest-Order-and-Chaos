using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private GameObject pauseMenu;
    private Selectable[] selectables;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        selectables = pauseMenu.GetComponentsInChildren<Selectable>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    public void PauseGame(EventSystem eventSystem)
    {
        Time.timeScale = 0;
        isPaused = true;
        // Set pause menu active
        pauseMenu.SetActive(true);
        if (selectables.Length <= 0)
        {
            Debug.LogError("No selectable objects found in Pause Menu Game Object");
            return;
        }
        eventSystem.firstSelectedGameObject = selectables[0].gameObject;
        eventSystem.SetSelectedGameObject(selectables[0].gameObject);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        // Set pause menu inactive
        pauseMenu.SetActive(false);
    }
}
