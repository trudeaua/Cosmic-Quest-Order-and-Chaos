using UnityEngine;

public class LevelPreview : MonoBehaviour
{
    [SerializeField] private string levelSceneName;

    [Header("Navigation")]
    public LevelPreview selectOnUp;
    public LevelPreview selectOnDown;
    public LevelPreview selectOnLeft;
    public LevelPreview selectOnRight;

    private bool selected = false;

    /// <summary>
    /// Description: Get the name of the level preview's associated scene
    /// Rationale: Other classes should be able to access the scene name of the level preview
    /// </summary>
    /// <returns></returns>
    public string GetLevelName()
    {
        return levelSceneName;
    }

    public void HighlightLevel()
    {
        selected = true;
    }

    public void UnhighlightLevel()
    {
        selected = false;
    }
}
