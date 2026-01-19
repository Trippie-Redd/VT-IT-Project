using HSM;
using UnityEngine;

namespace Player
{
    public class Falling : PlayerState
    {
        public Falling(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            
        }

        protected override void OnEnter()
        {
            _controller.UsingGravity = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}