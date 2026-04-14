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
            Vector2 input = _controller.inputReader.Direction;
            Transform t = _controller.transform;
            Vector3 move = (t.right * input.x + t.forward * input.y);
            if (move.sqrMagnitude > 1f) move.Normalize();

            _controller.velocity.x = move.x * Options.targetSpeed;
            _controller.velocity.z = move.z * Options.targetSpeed;

            if (_controller.IsJumping)
            {
                _controller.velocity.y = _controller.jumpForce;
                _controller.IsJumping = false;
            }
            else if (_controller.velocity.y < 0)
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