using HSM;

namespace Player
{
    public class CrouchingIdle : PlayerState
    {
        private readonly PlayerGroundedOptions _options = new()
        {
            targetSpeed = 0f
        };
        
        public CrouchingIdle(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
        }

        protected override void OnEnter()
        {
            var groundedState = (Grounded)Parent.Parent;
            groundedState.Options = _options;
        }
    }
}