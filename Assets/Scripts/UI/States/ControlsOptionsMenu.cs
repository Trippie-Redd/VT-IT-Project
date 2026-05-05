using HSM;
using UnityEngine.UIElements;

namespace UI
{
    public class ControlsOptionsMenu : MenuState
    {
        public ControlsOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
        {

        }

        protected override void OnEnter()
        {
            ShowVE("controls-options-container");
            
            menu.panelHeaderText.Enter("Controls");
        }


        protected override void OnExit()
        {
            HideVE("controls-options-container");
            
            menu.panelHeaderText.Exit();
        }
    }
}
