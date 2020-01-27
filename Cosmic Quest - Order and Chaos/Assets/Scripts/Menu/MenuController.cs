using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Singleton
    public static MenuController _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Only one menu controller should be in the scene!");
    }
    #endregion

    // Maintains the currently active menu in the menu canvas
    [SerializeField] protected GameObject activeMenu;

    // Maintains the menus that the player has navigated through
    protected Stack<GameObject> menuStack;

    protected Camera mainCamera;

    protected AudioSource musicSource;

    protected virtual void Start()
    {
        menuStack = new Stack<GameObject>();
        activeMenu.SetActive(true);
        menuStack.Push(activeMenu);
        FindCameraAndMusic();
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    /// <param name="menu">The menu to navigate to</param>
    public virtual void PushMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
        menuStack.Push(menu);
    }

    /// <summary>
    /// Navigate to the previous menu, if any
    /// </summary>
    public virtual void PopMenu()
    {
        if (menuStack.Count > 0)
        {
            activeMenu.SetActive(false);
            menuStack.Pop();
            activeMenu = menuStack.Peek();
            activeMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Gets the first selectable `GameObject` found in the specified menu
    /// </summary>
    /// <param name="menu">Game object to search for buttons in</param>
    protected virtual GameObject GetDefaultButton(GameObject menu)
    {
        if (menu == null)
        {
            return null;
        }
        Selectable btn = menu.GetComponentInChildren<Selectable>();
        if (btn)
        {
            return btn.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Sets the root of a multiplayer event system to a submenu that can only be controlled by that player
    /// </summary>
    /// <param name="eventSystem">A multiplayer event system that corresponds to `playerNumber`</param>
    protected virtual void SetPlayerRoot(MultiplayerEventSystem eventSystem)
    {
        eventSystem.playerRoot = activeMenu;
        GameObject defaultButton = GetDefaultButton(activeMenu);
        eventSystem.firstSelectedGameObject = defaultButton;
        eventSystem.SetSelectedGameObject(defaultButton);
    }

    /// <summary>
    /// Find the main camera and the audio source attached to it
    /// </summary>
    protected void FindCameraAndMusic()
    {
        mainCamera = FindObjectOfType<Camera>();
        musicSource = mainCamera.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Set the master volume
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetMasterVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetMasterVolume(value);
        UpdateMusicVolume();
    }

    /// <summary>
    /// Set the volume of music
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetMusicVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetMusicVolume(value);
        UpdateMusicVolume();
        
    }

    /// <summary>
    /// Set the volume of SFX
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetSfxVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetSfxVolume(value);
    }

    /// <summary>
    /// Set the volume of vocal audio
    /// </summary>
    /// <param name="slider">Selectable slider</param>
    public void SetVoiceVolume(Slider slider)
    {
        float value = slider.normalizedValue;
        AudioHelper.SetVoiceVolume(value);
    }

    /// <summary>
    /// Update any playing music
    /// </summary>
    protected void UpdateMusicVolume()
    {
        if (musicSource != null)
        {
            musicSource.volume = AudioHelper.GetAudioModifier(AudioHelper.EntityAudioClip.AudioType.Music);
            Debug.Log(musicSource.volume);
        }
    }
}