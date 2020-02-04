using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    #region Singleton
    public static LevelsController Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one levels controller should be in the scene!");
    }
    #endregion

    public GameObject cursor;
    private LevelPreview[] levelPreviews;
    private LevelPreview currentlySelected;
    // Start is called before the first frame update
    void Start()
    {
        levelPreviews = GetComponentsInChildren<LevelPreview>();
        if (levelPreviews.Length > 0)
        {
            currentlySelected = levelPreviews[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateUp()
    {
        if (currentlySelected.selectOnUp != null)
        {
            currentlySelected = currentlySelected.selectOnUp;
            MoveCursor();
        }
    }

    public void NavigateDown()
    {
        if (currentlySelected.selectOnDown != null)
        {
            currentlySelected = currentlySelected.selectOnDown;
            MoveCursor();
        }
    }

    public void NavigateLeft()
    {
        if (currentlySelected.selectOnLeft != null)
        {
            currentlySelected = currentlySelected.selectOnLeft;
            MoveCursor();
        }
    }

    public void NavigateRight()
    {
        if (currentlySelected.selectOnRight != null)
        {
            currentlySelected = currentlySelected.selectOnRight;
            MoveCursor();
        }
    }

    public void SelectLevel()
    {
        StartCoroutine(GameSceneManager.Instance.LoadYourAsyncScene(currentlySelected.levelSceneName));
    }

    private void MoveCursor()
    {
        cursor.transform.position = currentlySelected.transform.position;
    }
}
