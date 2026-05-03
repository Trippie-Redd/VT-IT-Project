using HSM;
using UnityEngine.UIElements;
using UnityEngine;

public class PauseMenu : MenuState
{
    Button _startButton, _exitButton, _optionsButton;

    public PauseMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {
    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        ShowVE("menu-container");
        ShowVE("main-buttons-container");
        ShowVE("panel-container");
        ShowVE("panel-header");
        ShowVE("panel");
        ShowVE("mission-brief-container");

        _startButton = root.Q<Button>("start-button");
        _optionsButton = root.Q<Button>("options-button");
        _exitButton = root.Q<Button>("exit-button");

        _startButton.RegisterCallback<ClickEvent>(_StartClicked);
        _optionsButton.RegisterCallback<ClickEvent>(_OptionsClicked);
        _exitButton.RegisterCallback<ClickEvent>(_ExitClicked);
        
        menu.panelHeaderText.Enter("PAUSE");
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

    }

    void _OptionsClicked(ClickEvent evt)
    {
        var parent = (MenuRoot)Parent;
        parent.TransitionToOptions = true;
    }

    void _ExitClicked(ClickEvent evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
