using System.Diagnostics;
using HSM;
using UnityEngine.UIElements;

public class MainMenu : MenuState
{
    public readonly SettingsMenu settingsMenu;

    Button _startButton;
    Button _exitButton;
    Button _settingsButton;

    bool _transitionToSettings;

    public MainMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {
        settingsMenu = new SettingsMenu(machine, this, menu);
    }

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        root.Q<VisualElement>("main-buttons-container").style.display = DisplayStyle.Flex;
        root.Q<VisualElement>("spacer").style.display = DisplayStyle.Flex;

        _startButton = root.Q<Button>("start-button");
        Debug.Assert(_startButton != null);

        _settingsButton = root.Q<Button>("settings-button");
        Debug.Assert(_settingsButton != null);

        _exitButton = root.Q<Button>("exit-button");
        Debug.Assert(_exitButton != null);

        _startButton.RegisterCallback<ClickEvent>(_StartClicked);
        _settingsButton.RegisterCallback<ClickEvent>(_SettingsClicked);
        _exitButton.RegisterCallback<ClickEvent>(_ExitClicked);
    }

    protected override void OnExit()
    {
        menu.RootVE.Q<VisualElement>("main-buttons-container").style.display = DisplayStyle.None;
        menu.RootVE.Q<VisualElement>("spacer").style.display = DisplayStyle.None;

        _startButton.UnregisterCallback<ClickEvent>(_StartClicked);
        _settingsButton.UnregisterCallback<ClickEvent>(_SettingsClicked);
        _exitButton.UnregisterCallback<ClickEvent>(_ExitClicked);
    }

    protected override State GetTransition()
    {
        if (_transitionToSettings) return settingsMenu;
        else return null;
    }


    void _StartClicked(ClickEvent evt)
    {

    }

    void _SettingsClicked(ClickEvent evt)
    {
        _transitionToSettings = true;
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
