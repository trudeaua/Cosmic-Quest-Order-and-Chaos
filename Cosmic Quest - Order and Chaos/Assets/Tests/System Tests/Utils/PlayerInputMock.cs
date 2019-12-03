using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputMock
{
    public readonly Gamepad Gamepad = null;
    public int PlayerNumber { get; }
    
    public PlayerInputMock(int playerNumber = 0)
    {
        PlayerNumber = playerNumber;
        ReadOnlyArray<Gamepad> devices = Gamepad.all;
        
        // Search input devices for the fake input
        foreach (Gamepad device in devices)
        {
            if (device.name == "MockGamepad" + PlayerNumber)
            {
                Gamepad = device;
                break;
            }
        }
        
        // if not found, add it and return it
        if (Gamepad == null)
        {
            Gamepad = InputSystem.AddDevice<Gamepad>("MockGamepad" + PlayerNumber);
        }
    }
    
    private void SetUpAndQueueEvent(InputEventPtr eventPtr, InputControl control, Vector2 val)
    {
        control.WriteValueIntoEvent(val, eventPtr);
        InputSystem.QueueEvent(eventPtr);
    }

    private void SetUpAndQueueEvent(InputEventPtr eventPtr, InputControl control, float val)
    {
        control.WriteValueIntoEvent(val, eventPtr);
        InputSystem.QueueEvent(eventPtr);
    }

    public void Press(InputControl control)
    {
        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr, control, 1f);
        
        InputSystem.Update();
    }
    
    public void Release(InputControl control)
    {
        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr, control, 0f);

        InputSystem.Update();
    }

    public void Press(InputControl control, Vector2 input)
    {
        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr, control, input);

        InputSystem.Update();
    }

    public void Release(InputControl control, Vector2 input)
    {
        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr, control, input);

        InputSystem.Update();
    }

    public void SetInputToMockGamepad(PlayerInput input)
    {
        // Change the input device to the Mock Gamepad
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        foreach (InputDevice d in devices)
        {
            if (d.name == "MockGamepad" + PlayerNumber)
            {
                InputDevice[] dev = new InputDevice[1];
                dev[0] = d;
                input.SwitchCurrentControlScheme("Gamepad", dev);
                break;
            }
        }
    }
}
