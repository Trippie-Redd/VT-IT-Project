using UnityEngine;
using UnityEngine.UIElements;

public class StartButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("start-button").RegisterCallback<PointerDownEvent>(_OnClick);
    }

    void OnDisable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q("start-button").UnregisterCallback<PointerDownEvent>(_OnClick);
    }

    void _OnClick(PointerDownEvent evt)
    {
        
    }
}
