using UnityEngine;
using UnityEngine.UIElements;

public class ExitButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("exit-button").RegisterCallback<PointerDownEvent>(_OnClick);
    }

    void OnDisable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("exit-button").UnregisterCallback<PointerDownEvent>(_OnClick);
    }

    void _OnClick(PointerDownEvent evt)
    {
        
    }
}
