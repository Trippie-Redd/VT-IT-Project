using HSM;
using UnityEngine;

namespace Player
{
    public sealed class Root : PlayerState
    {
        public readonly Grounded grounded;
        public readonly Airborne airborne;
        public readonly Submerged submerged;

        public Root(StateMachine machine, PlayerController controller) : base(machine, null, controller)
        {
            grounded = new Grounded(machine, this, controller);
            airborne = new Airborne(machine, this, controller);
            submerged = new Submerged(machine, this, controller);
        }

        protected override State GetInitialState() => grounded;

        protected override State GetTransition()
        {
            if (_controller.IsSubmerged) return submerged;
            if (!_controller.IsGrounded) return airborne;
            return grounded;
        }
    }
}