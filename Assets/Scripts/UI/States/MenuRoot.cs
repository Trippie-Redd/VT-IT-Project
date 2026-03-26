using HSM;
using UnityEngine;

public class MenuRoot : MenuState
{
    public readonly MainMenu mainMenu;

    public readonly PauseMenu pauseMenu;

    public readonly OptionsMenu optionsMenu;

    public readonly NotInMenu notInMenu;

    public bool TransitionToOptions = false;
    public bool TransitionToMainMenu { get; set; } = false;
    public bool TransitionToPauseMenu { get; set; } = false;

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
        if (TransitionToOptions)
        {
            Debug.Log("Reached options");
            TransitionToOptions = false;
            return optionsMenu;
        }

        if (TransitionToMainMenu)
        {
            Debug.Log("Reached main");
            TransitionToMainMenu = false;
            return mainMenu;
        }

        if (TransitionToPauseMenu)
        {
            Debug.Log("Reached pause");
            TransitionToMainMenu = false;
            return pauseMenu;
        }

        return ActiveChild;
    }
}
