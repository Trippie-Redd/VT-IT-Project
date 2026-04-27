using HSM;
using UnityEngine;

namespace Player
{
    public sealed class Root : PlayerState
    {
        public readonly Grounded grounded;
        public readonly Airborne airborne;
        public readonly Submerged submerged;

        public bool IsCrouching;
        public bool IsSprinting;

        float _standingCameraY;

        public Root(StateMachine machine, PlayerController controller) : base(machine, null, controller)
        {
            grounded  = new Grounded(machine, this, controller);
            airborne  = new Airborne(machine, this, controller);
            submerged = new Submerged(machine, this, controller);
        }

        protected override State GetInitialState() => grounded;

        protected override void OnEnter()
        {
            _standingCameraY = _controller.cameraTransform != null
                ? _controller.cameraTransform.localPosition.y
                : 0f;
        }

        protected override State GetTransition()
        {
            if (_controller.IsSubmerged) return submerged;
            if (!_controller.IsGrounded) return airborne;
            return grounded;
        }

        protected override void OnUpdate(float deltaTime) => _UpdateCrouchPosture(deltaTime);

        public void OnSprintInput(bool pressed)
        {
            if (IsCrouching && !_controller.CanStandUp()) return;
            IsCrouching = false;
            IsSprinting = pressed;
        }

        public void OnCrouchInput(bool pressed)
        {
            if (!pressed) return;
            if (IsCrouching && !_controller.CanStandUp()) return;
            IsCrouching = !IsCrouching;
            if (IsCrouching) IsSprinting = false;
        }

        void _UpdateCrouchPosture(float deltaTime)
        {
            if (_controller.cameraTransform == null) return;

            CharacterController cc = _controller.characterController;
            float standing = _controller.standingHeight;
            float crouching = _controller.crouchingHeight;
            float lerpRate = _controller.crouchSpeed * deltaTime;

            float targetHeight = IsCrouching ? crouching : standing;
            float newHeight = Mathf.Lerp(cc.height, targetHeight, lerpRate);
            cc.height = newHeight;
            // Keep capsule bottom fixed so the player doesn't float up when crouching
            cc.center = new Vector3(0f, (newHeight - standing) / 2f, 0f);

            float targetCamY = IsCrouching ? _standingCameraY - (standing - crouching) : _standingCameraY;
            Vector3 camPos = _controller.cameraTransform.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetCamY, lerpRate);
            _controller.cameraTransform.localPosition = camPos;
        }
    }
}
