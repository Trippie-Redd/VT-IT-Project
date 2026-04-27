using HSM;

namespace Player
{
    public class Grounded : PlayerState
    {
        public readonly Moving moving;
        public readonly Idle idle;

        public PlayerGroundedOptions Options;

        public Grounded(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            moving = new Moving(machine, this, controller);
            idle = new Idle(machine, this, controller);
        }

        protected override State GetInitialState() => idle;

        protected override void OnUpdate(float deltaTime)
        {
            ApplyHorizontalMovement(Options.targetSpeed);

            if (_controller.velocity.y < 0f)
                _controller.velocity.y = -2f; // Small downward force to keep grounded
        }

        protected override State GetTransition()
            => _controller.inputReader.Direction.sqrMagnitude > 0.01f ? moving : idle;

        public void OnJumpInput()
        {
            if (!_controller.IsGrounded) return;
            if (_controller.root.IsCrouching && !_controller.CanStandUp()) return;
            _controller.root.IsCrouching = false;
            _controller.velocity.y = _controller.jumpForce;
        }
    }
}
