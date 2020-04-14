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
    /// Set the anti aliasing level
    /// </summary>
    /// <param name="level">Index of the anti aliasing level as found under the project settings</param>
    public static void SetAntiAliasingLevel(int level)
    {
        QualitySettings.antiAliasing = level;
    }

    /// <summary>
    /// Get a list of the names of all Unity quality levels
    /// </summary>
    /// <returns>A list of all quality levels</returns>
    public static string[] GetQualityLevels()
    {
        return QualitySettings.names;
    }

    /// <summary>
    /// Get the current quality level
    /// </summary>
    /// <returns>The index of the quality level in the project settings</returns>
    public static int GetCurrentQualityLevel()
    {
        return QualitySettings.GetQualityLevel();
    }

    /// <summary>
    /// Get the current anti aliasing level
    /// </summary>
    /// <returns>The index of the current anti aliasing level in the project settings</returns>
    public static int GetCurrentAntiAliasingLevel()
    {
        return QualitySettings.antiAliasing;
    }

    /// <summary>
    /// Get all allowed anti aliasing levels
    /// </summary>
    /// <returns>An array of all allowed anti aliasing levels</returns>
    public static int[] GetAntiAliasingLevels()
    {
        int[] levels = { 0, 2, 4, 8 };
        return levels;
    }

    /// <summary>
    /// Is the current game instance running in full screen mode?
    /// </summary>
    /// <returns></returns>
    public static bool isFullscreen()
    {
        return Screen.fullScreen;
    }

    /// <summary>
    /// Set the screen mode of the game
    /// </summary>
    /// <param name="isFullScreen">Should the game be full screen?</param>
    public static void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}