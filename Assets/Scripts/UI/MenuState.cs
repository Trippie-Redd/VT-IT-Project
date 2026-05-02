using HSM;
using UnityEngine.UIElements;

public class MenuState : State
{
    protected readonly Menu menu;

    protected MenuState(StateMachine machine, State parent, Menu menu) : base(machine, parent)
        => this.menu = menu;

    protected void ShowVE(string name)
        => menu.RootVE.Q<VisualElement>(name).style.display = DisplayStyle.Flex;
    
    protected void HideVE(string name)
        => menu.RootVE.Q<VisualElement>(name).style.display = DisplayStyle.None;
}
