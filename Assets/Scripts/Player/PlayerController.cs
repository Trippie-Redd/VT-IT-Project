using System.Collections.Generic;
using HSM;
using Input;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public Root root;
        public StateMachine machine;

        [HideInInspector] public CharacterController characterController;
        public InputReader inputReader;

        [HideInInspector] public Vector3 velocity;

        readonly HashSet<Water> _waters = new();
        public bool IsSubmerged => _waters.Count > 0;
        public bool IsGrounded => characterController.isGrounded;

        public float jumpHeight = 1.27f;
        public float jumpAirTime = 1.02f;
        [HideInInspector] public float jumpForce;
        [HideInInspector] public float gravity;

        public float standingHeight = 2f;
        public float crouchingHeight = 1.2f;
        public float crouchSpeed = 8f;
        public Transform cameraTransform;

        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();

            jumpForce = 4f * jumpHeight / jumpAirTime;
            gravity   = 8f * jumpHeight / (jumpAirTime * jumpAirTime);

            Debug.Assert(cameraTransform != null, "PlayerController: cameraTransform is not assigned.", this);

            root = new Root(null, this);
            var builder = new StateMachineBuilder(root);
            machine = builder.Build();

            inputReader.Sprint += _OnSprint;
            inputReader.Crouch += _OnCrouch;
            inputReader.Jump   += _OnJump;
            inputReader.EnablePlayerActions();
        }

        void OnDestroy()
        {
            inputReader.Sprint -= _OnSprint;
            inputReader.Crouch -= _OnCrouch;
            inputReader.Jump   -= _OnJump;
            inputReader.DisablePlayerActions();
        }

        void Update()
        {
            machine.Tick(Time.deltaTime);
            characterController.Move(velocity * Time.deltaTime);
            Debug.Log(_waters.Count);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Water water)) _waters.Add(water);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Water water)) _waters.Remove(water);
        }

        public bool CanStandUp()
        {
            float radius = characterController.radius;
            // Cast from the top sphere of the crouching capsule upward to where the standing top sphere would be
            float crouchTopSphereY = crouchingHeight - standingHeight / 2f - radius;
            Vector3 origin = transform.position + Vector3.up * crouchTopSphereY;
            float castDistance = standingHeight - crouchingHeight;
            return !Physics.SphereCast(origin, radius, Vector3.up, out _, castDistance);
        }

        void _OnSprint(bool pressed) => root.OnSprintInput(pressed);
        void _OnCrouch(bool pressed) => root.OnCrouchInput(pressed);
        void _OnJump()               => root.grounded.OnJumpInput();
    }
}
