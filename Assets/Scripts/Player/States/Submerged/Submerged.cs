using HSM;

namespace Player
{
    public class Submerged : PlayerState
    {
        public Submerged(StateMachine machine, State parent, PlayerController controller) : base(machine, parent, controller) { }
    }
}