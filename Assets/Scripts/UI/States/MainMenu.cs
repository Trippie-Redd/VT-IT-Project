using HSM;
using UnityEngine.UIElements;
using UnityEngine;

public class MainMenu : MenuState
{
    Button _startButton, _exitButton, _optionsButton;

    public MainMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {
    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        ShowVE("main-buttons-container");
        ShowVE("panel-container");
        ShowVE("menu-container");

        _startButton = root.Q<Button>("start-button");
        _optionsButton = root.Q<Button>("options-button");
        _exitButton = root.Q<Button>("exit-button");

        _startButton.RegisterCallback<ClickEvent>(_StartClicked);
        _optionsButton.RegisterCallback<ClickEvent>(_OptionsClicked);
        _exitButton.RegisterCallback<ClickEvent>(_ExitClicked);
    }

    protected override void OnExit()
    {
        HideVE("main-buttons-container");
        HideVE("panel-container");
        HideVE("menu-container");

        _startButton.UnregisterCallback<ClickEvent>(_StartClicked);
        _optionsButton.UnregisterCallback<ClickEvent>(_OptionsClicked);
        _exitButton.UnregisterCallback<ClickEvent>(_ExitClicked);
    }


    void _StartClicked(ClickEvent evt)
    {

    }

    void _OptionsClicked(ClickEvent evt)
    {
        ((MenuRoot)Parent).TransitionToOptions = true;
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
