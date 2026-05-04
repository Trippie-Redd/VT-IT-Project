using HSM;
using UnityEngine;

namespace Player
{
    public class Sprinting : PlayerState
    {
        private readonly PlayerGroundedOptions _options = new()
        {
            targetSpeed = 9f
        };
        
        public Sprinting(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }

        protected override void OnEnter()
        {
            var groundedState = (Grounded)Parent.Parent;
            groundedState.Options = _options;
        }
    }
}