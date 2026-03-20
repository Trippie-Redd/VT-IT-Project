using HSM;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    public VisualElement RootVE { private set; get; }

    State _root;
    StateMachine _machine;

    void Start()
    {
        RootVE = GetComponent<UIDocument>().rootVisualElement;
        Debug.Assert(RootVE != null);

        _root = new MenuRoot(null, this);
        var builder = new StateMachineBuilder(_root);
        _machine = builder.Build();
    }

    void Update()
    {
        _machine.Tick(Time.deltaTime);
    }
}
