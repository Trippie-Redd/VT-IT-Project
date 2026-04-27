using HSM;

namespace Player
{
    public class Airborne : PlayerState
    {
        public readonly Falling falling;
        public readonly Jumping jumping;

        public Airborne(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            falling = new Falling(machine, this, controller);
            jumping = new Jumping(machine, this, controller);
        }

        protected override State GetInitialState() => _controller.velocity.y > 0f ? jumping : falling;

        protected override void OnUpdate(float deltaTime)
        {
            ApplyHorizontalMovement(5f);
            _controller.velocity.y -= _controller.gravity * deltaTime;
        }

        protected override State GetTransition() => _controller.velocity.y > 0f ? jumping : falling;
    }
}
