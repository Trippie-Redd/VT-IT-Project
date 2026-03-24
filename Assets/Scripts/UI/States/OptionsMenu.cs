using HSM;
using UnityEngine.UIElements;

public class OptionsMenu : MenuState
{
    public readonly AudioOptionsMenu audioOptionsMenu;
    public readonly GameplayOptionsMenu gameplayOptionsMenu;
    public readonly ControlsOptionsMenu controlsOptionsMenu;

    Button _audioButton, _controlsButton, _gameplayButton, _returnButton;

    bool _transitionToAudio, _transitionToGameplay, _transitionToControls;
    
    public bool TransitionFromOptions { get; set; }

    public OptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {
        audioOptionsMenu = new AudioOptionsMenu(machine, this, menu);
        gameplayOptionsMenu = new GameplayOptionsMenu(machine, this, menu);
        controlsOptionsMenu = new ControlsOptionsMenu(machine, this, menu);
    }

    protected override State GetInitialState() => audioOptionsMenu;

    protected override void OnEnter()
    {
        var root = menu.RootVE;

        root.Q<VisualElement>("options-container").style.display = DisplayStyle.Flex;

        _audioButton = root.Q<Button>("audio-button");
        _gameplayButton = root.Q<Button>("gameplay-button");
        _controlsButton = root.Q<Button>("controls-button");
        _returnButton = root.Q<Button>("return-button");
        

        _audioButton.RegisterCallback<ClickEvent>(_AudioClicked);
        _gameplayButton.RegisterCallback<ClickEvent>(_GameplayClicked);
        _controlsButton.RegisterCallback<ClickEvent>(_ControlsClicked);
        _returnButton.RegisterCallback<ClickEvent>(_ReturnClicked);
    }


    protected override void OnExit()
    {
        menu.RootVE.Q<VisualElement>("options-container").style.display = DisplayStyle.None;

        _audioButton.UnregisterCallback<ClickEvent>(_AudioClicked);
        _gameplayButton.UnregisterCallback<ClickEvent>(_GameplayClicked);
        _controlsButton.UnregisterCallback<ClickEvent>(_ControlsClicked);
        _returnButton.UnregisterCallback<ClickEvent>(_ReturnClicked);
    }

    protected override State GetTransition()
    {
        State result = null;

        if (_transitionToAudio)         result = audioOptionsMenu;
        else if (_transitionToGameplay) result = gameplayOptionsMenu;
        else if (_transitionToControls) result = controlsOptionsMenu;

        return result;
    }

    void _AudioClicked(ClickEvent evt)
    {
        _transitionToGameplay = false;
        _transitionToControls = false;
        
        _transitionToAudio = true;
    }

    void _GameplayClicked(ClickEvent evt)
    {
        _transitionToAudio = false;
        _transitionToControls = false;
        
        _transitionToGameplay = true;
    }

    void _ControlsClicked(ClickEvent evt)
    {
        _transitionToAudio = false;
        _transitionToGameplay = false;

        _transitionToControls = true;
    }
    
    void _ReturnClicked(ClickEvent evt)
    {
        TransitionFromOptions = true;
    }
}
