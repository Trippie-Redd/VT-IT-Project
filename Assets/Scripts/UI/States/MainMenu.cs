using System.Diagnostics;
using HSM;
using UnityEngine.UIElements;

public class MainMenu : MenuState
{
    public readonly OptionsMenu optionsMenu;

    Button _startButton, _exitButton, _optionsButton;

    public bool TransitionToOptions { set; get; } 

    public MainMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {
        optionsMenu = new OptionsMenu(machine, this, menu);
    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        root.Q<VisualElement>("main-buttons-container").style.display = DisplayStyle.Flex;
        root.Q<VisualElement>("panel-container").style.display = DisplayStyle.Flex;

        _startButton = root.Q<Button>("start-button");
        _optionsButton = root.Q<Button>("options-button");
        _exitButton = root.Q<Button>("exit-button");

        _startButton.RegisterCallback<ClickEvent>(_StartClicked);
        _optionsButton.RegisterCallback<ClickEvent>(_OptionsClicked);
        _exitButton.RegisterCallback<ClickEvent>(_ExitClicked);
    }

    protected override void OnExit()
    {
        menu.RootVE.Q<VisualElement>("main-buttons-container").style.display = DisplayStyle.None;
        menu.RootVE.Q<VisualElement>("panel-container").style.display = DisplayStyle.None;

        _startButton.UnregisterCallback<ClickEvent>(_StartClicked);
        _optionsButton.UnregisterCallback<ClickEvent>(_OptionsClicked);
        _exitButton.UnregisterCallback<ClickEvent>(_ExitClicked);
    }


    void _StartClicked(ClickEvent evt)
    {

    }

    void _OptionsClicked(ClickEvent evt)
    {
        TransitionToOptions = true;
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
