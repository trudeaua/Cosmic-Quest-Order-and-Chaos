using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    [System.Serializable]
    public class EntityAudioClip
    {
        public enum AudioType
        {
            Sfx,
            Vocal,
            Music
        }
        [Tooltip("Pitch of the audio (affects tempo as well)")]
        public float pitch;
        [Tooltip("How loud the audio should be")]
        public float volume;
        [Tooltip("Audio clip file")]
        public AudioClip clip;
        [Tooltip("Is the audio vocal or from a weapon?")]
        public AudioType type;
        [Tooltip("Time in seconds until audio plays")]
        public float delay;
        [Tooltip("Should the audio loop?")]
        public bool loop;
    }

    public static float MasterVolume { get; internal set; } = 0.5f;
    public static float MusicVolume { get; internal set; } = 0.5f;
    public static float SfxVolume { get; internal set; } = 0.5f;
    public static float VoiceVolume { get; internal set; } = 0.5f;

    /// <summary>
    /// Plays an audio clip that overlaps with currently playing audio clips from the same audio source.<br/>
    /// <b>Will not loop audio!!!</b>
    /// </summary>
    /// <param name="source">Audio source to play from</param>
    /// <param name="entityAudio">Entity audio clip to play</param>
    /// <returns>An IEnumerator</returns>
    public static IEnumerator PlayAudioOverlap(AudioSource source, EntityAudioClip entityAudio)
    {
        if (entityAudio.delay > 0)
        {
            yield return new WaitForSeconds(entityAudio.delay);
        }
        
        source.pitch = entityAudio.pitch;
        source.volume = entityAudio.volume * GetAudioModifier(entityAudio.type);
        source.loop = entityAudio.loop;
        source.PlayOneShot(entityAudio.clip);
    }

    /// <summary>
    /// Plays an audio clip that interrupts the currently playing audio clip(s) from the given audio source.<br/>
    /// <b>Use this in combination with AudioHelper.StopAudio() if you have a looping audio clip!!!</b>
    /// </summary>
    /// <param name="source">Audio source to play from</param>
    /// <param name="entityAudio">Entity audio clip to play</param>
    /// <returns>An IEnumerator</returns>
    public static IEnumerator PlayAudio(AudioSource source, EntityAudioClip entityAudio)
    {
        if (entityAudio.delay > 0)
        {
            yield return new WaitForSeconds(entityAudio.delay);
        }
        source.pitch = entityAudio.pitch;
        source.volume = entityAudio.volume * GetAudioModifier(entityAudio.type);
        source.loop = entityAudio.loop;
        source.clip = entityAudio.clip;
        source.Play();
    }

    /// <summary>
    /// Stops the given entity audio clip from playing
    /// </summary>
    /// <param name="entityAudio">Entity audio clip to stop playing</param>
    public static void StopAudio(AudioSource source)
    {
        source.Stop();
    }

    /// <summary>
    /// Set the master volume level
    /// </summary>
    /// <param name="value">Value to set the master volume to (bewteen 0 and 1)</param>
    public static void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Max(0, value);
    }

    /// <summary>
    /// Set the music volume level
    /// </summary>
    /// <param name="value">Value to set the music volume to (bewteen 0 and 1)</param>
    public static void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Max(0, value);
    }

    /// <summary>
    /// Set the sfx volume level
    /// </summary>
    /// <param name="value">Value to set the sfx volume to (bewteen 0 and 1)</param>
    public static void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Max(0, value);
    }

    /// <summary>
    /// Set the voice volume level
    /// </summary>
    /// <param name="value">Value to set the voice volume to (bewteen 0 and 1)</param>
    public static void SetVoiceVolume(float value)
    {
        VoiceVolume = Mathf.Max(0, value);
    }

    /// <summary>
    /// Set the audio speaker mode
    /// </summary>
    /// <param name="mode">Mode to set the speaker mode to</param>
    public static void SetAudioSpeakerMode(AudioSpeakerMode mode)
    {
        AudioSettings.speakerMode = mode;
    }

    /// <summary>
    /// Get a list of all allowed speaker modes
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<string, AudioSpeakerMode>> GetAudioSpeakerModes()
    {
        List<KeyValuePair<string, AudioSpeakerMode>> modes = new List<KeyValuePair<string, AudioSpeakerMode>>();
        modes.Add(new KeyValuePair<string, AudioSpeakerMode>("Mono", AudioSpeakerMode.Mono));
        modes.Add(new KeyValuePair<string, AudioSpeakerMode>("Stereo", AudioSpeakerMode.Stereo));
        modes.Add(new KeyValuePair<string, AudioSpeakerMode>("Surround", AudioSpeakerMode.Surround));
        return modes;
    }

    /// <summary>
    /// Get the current speaker mode
    /// </summary>
    /// <returns>The current speaker mode</returns>
    public static AudioSpeakerMode GetCurrentSpeakerMode()
    {
        return AudioSettings.speakerMode;
    }

    /// <summary>
    /// Get a modifier value that represents the volume level of the specified audio type adjusted with the master volume level
    /// </summary>
    /// <param name="audioType">Type of the audio clip</param>
    /// <returns>A modifier value (between 0 and 1)</returns>
    public static float GetAudioModifier(EntityAudioClip.AudioType audioType)
    {
        float audioModifier = MasterVolume;
        switch (audioType)
        {
            case EntityAudioClip.AudioType.Music:
                audioModifier *= MusicVolume;
                break;
            case EntityAudioClip.AudioType.Sfx:
                audioModifier *= SfxVolume;
                break;
            case EntityAudioClip.AudioType.Vocal:
                audioModifier *= VoiceVolume;
                break;
            default:
                break;
        }
        return audioModifier;
    }
}