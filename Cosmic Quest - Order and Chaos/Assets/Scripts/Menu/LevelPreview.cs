using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPreview : MonoBehaviour
{
    [SerializeField] private string levelSceneName;

    [Header("Navigation")]
    public LevelPreview selectOnUp;
    public LevelPreview selectOnDown;
    public LevelPreview selectOnLeft;
    public LevelPreview selectOnRight;

    /// <summary>
    /// Description: Get the name of the level preview's associated scene
    /// Rationale: Other classes should be able to access the scene name of the level preview
    /// </summary>
    /// <returns></returns>
    public string GetLevelName()
    {
        return levelSceneName;
    }

}
