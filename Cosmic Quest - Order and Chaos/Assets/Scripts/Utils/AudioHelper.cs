using System.Collections;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    // Audio clip to play along with some parameters
    [System.Serializable]
    public class EntityAudioClip
    {
        public enum AudioType
        {
            Weapon,
            Vocal
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
        source.volume = entityAudio.volume;
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
        source.volume = entityAudio.volume;
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
}