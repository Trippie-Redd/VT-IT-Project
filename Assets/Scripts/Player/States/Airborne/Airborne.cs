using HSM;
using UnityEngine;

namespace Player
{
    public class Airborne : PlayerState
    {
        public readonly Falling falling;
        
        public Airborne(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller)
        {
            falling = new Falling(machine, this, controller);
        }

        protected override State GetInitialState() => falling;

        protected override State GetTransition()
        {
            return falling;
        }
    }
}