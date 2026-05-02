using HSM;

namespace Player
{
    public class Jumping : PlayerState
    {
        public Jumping(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }
    }
}
