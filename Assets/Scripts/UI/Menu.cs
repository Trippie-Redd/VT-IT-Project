using HSM;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    public VisualElement RootVE { private set; get; }

    public bool inGame = false;

    State _root;
    StateMachine _machine;

    void Start()
    {
        RootVE = GetComponent<UIDocument>().rootVisualElement;

        RootVE.Q<VisualElement>("menu-container").style.display           = DisplayStyle.None;
        RootVE.Q<VisualElement>("options-button-container").style.display = DisplayStyle.None;
        RootVE.Q<VisualElement>("main-buttons-container").style.display   = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel-container").style.display          = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel-header").style.display             = DisplayStyle.None;
        RootVE.Q<VisualElement>("panel").style.display                    = DisplayStyle.None;

        _root = new MenuRoot(null, this);
        var builder = new StateMachineBuilder(_root);
        _machine = builder.Build();
    }

    void Update()
    {
        _machine.Tick(Time.deltaTime);
    }
}
