using HSM;
using UnityEngine;

namespace Player
{
    public class Crouching : PlayerState
    {
        private readonly PlayerGroundedOptions _options = new()
        {
            targetSpeed = 4f
        };
        
        public Crouching(StateMachine machine, State parent, PlayerController controller, bool idle = false) : base(machine, parent, controller)
        {
            if (idle) _options.targetSpeed = 0f;
        }

        protected override void OnEnter()
        {
            var groundedState = (Grounded)Parent.Parent;
            groundedState.Options = _options;
        }
    }
}