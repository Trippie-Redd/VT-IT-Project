using HSM;
using UnityEngine.UIElements;

public class ControlsOptionsMenu : MenuState
{
    public ControlsOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {

    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        root.Q<VisualElement>("controls-options-menu").style.display = DisplayStyle.Flex;
    }


    protected override void OnExit()
    {
        menu.RootVE.Q<VisualElement>("controls-options-menu").style.display = DisplayStyle.None;
    }
}
