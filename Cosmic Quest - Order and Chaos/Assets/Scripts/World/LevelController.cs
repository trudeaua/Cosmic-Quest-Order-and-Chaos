using UnityEngine;

public class LevelController : MonoBehaviour
{
    public ChaosVoid chaosVoid;

    public void Clear()
    {
        LevelManager.Instance.ClearChaosVoid(chaosVoid);
    }
}
