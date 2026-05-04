using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2> Look = delegate { };
        public event UnityAction Attack = delegate { };
        public event UnityAction Reload = delegate { };
        public event UnityAction Interact = delegate { };
        public event UnityAction Jump = delegate { };

        public event UnityAction Pause = delegate { };

        public event UnityAction<bool> Sprint = delegate { };
        public event UnityAction<bool> Crouch = delegate { };

        public bool GameplayInputEnabled { get; set; } = true;

        private InputSystem_Actions _inputActions;

        public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsJumpHeld   => _inputActions != null && _inputActions.Player.Jump.IsPressed();
        public bool IsCrouchHeld => _inputActions != null && _inputActions.Player.Crouch.IsPressed();
        public bool IsAttackHeld => _inputActions != null && _inputActions.Player.Attack.IsPressed();
        
        public void EnablePlayerActions()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputSystem_Actions();
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Enable();
        }

        public void DisablePlayerActions()
        {
            _inputActions?.Disable();
        }

        private void OnDisable()
        {
            DisablePlayerActions();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled) Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled) Look.Invoke(context.ReadValue<Vector2>());
        }

        private bool _IsDeviceMouse(InputAction.CallbackContext context)
            => context.control.device.name == "Mouse";

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled && context.performed) Attack.Invoke();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled && context.performed) Reload.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled && context.performed) Interact.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed) Pause.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (!GameplayInputEnabled) return;
            if (context.performed)
                Crouch.Invoke(true);
            else if (context.canceled)
                Crouch.Invoke(false);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (GameplayInputEnabled && context.performed) Jump.Invoke();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!GameplayInputEnabled) return;
            if (context.performed)
                Sprint.Invoke(true);
            else if (context.canceled)
                Sprint.Invoke(false);
        }
    }
}