using HSM;

public class MenuRoot : MenuState
{
    public readonly MainMenu mainMenu;

    public readonly PauseMenu pauseMenu;

    public readonly OptionsMenu optionsMenu;

    public readonly NotInMenu notInMenu;

    public bool TransitionToOptions { get; set; } = false;
    public bool TransitionToMainMenu { get; set; } = false;
    public bool TransitionToPauseMenu { get; set; } = false;

    State _stateBeforeOptions;

    public MenuRoot(StateMachine machine, Menu menu) : base(machine, null, menu)
    {
        mainMenu = new MainMenu(machine, this, menu);
        pauseMenu = new PauseMenu(machine, this, menu);
        notInMenu = new NotInMenu(machine, this, menu);
        optionsMenu = new OptionsMenu(machine, this, menu);
    }

    protected override State GetInitialState()
        => menu.inGame ? notInMenu : mainMenu;

    protected override State GetTransition()
    {
        if (TransitionToOptions)
        {
            TransitionToOptions = false;
            _stateBeforeOptions = ActiveChild;
            return optionsMenu;
        }

        if (optionsMenu.TransitionFromOptions)
        {
            optionsMenu.TransitionFromOptions = false;
            return _stateBeforeOptions ?? mainMenu;
        }

        if (TransitionToMainMenu)
        {
            TransitionToMainMenu = false;
            return mainMenu;
        }

        if (TransitionToPauseMenu)
        {
            TransitionToPauseMenu = false;
            return pauseMenu;
        }

        return ActiveChild;
    }
}
