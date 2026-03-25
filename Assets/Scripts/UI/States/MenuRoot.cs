using HSM;
using UnityEngine;

public class MenuRoot : MenuState
{
    public readonly MainMenu mainMenu;

    public readonly PauseMenu pauseMenu;

    public readonly OptionsMenu optionsMenu;

    public readonly NotInMenu notInMenu;

    public MenuRoot(StateMachine machine, Menu menu) : base(machine, null, menu)
    {
        mainMenu = new MainMenu(machine, this, menu);
        pauseMenu = new PauseMenu(machine, this, menu);
        notInMenu = new NotInMenu(machine, this, menu);
        pauseMenu = new PauseMenu(machine, this, menu);
    }

    protected override State GetInitialState()
        => menu.inGame ? notInMenu : mainMenu;

    protected override State GetTransition()
    {
        State result = this;

        if (ActiveChild is MainMenu)
        {
            if (mainMenu.TransitionToOptions)
            {
                mainMenu.TransitionToOptions = false;
                result = optionsMenu;
            }
        }
        else if (ActiveChild is PauseMenu)
        {
            if (pauseMenu.TransitionToOptions)
            {
                pauseMenu.TransitionToOptions = false;
                result = optionsMenu;
            }
        }
        else if (ActiveChild is not NotInMenu)
        {
            if (optionsMenu.TransitionFromOptions)
            {
                optionsMenu.TransitionFromOptions = false;

                result = menu.inGame
                    ? pauseMenu
                    : mainMenu;
            }
        }

        return result;
    }
}
