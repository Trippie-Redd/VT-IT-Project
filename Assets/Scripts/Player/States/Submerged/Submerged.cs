using HSM;

namespace Player
{
    public class Submerged : PlayerState
    {
        const float SwimSpeed = 4f;
        const float VerticalSwimSpeed = 3f;

        public Submerged(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }

        protected override void OnEnter()
        {
            // Drop posture flags so we don't surface still crouched/sprinting
            var root = (Root)Parent;
            root.IsCrouching = false;
            root.IsSprinting = false;
        }

        protected override void OnUpdate(float deltaTime)
        {
            ApplyHorizontalMovement(SwimSpeed);

            float vertical = 0f;
            if (_controller.inputReader.IsJumpHeld)   vertical += 1f;
            if (_controller.inputReader.IsCrouchHeld) vertical -= 1f;

            _controller.velocity.y = vertical * VerticalSwimSpeed;
        }
    }
}
