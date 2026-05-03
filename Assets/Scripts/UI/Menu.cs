using HSM;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelHeaderText), typeof(UIDocument), typeof(AudioSource))]
public class Menu : MonoBehaviour
{
    public VisualElement RootVE { private set; get; }

    public bool inGame = false;

    public PanelHeaderText panelHeaderText;

    [SerializeField] AudioClip buttonClickSound;

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
    }

    void Update()
    {
        _machine.Tick(Time.deltaTime);
    }
}
