using HSM;
using UnityEngine;

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

        protected override void OnEnter()
        {
            _controller.UsingGravity = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector3 direction = _controller.inputReader.Direction.normalized;
            CharacterController controller = _controller.characterController;
            
            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * (Options.targetSpeed * deltaTime));
            }
            
            if (_controller.velocity.y < 0)
            {
                _controller.velocity.y = -2f; // Small downward force to keep grounded
            }
        }

        protected override State GetTransition()
        {
            return _controller.IsMoving ? moving : idle;
        }
    }
}