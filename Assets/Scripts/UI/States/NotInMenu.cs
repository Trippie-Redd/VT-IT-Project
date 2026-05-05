using HSM;
using UnityEngine;

namespace UI
{
    public class NotInMenu : MenuState
    {
        public NotInMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
        {
        }

        protected override void OnEnter()
        {
            HideRoot();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (menu.HUD != null) menu.HUD.SetActive(true);
            if (menu.InputReader != null) menu.InputReader.GameplayInputEnabled = true;
        }
    }
}