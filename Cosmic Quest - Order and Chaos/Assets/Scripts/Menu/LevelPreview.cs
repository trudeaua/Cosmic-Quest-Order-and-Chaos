using UnityEngine;

public class LevelPreview : MonoBehaviour
{
    public GameObject pathToNextLevel;

    public ChaosVoid chaosVoid;

    [Header("Navigation")]
    public LevelPreview selectOnUp;
    public LevelPreview selectOnDown;
    public LevelPreview selectOnLeft;
    public LevelPreview selectOnRight;

    private bool selected = false;

    public void HighlightLevel()
    {
        selected = true;
    }

    public void UnhighlightLevel()
    {
        selected = false;
    }
}
