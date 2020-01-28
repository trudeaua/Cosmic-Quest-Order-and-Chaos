using System.Collections;
using UnityEngine;

public class VideoHelper : MonoBehaviour
{
    public static float QualityLevel { get; internal set; } = 0.5f;

    public static void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public static string[] GetQualityLevels()
    {
        return QualitySettings.names;
    }
}