using UnityEngine;

public class VideoHelper : MonoBehaviour
{
    public static float QualityLevel { get; internal set; } = 0.5f;

    /// <summary>
    /// Set the quality level of the game
    /// </summary>
    /// <param name="level">Index of the quality level to play at</param>
    public static void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    /// <summary>
    /// Get a list of the names of all Unity quality levels
    /// </summary>
    /// <returns>A list of all quality levels</returns>
    public static string[] GetQualityLevels()
    {
        return QualitySettings.names;
    }
}