using UnityEngine;
using UnityEngine.UIElements;

public class TextHighlighter : MonoBehaviour
{
    VisualElement _container;
    VisualElement _fill;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _container = root.Q<VisualElement>("highlight-container");
        _fill = root.Q<VisualElement>("highlight-fill");

        _container.RegisterCallback<PointerEnterEvent>(_OnPointerEnter);
        _container.RegisterCallback<PointerLeaveEvent>(_OnPointerLeave);
    }

    void OnDisable()
    {
        _container.UnregisterCallback<PointerEnterEvent>(_OnPointerEnter);
        _container.UnregisterCallback<PointerLeaveEvent>(_OnPointerLeave);
    }

    void _OnPointerEnter(PointerEnterEvent evt) =>
        _fill.AddToClassList("highlight-fill--active");

    void _OnPointerLeave(PointerLeaveEvent evt) =>
        _fill.RemoveFromClassList("highlight-fill--active");
}
