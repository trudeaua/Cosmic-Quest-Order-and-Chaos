using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPreview : MonoBehaviour
{
    public string levelSceneName;

    [Header("Navigation")]
    public LevelPreview selectOnUp;
    public LevelPreview selectOnDown;
    public LevelPreview selectOnLeft;
    public LevelPreview selectOnRight;
    
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
