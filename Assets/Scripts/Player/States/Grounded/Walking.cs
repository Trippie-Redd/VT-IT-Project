using HSM;
using UnityEngine;

namespace Player
{
    public class Walking : PlayerState
    {
        private readonly PlayerGroundedOptions _options = new()
        {
            targetSpeed = 5f
        };
        
        public Walking(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }

        protected override void OnEnter()
        {
            var groundedState = (Grounded)Parent.Parent;
            groundedState.Options = _options;
        }
    }
}