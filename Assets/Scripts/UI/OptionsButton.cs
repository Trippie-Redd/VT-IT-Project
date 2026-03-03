using UnityEngine;
using UnityEngine.UIElements;

public class OptionsButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("options-button").RegisterCallback<PointerDownEvent>(_OnClick);
    }

    void OnDisable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("options-button").UnregisterCallback<PointerDownEvent>(_OnClick);
    }

    void _OnClick(PointerDownEvent evt)
    {
        
    }
}
