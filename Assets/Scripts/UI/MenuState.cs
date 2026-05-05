using HSM;
using UnityEngine.UIElements;

namespace UI
{
    public class MenuState : State
    {
        protected readonly Menu menu;

        protected MenuState(StateMachine machine, State parent, Menu menu) : base(machine, parent)
            => this.menu = menu;

        VisualElement Root => menu.GetComponent<UIDocument>().rootVisualElement;

        protected void ShowVE(string name) => Root.Q<VisualElement>(name).style.display = DisplayStyle.Flex;
        protected void HideVE(string name) => Root.Q<VisualElement>(name).style.display = DisplayStyle.None;
        protected void ShowRoot()          => Root.style.display = DisplayStyle.Flex;
        protected void HideRoot()          => Root.style.display = DisplayStyle.None;
    }
}