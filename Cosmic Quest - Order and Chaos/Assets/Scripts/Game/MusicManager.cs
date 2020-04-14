using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Only one music manager should be in the scene!");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion
    public AudioHelper.EntityAudioClip PlayingMusic;
    public AudioHelper.EntityAudioClip BossFightMusic;
    public AudioHelper.EntityAudioClip GameOverMusic;
    public AudioHelper.EntityAudioClip MenuMusic;
    public AudioHelper.EntityAudioClip PausedMusic;
    public AudioHelper.EntityAudioClip LoadingMusic;
    private AudioHelper.EntityAudioClip CurrentAudioLoop;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CurrentAudioLoop = PlayingMusic;
        // Listen for changes in game state
        GameManager.Instance.onStateChange.AddListener(UpdateMusic);
        UpdateMusic();
        PlayMusic();
    }

    private void Update()
    {
        if (CurrentAudioLoop.loopLength > 0 && CurrentAudioLoop.loopThreshold > 0)
        {
            // This allows a section of music to be looped - it essentially trims the currently playing audio
            if (audioSource.timeSamples > CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency)
            {
                audioSource.timeSamples -= Mathf.RoundToInt(CurrentAudioLoop.loopLength * CurrentAudioLoop.clip.frequency);
            }
        }
    }

    /// <summary>
    /// Description: Stops the playing music
    /// Rationale: Sometimes there should not be music playing
    /// </summary>
    public void StopMusic()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// Description: Plays the audio source
    /// Rationale: Need to play music if it's been reassigned or stopped
    /// </summary>
    public void PlayMusic()
    {
        audioSource.clip = CurrentAudioLoop.clip;
        audioSource.pitch = CurrentAudioLoop.pitch;
        audioSource.volume = CurrentAudioLoop.volume * AudioHelper.GetAudioModifier(CurrentAudioLoop.type);
        audioSource.timeSamples = Mathf.RoundToInt(CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency);
        audioSource.Play();
    }

    /// <summary>
    /// Description: Updates the music according to the game state
    /// Rationale: Different game states should have different music
    /// </summary>
    private void UpdateMusic()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Menu:
                break;
            case GameManager.GameState.Paused:
                break;
            case GameManager.GameState.Loading:
                break;
            case GameManager.GameState.Playing:
                CurrentAudioLoop = PlayingMusic;
                break;
            case GameManager.GameState.BossFight:
                CurrentAudioLoop = BossFightMusic;
                break;
            case GameManager.GameState.GameOver:
                break;
            default:
                break;
        }
    }
}
