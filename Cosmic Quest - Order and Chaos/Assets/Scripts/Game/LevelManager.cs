using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one level manager should be in the scene!");
    }
    #endregion
    
    // List of all chaos voids
    public ChaosVoid[] chaosVoids;

    public void StartNewGame(bool tutorial)
    {
        // Starts a new game
    }

    public void LoadGame(string saveData)
    {
        // Loads a saved game
    }
}
