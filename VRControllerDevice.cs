using System.Collections.Generic;
using InControl;
using UnityEngine;

public class VRControllerDevice : InputDevice
{
    private const float LowerDeadZone = 0.2f;

    private const float UpperDeadZone = 0.9f;

    public int DeviceIndex { get; private set; }

    public bool IsConnected => false;

    public DirectionMapping DirectionMappingMode = VRControllerDevice.DirectionMapping.Absolute;

    public enum DirectionMapping
    {
        Absolute,
        ControllerRelative,
        HeadRelative
    }

    private Transform ReferenceTransform = GameObject.Instantiate(new GameObject(), null).transform;

    public VRControllerDevice(int deviceIndex)
        : base("VR Controller")
    {
        DeviceIndex = deviceIndex;
        base.SortOrder = deviceIndex;
        base.Meta = "VR Controller #" + deviceIndex;
        AddControl(InputControlType.LeftStickLeft, "Left Stick Left");
        AddControl(InputControlType.LeftStickRight, "Left Stick Right");
        AddControl(InputControlType.LeftStickUp, "Left Stick Up");
        AddControl(InputControlType.LeftStickDown, "Left Stick Down");
        AddControl(InputControlType.RightStickLeft, "Right Stick Left");
        AddControl(InputControlType.RightStickRight, "Right Stick Right");
        AddControl(InputControlType.RightStickUp, "Right Stick Up");
        AddControl(InputControlType.RightStickDown, "Right Stick Down");
        AddControl(InputControlType.LeftTrigger, "Left Trigger");
        AddControl(InputControlType.RightTrigger, "Right Trigger");
        AddControl(InputControlType.DPadUp, "DPad Up");
        AddControl(InputControlType.DPadDown, "DPad Down");
        AddControl(InputControlType.DPadLeft, "DPad Left");
        AddControl(InputControlType.DPadRight, "DPad Right");
        AddControl(InputControlType.Action1, "O");
        AddControl(InputControlType.Action2, "A");
        AddControl(InputControlType.Action3, "Y");
        AddControl(InputControlType.Action4, "U");
        AddControl(InputControlType.LeftBumper, "Left Bumper");
        AddControl(InputControlType.RightBumper, "Right Bumper");
        AddControl(InputControlType.LeftStickButton, "Left Stick Button");
        AddControl(InputControlType.RightStickButton, "Right Stick Button");
        AddControl(InputControlType.Menu, "Menu");
    }

    public void BeforeAttach()
    {
    }

    // Implement these types of locomotion input: https://developer.oculus.com/resources/artificial-locomotion-controls/

    public override void Update(ulong updateTick, float deltaTime)
    {
        base.Update(updateTick, deltaTime);

        var vpxControllerState = VorpX.vpxGetControllerState( (uint) DeviceIndex);

        Vector2 joystickInput = new Vector2(vpxControllerState.StickX, vpxControllerState.StickY);

        switch (DirectionMappingMode)
        {
            case DirectionMapping.ControllerRelative:
                var selfRotation4f = VorpX.vpxGetControllerRotationQuaternion((uint)DeviceIndex);
                var selfRotation = new Quaternion(-selfRotation4f.x, -selfRotation4f.y, -selfRotation4f.z, selfRotation4f.w);
                ReferenceTransform.rotation = selfRotation;

                joystickInput = GetRelativeMovement(joystickInput, ReferenceTransform);
                break;
            case DirectionMapping.HeadRelative:
                joystickInput = GetRelativeMovement(joystickInput, Camera.main.transform);
                break;
            default:
                break;
        }

        var stickXMin = Mathf.Min(joystickInput.x, 0);
        var stickXMax = Mathf.Max(0, joystickInput.x);

        var stickYMin = Mathf.Min(joystickInput.y, 0);
        var stickYMax = Mathf.Max(0, joystickInput.y);

        UpdateWithValue(InputControlType.LeftStickLeft, stickXMin, updateTick, deltaTime);
        UpdateWithValue(InputControlType.LeftStickRight, stickXMax, updateTick, deltaTime);

        UpdateWithValue(InputControlType.LeftStickDown, stickYMin, updateTick, deltaTime);
        UpdateWithValue(InputControlType.LeftStickUp, stickYMax, updateTick, deltaTime);
    }

    //TODO: Fix that the movement is screwed after the mouse is moved
    public Vector2 GetRelativeMovement(Vector2 movementInput, Transform transform)
    {
        // get input from WASD keys
        float horizontal = movementInput.x;
        float vertical = movementInput.y;

        // get the camera's forward and right vectors
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // adjust the forward and right vectors so they are aligned with the ground plane
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        // combine the vectors to get the direction the player should move
        Vector3 direction = forward * vertical + right * horizontal;
        return new Vector2(direction.x, direction.z).normalized;
    }

    /*
		MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
		MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
		MoveBack.AddDefaultBinding(InputControlType.LeftStickDown);
		LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		LookRight.AddDefaultBinding(InputControlType.RightStickRight);
		LookUp.AddDefaultBinding(InputControlType.RightStickUp);
		LookDown.AddDefaultBinding(InputControlType.RightStickDown);
		Run.AddDefaultBinding(InputControlType.LeftStickButton);
		Jump.AddDefaultBinding(InputControlType.Action1);
		ToggleCrouch.AddDefaultBinding(InputControlType.RightStickButton);
		Activate.AddDefaultBinding(InputControlType.Action4);
		Reload.AddDefaultBinding(InputControlType.Action2);
		Primary.AddDefaultBinding(InputControlType.RightTrigger);
		Secondary.AddDefaultBinding(InputControlType.LeftTrigger);
		InventorySlotLeft.AddDefaultBinding(InputControlType.LeftBumper);
		InventorySlotRight.AddDefaultBinding(InputControlType.RightBumper);
		ToggleFlashlight.AddDefaultBinding(InputControlType.DPadLeft);
		InventorySlot1.AddDefaultBinding(InputControlType.DPadUp);
		InventorySlot2.AddDefaultBinding(InputControlType.DPadRight);
		InventorySlot3.AddDefaultBinding(InputControlType.DPadDown);
		ScrollUp.AddDefaultBinding(InputControlType.DPadUp);
		ScrollDown.AddDefaultBinding(InputControlType.DPadDown);
		Menu.AddDefaultBinding(InputControlType.Menu);
		Menu.AddDefaultBinding(InputControlType.Options);
		Menu.AddDefaultBinding(InputControlType.Start);
		Inventory.AddDefaultBinding(InputControlType.Action3);
		Scoreboard.AddDefaultBinding(InputControlType.View);
		Scoreboard.AddDefaultBinding(InputControlType.TouchPadButton);
		Scoreboard.AddDefaultBinding(InputControlType.Back);
     */
}
