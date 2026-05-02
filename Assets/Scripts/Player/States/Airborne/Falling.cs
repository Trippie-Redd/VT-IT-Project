using HSM;

namespace Player
{
    public class Falling : PlayerState
    {
        public Falling(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }
    }
}
