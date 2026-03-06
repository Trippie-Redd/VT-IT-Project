using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class OptionsButton : MonoBehaviour
{
    // PLACEHOLDER - WILL CHANGE

    Button _button;

    VisualElement _panelContainer;
    VisualElement _spacer;

    void OnEnable()
    {
        var document = GetComponent<UIDocument>();

        _button = document.rootVisualElement.Q<Button>("options-button");
        _panelContainer = document.rootVisualElement.Q<VisualElement>("panel-container");
        _spacer = document.rootVisualElement.Q<VisualElement>("spacer");

        Debug.Assert(_button != null && _panelContainer != null && _spacer != null);

        _button.RegisterCallback<ClickEvent>(_OnClick);
    }

    void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(_OnClick);
    }


    void _OnClick(ClickEvent evt)
    {
        if (_panelContainer.style.display == DisplayStyle.None)
        {
            _spacer.style.display = DisplayStyle.None;
            _panelContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            _panelContainer.style.display = DisplayStyle.None;
            _spacer.style.display = DisplayStyle.Flex;

        }
    }
}
