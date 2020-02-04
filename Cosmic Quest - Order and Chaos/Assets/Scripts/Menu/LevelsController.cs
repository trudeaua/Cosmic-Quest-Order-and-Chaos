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
    public Camera camera;
    private LevelPreview[] levelPreviews;
    private LevelPreview currentlySelected;
    // Start is called before the first frame update
    void Start()
    {
        levelPreviews = GetComponentsInChildren<LevelPreview>();
        if (levelPreviews.Length > 0)
        {
            currentlySelected = levelPreviews[0];
            StartCoroutine(MoveCursor(0.3f));
            StartCoroutine(MoveCamera(0.3f));
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
            StartCoroutine(MoveCursor(0.3f));
            StartCoroutine(MoveCamera(0.3f));
        }
    }

    public void NavigateDown()
    {
        if (currentlySelected.selectOnDown != null)
        {
            currentlySelected = currentlySelected.selectOnDown;
            StartCoroutine(MoveCursor(0.3f));
            StartCoroutine(MoveCamera(0.3f));
        }
    }

    public void NavigateLeft()
    {
        if (currentlySelected.selectOnLeft != null)
        {
            currentlySelected = currentlySelected.selectOnLeft;
            StartCoroutine(MoveCursor(0.3f));
            StartCoroutine(MoveCamera(0.3f));
        }
    }

    public void NavigateRight()
    {
        if (currentlySelected.selectOnRight != null)
        {
            currentlySelected = currentlySelected.selectOnRight;
            StartCoroutine(MoveCursor(0.3f));
            StartCoroutine(MoveCamera(0.3f));
        }
    }

    public void SelectLevel()
    {
        StartCoroutine(GameSceneManager.Instance.LoadYourAsyncScene(currentlySelected.levelSceneName));
    }

    private IEnumerator MoveCursor(float time)
    {
        //cursor.transform.position = currentlySelected.transform.position;
        float elapsed = 0;
        while (elapsed < time)
        {
            float newX = Mathf.Lerp(cursor.transform.position.x, currentlySelected.transform.position.x, elapsed / time);
            cursor.transform.position = new Vector3(newX, cursor.transform.position.y, cursor.transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveCamera(float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            float newX = Mathf.Lerp(camera.transform.position.x, currentlySelected.transform.position.x, elapsed / time);
            camera.transform.position = new Vector3(newX, camera.transform.position.y, camera.transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
