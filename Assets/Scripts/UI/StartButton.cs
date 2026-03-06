using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class StartButton : MonoBehaviour
{
    Button _button;

    void OnEnable()
    {
        var document = GetComponent<UIDocument>();

        _button = document.rootVisualElement.Q<Button>("start-button");

        Debug.Assert(_button != null);

        _button.RegisterCallback<ClickEvent>(_OnClick);
    }

    void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(_OnClick);
    }

    void _OnClick(ClickEvent evt)
    {
    }

}
