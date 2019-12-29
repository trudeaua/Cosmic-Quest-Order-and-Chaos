using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO: Implement conotroller support for the menu
public class DisplayDescriptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Description; // Display description of highligthed selection when mouse hovers over screen

    // Mouse Support
    // Enable backstory text when character is highlighted  
    public void OnPointerEnter(PointerEventData eventData)
    {
        Description.SetActive(true);
    }   

    // Disable backstory text when character is not highlighted anymore
    public void OnPointerExit(PointerEventData eventData)
    {
        Description.SetActive(false);
    }   
}