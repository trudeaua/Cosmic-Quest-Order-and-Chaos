using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputMock
{
    public readonly Gamepad Gamepad = null;

    public enum MockInput
    {
        LeftStick,
        RightStick,
        RightShoulder,
        RightTrigger,
        ButtonNorth,
        ButtonSouth
    }
    
    public PlayerInputMock()
    {
        ReadOnlyArray<Gamepad> devices = Gamepad.all;
        // Search input devices for the fake input
        foreach (Gamepad device in devices)
        {
            if (device.name == "MockGamepad")
            {
                Gamepad = device;
                break;
            }
        }
        // if not found, add it and return it
        if (Gamepad == null)
        {
            Gamepad = InputSystem.AddDevice<Gamepad>("MockGamepad");
        }
    }
    
    public void Set(MockInput control, Vector2 input)
    {
        if (input.sqrMagnitude > 1f)
            throw new Exception("Input vector must have a magnitude no greater than 1!");
        using (StateEvent.From(Gamepad, out var eventPtr))
        {
            switch (control)
            {
                case MockInput.LeftStick:
                    Gamepad.leftStick.WriteValueIntoEvent(input, eventPtr);
                    break;
                case MockInput.RightStick:
                    Gamepad.rightStick.WriteValueIntoEvent(input, eventPtr);
                    break;
                default:
                    throw new Exception("Invalid gamepad input control");
            }
            
            InputSystem.QueueEvent(eventPtr);
            InputSystem.Update();
        }
    }

    public void Press(MockInput control)
    {
        GamepadState newState;
        
        switch (control)
        {
            case MockInput.RightShoulder:
                newState = new GamepadState { buttons = (uint)GamepadButton.RightShoulder };
                break;
            default:
                throw new Exception("Invalid gamepad input control!");
        }
        
        InputSystem.QueueStateEvent(Gamepad, newState);
        InputSystem.Update();
    }

    public void Press(InputControl control)
    {
        void SetUpAndQueueEvent(InputEventPtr eventPtr)
        {
            control.WriteValueIntoEvent(1f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }

        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr);
        
        InputSystem.Update();
    }
    
    public void Release(InputControl control)
    {
        void SetUpAndQueueEvent(InputEventPtr eventPtr)
        {
            control.WriteValueIntoEvent(0f, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }

        using (DeltaStateEvent.From(control, out var eventPtr))
            SetUpAndQueueEvent(eventPtr);
        
        InputSystem.Update();
    }
}
