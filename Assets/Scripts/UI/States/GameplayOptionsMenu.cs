using HSM;
using UnityEngine.UIElements;

public class GameplayOptionsMenu : MenuState
{
    public GameplayOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {

    }

    protected override void OnEnter()
    {
        ShowVE("gameplay-options-container");
    }


    protected override void OnExit()
    {
        HideVE("gameplay-options-container");
    }
}
