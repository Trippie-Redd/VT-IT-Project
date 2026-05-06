using HSM;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MenuState
    {
        Button _startButton, _exitButton, _optionsButton;

        public PauseMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
        {
        }

        protected override void OnEnter()
        {
            ShowRoot();

            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            if (menu.HUD != null) menu.HUD.SetActive(false);
            if (menu.InputReader != null) menu.InputReader.GameplayInputEnabled = false;

            ShowVE("menu-container");
            ShowVE("main-buttons-container");
            ShowVE("panel-container");
            ShowVE("panel-header");
            ShowVE("panel");
            ShowVE("mission-brief-container");

            _startButton = menu.GetComponent<UIDocument>().rootVisualElement.Q<Button>("start-button");
            _optionsButton = menu.GetComponent<UIDocument>().rootVisualElement.Q<Button>("options-button");
            _exitButton = menu.GetComponent<UIDocument>().rootVisualElement.Q<Button>("exit-button");

            _startButton.RegisterCallback<ClickEvent>(_StartClicked);
            _optionsButton.RegisterCallback<ClickEvent>(_OptionsClicked);
            _exitButton.RegisterCallback<ClickEvent>(_ExitClicked);

            menu.panelHeaderText.Enter("PAUSE");
            _startButton.text = "CONTINUE";
        }

        protected override void OnExit()
        {
            HideVE("menu-container");
            HideVE("main-buttons-container");
            HideVE("panel-container");
            HideVE("panel-header");
            HideVE("panel");
            HideVE("mission-brief-container");

            _startButton.UnregisterCallback<ClickEvent>(_StartClicked);
            _optionsButton.UnregisterCallback<ClickEvent>(_OptionsClicked);
            _exitButton.UnregisterCallback<ClickEvent>(_ExitClicked);

            menu.panelHeaderText.Exit();
        }


        void _StartClicked(ClickEvent evt)
        {
            ((MenuRoot)Parent).TransitionToNotInMenu = true;
        }

        void _OptionsClicked(ClickEvent evt)
        {
            var parent = (MenuRoot)Parent;
            parent.TransitionToOptions = true;
        }

        void _ExitClicked(ClickEvent evt)
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene((int)Utils.SceneEnum.MainMenu);
        }
    }
}