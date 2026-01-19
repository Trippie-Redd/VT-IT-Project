using HSM;

namespace Player
{
    public abstract class PlayerState : State
    {
        protected readonly PlayerController _controller;
        
        protected PlayerState(StateMachine machine, State parent,PlayerController controller) : base(machine, parent)
            => _controller = controller;
    }
}