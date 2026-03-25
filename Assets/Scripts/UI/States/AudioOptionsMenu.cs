using HSM;
using UnityEngine.UIElements;

public class AudioOptionsMenu : MenuState
{
    public AudioOptionsMenu(StateMachine machine, State parent, Menu menu) : base(machine, parent, menu)
    {

    }

    protected override void OnEnter()
    {
        ShowVE("audio-options-container");
    }


    protected override void OnExit()
    {
        HideVE("audio-options-container");
    }
}
