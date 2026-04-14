using HSM;
using UnityEngine;

namespace Player
{
    public class Airborne : PlayerState
    {
        public readonly Falling falling;
        public readonly Jumping jumping;

        public Airborne(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            falling = new Falling(machine, this, controller);
            jumping = new Jumping(machine, this, controller);
        }

        protected override State GetInitialState() => _controller.velocity.y > 0f ? jumping : falling;

        protected override void OnUpdate(float deltaTime)
        {
            Vector2 input = _controller.inputReader.Direction;
            Transform t = _controller.transform;
            Vector3 move = t.right * input.x + t.forward * input.y;
            if (move.sqrMagnitude > 1f) move.Normalize();

            _controller.velocity.x = move.x * 5f;
            _controller.velocity.z = move.z * 5f;
        }

        protected override State GetTransition() => _controller.velocity.y > 0f ? jumping : falling;
    }
}