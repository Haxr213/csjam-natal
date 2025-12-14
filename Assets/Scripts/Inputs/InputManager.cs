using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.iOS;
using System;

[CreateAssetMenu(fileName = "InputManager", menuName = "Scriptable Objects/InputManager")]
public class InputManager : ScriptableObject
{
    private PlayerController _playerInput;

    // tracks which device type is active based on recent changes
    private DeviceType activeDevice = DeviceType.Keyboard;

    /// <summary>
    /// Event fired when the active device changes (from xbox to keyboard)
    /// </summary>
    public event Action ActiveDeviceChangeEvent;

    private void OnEnable()
    {
        if(_playerInput == null)
        {
            _playerInput = new PlayerController();
            // enable the input so we start getting events
            _playerInput.Enable();

            InputSystem.onActionChange += TrackActions;
        }
    }

    public DeviceType GetActiveDevice()
    {
        return activeDevice;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= TrackActions;
    }

    private void TrackActions(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction inputAction = (InputAction)obj;
            InputControl activeControl = inputAction.activeControl;
            //Debug.LogFormat("Current Control {0}", activeControl);

            var newDevice = DeviceType.Keyboard;

            if (activeControl.device is Keyboard)
            {
                newDevice = DeviceType.Keyboard;
            }

            if (activeControl.device is Gamepad)
            {
                newDevice = DeviceType.Gamepad;

                // we can further categorize these if we had spritesheets per brand
                if (activeControl.device is XInputController)
                {
                    //Debug.LogFormat("Device is Xbox Controller");
                }

                if (activeControl.device is DualSenseGamepadHID || activeControl.device is DualShockGamepad)
                {
                    //Debug.LogFormat("Device is Playstation");
                }
            }

            // we detected a change
            if (activeDevice != newDevice)
            {
                activeDevice = newDevice;
                // fire an event to anyone listening
                ActiveDeviceChangeEvent?.Invoke();
            }
        }

        if (change == InputActionChange.BoundControlsChanged)
        {
            //BindingsChangedEvent?.Invoke();
        }
    }

    public PlayerController GetPlayerInput()
    {
        return _playerInput;
    }

    public InputBinding GetBinding(string actionName, DeviceType deviceType)
    {
        InputAction action = _playerInput.FindAction(actionName);

        InputBinding deviceBinding = action.bindings[(int)deviceType];

        return deviceBinding;
    }
}
