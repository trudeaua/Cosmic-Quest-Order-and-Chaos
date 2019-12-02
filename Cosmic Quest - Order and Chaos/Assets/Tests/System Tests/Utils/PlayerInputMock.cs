using System;
using UnityEngine;
using NSubstitute;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMotorController))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(PlayerInteractionController))]
public class PlayerInputMock : MonoBehaviour
{
    public enum MockGamepad
    {
        LeftStick,
        RightStick,
        RightButton,
        RightBumper,
        ButtonSouth,
        ButtonNorth
    }

    public void Set(MockGamepad control, Vector2 input)
    {
        if (input.sqrMagnitude > 1f)
            throw new Exception("Input vector too large. Must have a magnitude no greater than 1.");
        
        switch (control)
        {
            case MockGamepad.LeftStick:
                SendMessage("OnMove", input, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.RightStick:
                SendMessage("OnLook", input, SendMessageOptions.DontRequireReceiver);
                break;
            default:
                throw new Exception("Received invalid control");
        }
    }

    public void Press(MockGamepad control)
    {
        InputValue inputValue = Substitute.For<InputValue>();
        inputValue.isPressed.Returns(true);
        
        switch (control)
        {
            case MockGamepad.RightButton:
                SendMessage("OnPrimaryAttack", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.RightBumper:
                SendMessage("OnSecondaryAttack", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.ButtonNorth:
                SendMessage("OnUltimateAbility", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.ButtonSouth:
                SendMessage("OnInteract", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            default:
                throw new Exception("Received invalid control");
        }
    }
    
    public void Release(MockGamepad control)
    {
        InputValue inputValue = Substitute.For<InputValue>();
        inputValue.isPressed.Returns(false);
        
        switch (control)
        {
            case MockGamepad.RightButton:
                SendMessage("OnPrimaryAttack", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.RightBumper:
                SendMessage("OnSecondaryAttack", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.ButtonNorth:
                SendMessage("OnUltimateAbility", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            case MockGamepad.ButtonSouth:
                SendMessage("OnInteract", inputValue, SendMessageOptions.DontRequireReceiver);
                break;
            default:
                throw new Exception("Received invalid control");
        }
    }
}
