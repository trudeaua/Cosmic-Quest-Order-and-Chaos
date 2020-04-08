using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputMock
{
    public readonly Gamepad Gamepad = null;
    private int mockDeviceId;

    public int PlayerNumber { get; }
    private int[] DeviceIds { get; }
    private string ControlScheme { get; }

    /// <summary>
    /// Constructor for PlayerInputMock.
    /// Use for automating a player-controlled player.
    /// </summary>
    /// <param name="playerNumber">Players number</param>
    /// <param name="deviceId">Id of the players device</param>
    /// <param name="controlScheme">Control scheme of the players device</param>
    public PlayerInputMock(int playerNumber, int[] deviceIds, string controlScheme)
    {
        PlayerNumber = playerNumber;
        DeviceIds = deviceIds;
        ControlScheme = controlScheme;
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
        mockDeviceId = Gamepad.deviceId;
    }

    /// <summary>
    /// Constructor for PlayerInputMock.
    /// Use for automating a script-controlled player.
    /// </summary>
    /// <param name="playerNumber"></param>
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
        mockDeviceId = Gamepad.deviceId;
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

    public void ResetPlayerInput(PlayerInput input)
    {
        // Change the mock input device back to what it was originally
        ReadOnlyArray<InputDevice> devices = InputSystem.devices;
        InputDevice deviceToRemove = null;
        foreach (InputDevice d in devices)
        {
            if (d.deviceId == mockDeviceId)
            {
                deviceToRemove = d;
            }
        }
        InputDevice[] originalDevices = InputSystem.devices.Where(e => DeviceIds.Contains(e.deviceId)).ToArray();
        input.SwitchCurrentControlScheme(ControlScheme, originalDevices);
        // Remove the mock input device from the input system
        InputSystem.RemoveDevice(deviceToRemove);
    }
}
