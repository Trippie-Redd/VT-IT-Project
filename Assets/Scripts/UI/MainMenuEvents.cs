using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Diagnostics.Debug;

[RequireComponent(typeof(UIDocument))]
public class MainMenuEvents : MonoBehaviour
{
    UIDocument _document;

    List<Button> _menuButtons = new();

    Button _startButton;
    Button _settingsButton;
    Button _exitButton;

    void Awake()
    {
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _settingsButton = _document.rootVisualElement.Q("SettingsButton") as Button;
        _exitButton = _document.rootVisualElement.Q("ExitButton") as Button;

        Assert(_startButton != null);
        Assert(_settingsButton != null);
        Assert(_exitButton != null);

        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        _menuButtons.ForEach(button =>
        {
            button.RegisterCallback<ClickEvent>(OnMenuButtonClicked);
        });
    }

    void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);

        _menuButtons.ForEach(button =>
        {
            button.UnregisterCallback<ClickEvent>(OnMenuButtonClicked);
        });
    }

    void OnMenuButtonClicked(ClickEvent evt)
    {
    }

    void OnStartButtonClicked(ClickEvent evt)
    {
    }

    void OnSettingsButtonClicked(ClickEvent evt)
    {
    }

    void OnExitButtonClicked(ClickEvent evt)
    {
    }
}
