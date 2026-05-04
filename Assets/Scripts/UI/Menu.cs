using HSM;
using Input;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelHeaderText), typeof(UIDocument), typeof(AudioSource))]
public class Menu : MonoBehaviour
{
    public VisualElement RootVE { private set; get; }

    public bool inGame = false;

    public PanelHeaderText panelHeaderText;

    public GameObject HUD;

    [SerializeField] AudioClip buttonClickSound;
    [SerializeField] InputReader inputReader;
    public InputReader InputReader => inputReader;

    AudioSource _audioSource;

    State _root;
    StateMachine _machine;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        RootVE = GetComponent<UIDocument>().rootVisualElement;
        RootVE.RegisterCallback<ClickEvent>(evt =>
        {
            if (evt.target is Button && buttonClickSound != null)
                _audioSource.PlayOneShot(buttonClickSound);
        });

        RootVE.Q<VisualElement>("menu-container").style.display           = DisplayStyle.None;
        RootVE.Q<VisualElement>("options-button-container").style.display = DisplayStyle.None;
        RootVE.Q<VisualElement>("main-buttons-container").style.display   = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel-container").style.display          = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel-header").style.display             = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel").style.display                    = DisplayStyle.None;

        panelHeaderText = GetComponent<PanelHeaderText>();

        _root = new MenuRoot(null, this);
        var builder = new StateMachineBuilder(_root);
        _machine = builder.Build();

        if (inGame && InputReader != null)
            InputReader.Pause += _OnPause;
    }

    void OnDestroy()
    {
        if (inGame && InputReader != null)
            InputReader.Pause -= _OnPause;
    }

    void Update()
    {
        _machine.Tick(Time.deltaTime);
    }

    void _OnPause()
    {
        var root = (MenuRoot)_root;
        if (root.ActiveChild is NotInMenu)
            root.TransitionToPauseMenu = true;
        else if (root.ActiveChild is PauseMenu)
            root.TransitionToNotInMenu = true;
    }
}
