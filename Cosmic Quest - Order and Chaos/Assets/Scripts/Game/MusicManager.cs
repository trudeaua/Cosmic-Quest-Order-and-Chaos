using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Serializable]
    public struct AudioLoop 
    {
        public AudioClip clip;
        public float loopLength;
        public float loopThreshold;
    }
    public AudioLoop PlayingMusic;
    public AudioLoop BossFightMusic;
    public AudioLoop GameOverMusic;
    public AudioLoop MenuMusic;
    public AudioLoop PausedMusic;
    public AudioLoop LoadingMusic;
    private AudioLoop CurrentAudioLoop;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CurrentAudioLoop = PlayingMusic;
        GameManager.Instance.onStateChange.AddListener(UpdateMusic);
        UpdateMusic();
    }

    private void Update()
    {
        if (CurrentAudioLoop.loopLength > 0 && CurrentAudioLoop.loopThreshold > 0)
        {
            if (audioSource.timeSamples > CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency)
            {
                Debug.Log(audioSource.timeSamples);
                audioSource.timeSamples -= Mathf.RoundToInt(CurrentAudioLoop.loopLength * CurrentAudioLoop.clip.frequency);
            }
        }
    }

    private void UpdateMusic()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Menu:
                CurrentAudioLoop = MenuMusic;
                break;
            case GameManager.GameState.Paused:
                CurrentAudioLoop = PausedMusic;
                break;
            case GameManager.GameState.Loading:
                CurrentAudioLoop = LoadingMusic;
                break;
            case GameManager.GameState.Playing:
                CurrentAudioLoop = PlayingMusic;
                break;
            case GameManager.GameState.BossFight:
                CurrentAudioLoop = BossFightMusic;
                break;
            case GameManager.GameState.GameOver:
                CurrentAudioLoop = GameOverMusic;
                break;
            default:
                break;
        }
        audioSource.clip = CurrentAudioLoop.clip;
        audioSource.timeSamples = Mathf.RoundToInt(CurrentAudioLoop.loopThreshold * CurrentAudioLoop.clip.frequency);
        audioSource.Play();
    }
}
