using HSM;
using UnityEngine.UIElements;

public class AudioOptionsMenu : MenuState
{
    public AudioOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {

    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        root.Q<VisualElement>("audio-options-menu").style.display = DisplayStyle.Flex;
    }


    protected override void OnExit()
    {
        menu.RootVE.Q<VisualElement>("audio-options-menu").style.display = DisplayStyle.None;
    }
}
