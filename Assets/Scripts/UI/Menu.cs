using HSM;
using Player;
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

        var allElements = RootVE.Query<VisualElement>().ToList();
        foreach (VisualElement element in allElements)
        {
            element.style.display = DisplayStyle.None;
        }

        _root = new MenuRoot(null, this);
        var builder = new StateMachineBuilder(_root);
        _machine = builder.Build();
    }

    void Update()
    {
        _machine.Tick(Time.deltaTime);

        print(_root.PathToRoot().ToString());
    }
}
