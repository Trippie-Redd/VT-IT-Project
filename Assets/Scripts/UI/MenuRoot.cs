using HSM;

public class MenuRoot : State
{
    public readonly MainMenu mainMenu;

    public readonly PauseMenu pauseMenu;

    public readonly NotInMenu notInMenu;

    public MenuRoot(StateMachine machine, Menu menu) : base(machine, null)
    {
        mainMenu = new MainMenu(machine, this, menu);
        pauseMenu = new PauseMenu(machine, this, menu);
        notInMenu = new NotInMenu(machine, this, menu);
    }

    protected override State GetInitialState() => mainMenu;

}
