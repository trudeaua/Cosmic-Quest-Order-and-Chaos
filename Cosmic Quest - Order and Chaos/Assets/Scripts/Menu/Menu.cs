using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button[] buttons;
    private Button currentButton;
    
    protected void Start()
    {
        RefreshButtons();
    }

    private void RefreshButtons()
    {
        buttons = GetComponentsInChildren<Button>();
        if (buttons.Length > 0)
        {
            currentButton = buttons[0];
        }
    }

    private void OnMenuNavigate(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        Debug.Log("HI");
        float angle = Vector3.Angle(Vector3.up, direction);
        // up
        if (angle < 80 || angle > 280)
        {
            
        }
        // down
        else
        {

        }
    }
}