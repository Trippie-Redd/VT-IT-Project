using HSM;
using UnityEngine;

namespace Player
{
    public class Moving : PlayerState
    {
        public readonly Crouching crouching;
        public readonly Walking walking;
        public readonly Sprinting sprinting;
        
        public Moving(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            walking = new Walking(machine, this, controller);
            sprinting = new Sprinting(machine, this, controller);
            crouching = new Crouching(machine, this, controller);
        }

        protected override State GetInitialState() => walking;

        protected override State GetTransition()
        {
            if (_controller.IsSprinting) return sprinting;
            if (_controller.IsCrouching)  return crouching;
            return                       walking;
        }
    }
}