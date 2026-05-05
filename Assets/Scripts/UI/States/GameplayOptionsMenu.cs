using HSM;
using UnityEngine.UIElements;

namespace UI
{
    public class GameplayOptionsMenu : MenuState
    {
        public GameplayOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
        {

        }

        protected override void OnEnter()
        {
            ShowVE("gameplay-options-container");
            
            menu.panelHeaderText.Enter("GAMEPLAY");
        }


        protected override void OnExit()
        {
            HideVE("gameplay-options-container");
            
            menu.panelHeaderText.Exit();
        }
    }
}
