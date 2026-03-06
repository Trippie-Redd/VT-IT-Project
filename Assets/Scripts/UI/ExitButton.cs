using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ExitButton : MonoBehaviour
{
    Button _button;

    void OnEnable()
    {
        _button = GetComponent<UIDocument>().rootVisualElement.Q<Button>("exit-button");
        _button.RegisterCallback<ClickEvent>(_OnClick);
    }

    void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(_OnClick);
    }

    void _OnClick(ClickEvent evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
