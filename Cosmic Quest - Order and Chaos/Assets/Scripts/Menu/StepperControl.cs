using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class StepperControl : MonoBehaviour
{
    public enum StepperProperty { 
        Character,
        Class
    }
    public int playerNumber;
    public StepperProperty property;
    public GameObject playerContainer;
    private string[] classNames;
    private string[] characterNames;
    private int selectedIndex = 0;
    private Button btn;
    private TextMeshProUGUI gui;
    private string[] currentPropertyNames;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn = GetComponent<Button>();
        gui = btn.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        characterNames = PlayerManager.Instance.GetCharacterNames();
        classNames = PlayerManager.Instance.GetClassNames();

        switch (property)
        {
            case StepperProperty.Character:
                currentPropertyNames = characterNames;
                break;
            case StepperProperty.Class:
                currentPropertyNames = classNames;
                break;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        switch (property)
        {
            case StepperProperty.Character:
                gui.text = characterNames[selectedIndex];
                break;
            case StepperProperty.Class:
                gui.text = classNames[selectedIndex];
                break;
        }

        RefreshPlayer();

        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        List<EventTrigger.Entry> triggers = eventTrigger.triggers;
        EventTrigger.Entry clickEventHandler = triggers.FirstOrDefault(
            t => t.eventID == EventTriggerType.Move
        );
        if (clickEventHandler == null)
        {
            clickEventHandler = new EventTrigger.Entry();
            clickEventHandler.eventID = EventTriggerType.Move;
            triggers.Add(clickEventHandler);
            eventTrigger.triggers = triggers;
        }
        clickEventHandler.callback.AddListener(OnMove);
    }

    private void OnEnable()
    {
        UpdatePlayer();
    }

    private void UpdateText()
    {
        if (currentPropertyNames.Length > selectedIndex)
        {
            gui.text = currentPropertyNames[selectedIndex];
        }
    }

    private void UpdatePlayer()
    {
        switch (property)
        {
            case StepperProperty.Character:
                SetCharacter();
                break;
            case StepperProperty.Class:
                SetClass();
                break;
        }
    }

    private void SetCharacter()
    {
        PlayerManager.Instance.AssignCharacterChoice(playerNumber, selectedIndex);
    }

    private void SetClass()
    {
        PlayerManager.Instance.AssignPrefab(playerNumber, selectedIndex);
    }

    private void RefreshPlayer()
    {
        PlayerStatsController playerStats = playerContainer.GetComponentInChildren<PlayerStatsController>();
        // Remove old preview
        if (playerStats != null)
        {
            Destroy(playerStats.gameObject);
        }
        else
        {
            SetCharacter();
            SetClass();
        }
        UpdatePlayer();
        GameObject playerInstance = PlayerManager.Instance.InstantiatePlayer(playerNumber);
        playerInstance.transform.parent = playerContainer.transform;

        // Transform the player instance so it looks nice on screen
        playerInstance.transform.localPosition = new Vector3(0, -60, -40);
        playerInstance.transform.Rotate(new Vector3(0, 180, 0));
        playerInstance.transform.localScale = new Vector3(75, 75, 75);

        // Turn off components so the player is simply displayed and can't be controlled
        playerInstance.GetComponent<Collider>().enabled = false;
        playerInstance.GetComponent<PlayerInput>().enabled = false;
        playerInstance.GetComponent<PlayerInteractionController>().enabled = false;
        playerInstance.GetComponent<EntityStatsController>().SetSpawn(false);
        playerInstance.GetComponent<EntityCombatController>().enabled = false;
        playerInstance.GetComponentInChildren<StatBar>().gameObject.SetActive(false);
        playerInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void OnMove(BaseEventData data)
    {
        if (data is AxisEventData)
        {
            AxisEventData axisEventData = data as AxisEventData;
            if (Mathf.Abs(axisEventData.moveVector.x) > Mathf.Abs(axisEventData.moveVector.y))
            {
                selectedIndex += axisEventData.moveVector.x > 0 ? 1 : -1;
                if (selectedIndex < 0)
                {
                    selectedIndex = currentPropertyNames.Length - 1;
                }
                else if (selectedIndex > currentPropertyNames.Length - 1)
                {
                    selectedIndex = 0;
                }
                UpdateText();
                RefreshPlayer();
            }
        }
    }
}
