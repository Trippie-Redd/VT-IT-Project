using HSM;
using UnityEngine;

namespace Player
{
    public class Idle : PlayerState
    {
        public readonly CrouchingIdle crouching;
        public readonly StandingIdle standing;
        
        public Idle(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            crouching = new CrouchingIdle(machine, this, controller);
            standing = new StandingIdle(machine, this, controller);
        }

        protected override State GetInitialState()
            => _controller.root.IsCrouching ? crouching : standing;

        protected override State GetTransition()
            => _controller.root.IsCrouching ? crouching : standing;

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}