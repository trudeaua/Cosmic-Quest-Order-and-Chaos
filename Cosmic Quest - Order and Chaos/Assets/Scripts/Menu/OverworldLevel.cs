using UnityEngine;

public class OverworldLevel : MonoBehaviour
{
    public ChaosVoid chaosVoid;

    [Header("Navigation")]
    public OverworldLevel selectOnUp;
    public OverworldLevel selectOnUpLeft;
    public OverworldLevel selectOnUpRight;
    public OverworldLevel selectOnDown;
    public OverworldLevel selectOnDownLeft;
    public OverworldLevel selectOnDownRight;
    public OverworldLevel selectOnLeft;
    public OverworldLevel selectOnRight;

    private bool selected = false;

    public void Start()
    {
    }

    public void HighlightChaosVoid()
    {
        selected = true;
    }

    public void UnhighlightChaosVoid()
    {
        selected = false;
    }


}
