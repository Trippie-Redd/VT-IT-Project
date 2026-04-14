using System;
using HSM;
using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController), typeof(Shooter))]
    public class PlayerController : MonoBehaviour
    {
        [Flags]
        public enum Flags
        {
            IsSubmerged  = 1 << 0,
            IsMoving     = 1 << 1,
            IsCrouching  = 1 << 2,
            IsSprinting  = 1 << 3,

            UsingGravity = 1 << 4,
            IsJumping    = 1 << 5,
        }
        
        #region Fields
        
        public State root;
        public StateMachine machine;

        [HideInInspector] public CharacterController characterController;
        public InputReader inputReader;

        [HideInInspector] public Vector3 velocity;

        Flags _flags;
        
        public bool IsGrounded => characterController.isGrounded;

        [HideInInspector] public bool IsSubmerged
        {
            get => _flags.HasFlag(Flags.IsSubmerged);
            set
            {
                if (value) _flags |= Flags.IsSubmerged;
                else       _flags &= ~Flags.IsSubmerged;
            }
        }

        [HideInInspector] public bool IsMoving
        {
            get => _flags.HasFlag(Flags.IsMoving);
            set
            {
                if (value) _flags |= Flags.IsMoving;
                else       _flags &= ~Flags.IsMoving;
            }
        }
        
        [HideInInspector] public bool IsCrouching
        {
            get => _flags.HasFlag(Flags.IsCrouching);
            set
            {
                if (value) _flags |= Flags.IsCrouching;
                else       _flags &= ~Flags.IsCrouching;
            }
        }
        
        [HideInInspector] public bool IsSprinting
        {
            get => _flags.HasFlag(Flags.IsSprinting);
            set
            {
                if (value) _flags |= Flags.IsSprinting;
                else       _flags &= ~Flags.IsSprinting;
            }
        }

        [HideInInspector] public bool UsingGravity
        {
            get => _flags.HasFlag(Flags.UsingGravity);
            set
            {
                if (value) _flags |= Flags.UsingGravity;
                else       _flags &= ~Flags.UsingGravity;
            }
        }

        [HideInInspector] public bool IsJumping
        {
            get => _flags.HasFlag(Flags.IsJumping);
            set
            {
                if (value) _flags |= Flags.IsJumping;
                else       _flags &= ~Flags.IsJumping;
            }
        }

        public float jumpHeight = 1.27f;
        public float jumpAirTime = 1.02f;

        [HideInInspector] public float jumpForce;
        [HideInInspector] public float gravity;

        public float standingHeight = 2f;
        public float crouchingHeight = 1.2f;
        public float crouchSpeed = 8f;
        public Transform cameraTransform;

        float _standingCameraY;

        #endregion
        
        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();

            jumpForce = 4f * jumpHeight / jumpAirTime;
            gravity   = 8f * jumpHeight / (jumpAirTime * jumpAirTime);

            Debug.Assert(cameraTransform != null, "PlayerController: cameraTransform is not assigned.", this);
            _standingCameraY = cameraTransform != null ? cameraTransform.localPosition.y : 0f;

            root = new Root(null, this);
            var builder = new StateMachineBuilder(root);
            machine = builder.Build();

            inputReader.Move   += _OnMove;
            inputReader.Sprint += _OnSprint;
            inputReader.Crouch += _OnCrouch;
            inputReader.Jump   += _OnJump;
            inputReader.Attack += GetComponent<Shooter>().Shooting;
            inputReader.EnablePlayerActions();
        }

        void OnDestroy()
        {
            inputReader.Move   -= _OnMove;
            inputReader.Sprint -= _OnSprint;
            inputReader.Crouch -= _OnCrouch;
            inputReader.Jump   -= _OnJump;
            inputReader.Attack -= GetComponent<Shooter>().Shooting;
            inputReader.DisablePlayerActions();
        }
        
        void Update()
        {
            machine.Tick(Time.deltaTime);

            if (UsingGravity)
                velocity += new Vector3(0f, -gravity * Time.deltaTime, 0f);

            _UpdateCrouch();

            characterController.Move(velocity * Time.deltaTime);
        }

        bool _CanStandUp()
        {
            float radius = characterController.radius;
            // Cast from the top sphere of the crouching capsule upward to where the standing top sphere would be
            float crouchTopSphereY = crouchingHeight - standingHeight / 2f - radius;
            Vector3 origin = transform.position + Vector3.up * crouchTopSphereY;
            float castDistance = standingHeight - crouchingHeight;
            return !Physics.SphereCast(origin, radius, Vector3.up, out _, castDistance);
        }

        void _UpdateCrouch()
        {
            if (cameraTransform == null) return;
            float targetHeight = IsCrouching ? crouchingHeight : standingHeight;
            float newHeight = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed * Time.deltaTime);
            characterController.height = newHeight;
            // Keep capsule bottom fixed so the player doesn't float up when crouching
            characterController.center = new Vector3(0f, (newHeight - standingHeight) / 2f, 0f);

            float targetCamY = IsCrouching
                ? _standingCameraY - (standingHeight - crouchingHeight)
                : _standingCameraY;
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetCamY, crouchSpeed * Time.deltaTime);
            cameraTransform.localPosition = camPos;
        }

        #region InputCallbacks

        void _OnMove(Vector2 movement)
        {
            IsMoving = movement.magnitude > .1f;
        }

        void _OnSprint(bool pressed)
        {
            if (IsCrouching && !_CanStandUp()) return;
            IsCrouching = false;
            IsSprinting = pressed;
        }

        void _OnCrouch(bool pressed)
        {
            if (!pressed) return;

            if (IsCrouching && !_CanStandUp()) return;
            IsCrouching = !IsCrouching;
            if (IsCrouching) IsSprinting = false;
        }

        void _OnJump()
        {
            if (IsGrounded && _CanStandUp())
            {
                IsCrouching = false;
                IsJumping = true;
            }
        }
        #endregion
    }
}