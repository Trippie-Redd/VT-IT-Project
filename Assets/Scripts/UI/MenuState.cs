using HSM;
using UnityEngine.UIElements;

public class MenuState : State
{
    protected readonly Menu menu;

    protected MenuState(StateMachine machine, State parent, Menu menu) : base(machine, parent)
        => this.menu = menu;
}
