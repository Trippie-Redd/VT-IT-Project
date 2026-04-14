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

        public float jumpForce = 5f;

        #endregion
        
        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();

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
                velocity += new Vector3(0f, -9.82f * Time.deltaTime, 0f);
            
            characterController.Move(velocity * Time.deltaTime);
        }

        #region InputCallbacks

        void _OnMove(Vector2 movement)
        {
            IsMoving = movement.magnitude > .1f;
        }

        void _OnSprint(bool pressed)
        {
            if (IsCrouching) IsCrouching = false;
            
            IsSprinting = pressed;
        }

        void _OnCrouch(bool pressed)
        {
            if (!pressed) return;

            IsCrouching = !IsCrouching;
        }

        void _OnJump()
        {
            if (IsGrounded)
                IsJumping = true;
        }
        #endregion
    }
}