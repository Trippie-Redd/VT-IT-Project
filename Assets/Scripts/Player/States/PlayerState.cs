using HSM;
using UnityEngine;

namespace Player
{
    public abstract class PlayerState : State
    {
        protected readonly PlayerController _controller;

        protected PlayerState(StateMachine machine, State parent, PlayerController controller) : base(machine, parent)
            => _controller = controller;

        protected void ApplyHorizontalMovement(float speed)
        {
            Vector2 input = _controller.inputReader.Direction;
            Transform t = _controller.transform;
            Vector3 move = t.right * input.x + t.forward * input.y;
            if (move.sqrMagnitude > 1f) move.Normalize();

            _controller.velocity.x = move.x * speed;
            _controller.velocity.z = move.z * speed;
        }
    }
}
