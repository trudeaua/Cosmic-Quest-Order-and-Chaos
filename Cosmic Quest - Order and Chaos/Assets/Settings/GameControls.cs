// GENERATED AUTOMATICALLY FROM 'Assets/Settings/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""9c4661d3-068a-4bc4-880d-e45624c55b69"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a2411fe7-c6e6-4bd0-b021-87e5e42de7aa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""298efebf-dd14-4d04-9a10-a0e016650a8f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""4376de81-1207-42e1-8154-ed6200a68419"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SecondaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""7fcc6c30-bd3c-41a9-92de-0f7a4fa0d592"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""e02c0312-8f9d-4012-bd07-e9f92deaf64f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""UltimateAbility"",
                    ""type"": ""Button"",
                    ""id"": ""c9be423e-0edd-45eb-857e-3609d971d47d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""f6bdf6d2-d3eb-4038-9778-443f8827d98a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""efc94c5a-66c5-4ba5-aeb8-a502e846cbf8"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.125,max=0.95)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""244dce68-56c1-4fc3-a812-ce47063db859"",
                    ""path"": ""2DVector(normalize=false)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f15f4642-4ee3-474a-9512-8854baebf785"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e25c0d0e-930b-425e-a9b3-9384f4309a4f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""25c92156-cece-4bdf-b4cf-901ba3e1c361"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f55f5689-cf8f-4449-83c6-bc679eb043dd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""71915566-51fe-4c9a-98a9-bba0bb438ff0"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd5ed275-6d77-4d7a-85e6-36ae5b943a5b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""180f8046-25f9-4af1-8abb-4fca2221bed3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cebb2677-9c6a-4c6b-8f2a-ac868b002fb4"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c27a0793-e45c-4d76-8b0e-c49ac51698bf"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.125,max=0.95)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dc6d5f3-954e-44c5-a71e-581452dcab63"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1c2d901-5de9-4eeb-abf5-9e302da8df57"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9130e8dd-6809-4b59-b0cd-c0190b780669"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6fc335f-6b5b-4bae-a977-11a75aead616"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e423dfe-f35f-4654-9a95-547cc7d7366a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67628a38-fadd-45c0-8566-52dfb696c24b"",
                    ""path"": ""<Keyboard>/#(E)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6f5da0c-1c91-47b3-a073-8d7b0fc89d09"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1d907c7-b69e-4e74-86d4-0710ce636725"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""UltimateAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c293e473-d0ae-4723-96dc-092712df5d6e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""UltimateAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0047b22c-e718-4a2f-a217-4863b5e79a0f"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""UltimateAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10f16e88-4e83-4f13-bccf-f43d074f92b0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bce08c7-30d3-44cf-bf3d-ee2852db9afc"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65cf0c6e-406a-4655-af9d-f5b309c836e0"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""a960e2a2-3101-4b42-b86d-ee315aaec8b1"",
            ""actions"": [
                {
                    ""name"": ""MenuNavigate"",
                    ""type"": ""Value"",
                    ""id"": ""6b206bb7-c9cc-4a35-951b-7f7a81206839"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuSelect"",
                    ""type"": ""Button"",
                    ""id"": ""42861f1a-b0d3-4966-9c50-9dd60852ca5a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MenuCancel"",
                    ""type"": ""Button"",
                    ""id"": ""70ba4ddf-fdf6-4b37-a8b7-62d4f8ca6203"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MenuOpen"",
                    ""type"": ""Button"",
                    ""id"": ""6b143529-40bc-40ad-b621-0a10ab3a6ead"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MenuClose"",
                    ""type"": ""Button"",
                    ""id"": ""684789e1-b3ac-4f60-92a9-eba3718282db"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a05432af-692b-4083-ac94-26390b19efd7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6410bf8b-168b-476a-9869-e79b6ef6dc5d"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""861628c2-15ce-4032-a3a9-25f72ee5cbfa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""11864ca2-ac12-4a4a-a986-490303d79a55"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fe1c973a-b692-4c86-be6b-d6dbc6859a90"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3a67ad94-0076-4c29-9792-f57d34dc304a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6b6f435c-6c8d-42e3-968c-1ac10683da6c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""6a9ef3b4-56d7-45be-bebc-3b6d995d7424"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""113cbb94-60fe-4a99-845f-53160b7e27e5"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ac5da0be-5b78-4b07-a85c-9d26a2d65739"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9648c550-e978-4c6d-b116-34f0d6f75be7"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""768d65f7-e9ec-49db-af2a-239da3246a35"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4f317ebd-e0b7-4760-8ecd-290e3637294b"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""MenuNavigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36c2b65d-ef9e-40b8-923a-a806a6b2feef"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fdf3ea40-b766-487f-b722-deff05fead86"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8f9dcdb-47f9-4916-8cd7-e45b33af77ac"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""MenuSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6df4b4c6-b28b-4023-a40d-6429331a0d44"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d250a49-8198-434e-9059-6c09d9cef70e"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuCancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dd6680d-9ffa-4e8a-85b8-4ad33aa57392"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuCancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3efc9c6-1ae2-4437-90c0-9b1066a8eace"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""MenuCancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3e43905-cc18-44d7-a9bf-a2234fd39661"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""824b896c-d16b-4dbe-a1d2-c77e68ad8bf0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f0b9b09-c00e-4ea4-b8a5-781e8eb6abc2"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""MenuOpen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ce1a645-1d5f-457b-9a19-1f242a387ef2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e29e0273-10a2-47e0-ac03-c415b231246e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87a50bcb-7ac5-41ce-b132-658082bbcce1"",
                    ""path"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""MenuClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<HID::mayflash limited MAYFLASH GameCube Controller Adapter>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_PrimaryAttack = m_Player.FindAction("PrimaryAttack", throwIfNotFound: true);
        m_Player_SecondaryAttack = m_Player.FindAction("SecondaryAttack", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_UltimateAbility = m_Player.FindAction("UltimateAbility", throwIfNotFound: true);
        m_Player_PauseGame = m_Player.FindAction("PauseGame", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_MenuNavigate = m_UI.FindAction("MenuNavigate", throwIfNotFound: true);
        m_UI_MenuSelect = m_UI.FindAction("MenuSelect", throwIfNotFound: true);
        m_UI_MenuCancel = m_UI.FindAction("MenuCancel", throwIfNotFound: true);
        m_UI_MenuOpen = m_UI.FindAction("MenuOpen", throwIfNotFound: true);
        m_UI_MenuClose = m_UI.FindAction("MenuClose", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_PrimaryAttack;
    private readonly InputAction m_Player_SecondaryAttack;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_UltimateAbility;
    private readonly InputAction m_Player_PauseGame;
    public struct PlayerActions
    {
        private @GameControls m_Wrapper;
        public PlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @PrimaryAttack => m_Wrapper.m_Player_PrimaryAttack;
        public InputAction @SecondaryAttack => m_Wrapper.m_Player_SecondaryAttack;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @UltimateAbility => m_Wrapper.m_Player_UltimateAbility;
        public InputAction @PauseGame => m_Wrapper.m_Player_PauseGame;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @PrimaryAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrimaryAttack;
                @SecondaryAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @UltimateAbility.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUltimateAbility;
                @UltimateAbility.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUltimateAbility;
                @UltimateAbility.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUltimateAbility;
                @PauseGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @PrimaryAttack.started += instance.OnPrimaryAttack;
                @PrimaryAttack.performed += instance.OnPrimaryAttack;
                @PrimaryAttack.canceled += instance.OnPrimaryAttack;
                @SecondaryAttack.started += instance.OnSecondaryAttack;
                @SecondaryAttack.performed += instance.OnSecondaryAttack;
                @SecondaryAttack.canceled += instance.OnSecondaryAttack;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @UltimateAbility.started += instance.OnUltimateAbility;
                @UltimateAbility.performed += instance.OnUltimateAbility;
                @UltimateAbility.canceled += instance.OnUltimateAbility;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_MenuNavigate;
    private readonly InputAction m_UI_MenuSelect;
    private readonly InputAction m_UI_MenuCancel;
    private readonly InputAction m_UI_MenuOpen;
    private readonly InputAction m_UI_MenuClose;
    public struct UIActions
    {
        private @GameControls m_Wrapper;
        public UIActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuNavigate => m_Wrapper.m_UI_MenuNavigate;
        public InputAction @MenuSelect => m_Wrapper.m_UI_MenuSelect;
        public InputAction @MenuCancel => m_Wrapper.m_UI_MenuCancel;
        public InputAction @MenuOpen => m_Wrapper.m_UI_MenuOpen;
        public InputAction @MenuClose => m_Wrapper.m_UI_MenuClose;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @MenuNavigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigate;
                @MenuNavigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigate;
                @MenuNavigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigate;
                @MenuSelect.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuSelect;
                @MenuSelect.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuSelect;
                @MenuSelect.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuSelect;
                @MenuCancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuCancel;
                @MenuCancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuCancel;
                @MenuCancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuCancel;
                @MenuOpen.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuOpen;
                @MenuOpen.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuOpen;
                @MenuOpen.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuOpen;
                @MenuClose.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuClose;
                @MenuClose.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuClose;
                @MenuClose.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuClose;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MenuNavigate.started += instance.OnMenuNavigate;
                @MenuNavigate.performed += instance.OnMenuNavigate;
                @MenuNavigate.canceled += instance.OnMenuNavigate;
                @MenuSelect.started += instance.OnMenuSelect;
                @MenuSelect.performed += instance.OnMenuSelect;
                @MenuSelect.canceled += instance.OnMenuSelect;
                @MenuCancel.started += instance.OnMenuCancel;
                @MenuCancel.performed += instance.OnMenuCancel;
                @MenuCancel.canceled += instance.OnMenuCancel;
                @MenuOpen.started += instance.OnMenuOpen;
                @MenuOpen.performed += instance.OnMenuOpen;
                @MenuOpen.canceled += instance.OnMenuOpen;
                @MenuClose.started += instance.OnMenuClose;
                @MenuClose.performed += instance.OnMenuClose;
                @MenuClose.canceled += instance.OnMenuClose;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnPrimaryAttack(InputAction.CallbackContext context);
        void OnSecondaryAttack(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnUltimateAbility(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMenuNavigate(InputAction.CallbackContext context);
        void OnMenuSelect(InputAction.CallbackContext context);
        void OnMenuCancel(InputAction.CallbackContext context);
        void OnMenuOpen(InputAction.CallbackContext context);
        void OnMenuClose(InputAction.CallbackContext context);
    }
}
