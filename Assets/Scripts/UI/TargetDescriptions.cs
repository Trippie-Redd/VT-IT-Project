using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// TODO - Fix positioning of name label
[RequireComponent(typeof(UIDocument))]
public class TargetDescriptions : MonoBehaviour
{
    List<Image> _targets = new();

    VisualElement _root;

    Label _nameLabel;

    TargetsTracker _targetsTracker;

    bool _active;

    void Start()
    {
        if (!_active) _EnableTracker();
    }
    void OnEnable()
    {

        if (!_active) _EnableTracker();
    }
    
    // TODO - fix wack NullReferenceException 
    void _EnableTracker()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _targets = _root.Query<Image>(className: "menu-target-description").ToList();

        foreach (Image element in _targets)
        {
            _targetsTracker = FindFirstObjectByType<TargetsTracker>().GetComponent<TargetsTracker>();

            var target = TargetsTracker.EnumFromString(element.name);

            if (!_targetsTracker.IsAlive(target))
            {
                var grayscale = new FilterFunction(FilterFunctionType.Grayscale);
                grayscale.AddParameter(new FilterParameter(1.0f));

                // maybe remove on disable, don't think it's needed tho
                element.style.filter = new StyleList<FilterFunction>(new List<FilterFunction> { grayscale });
            }

            element.RegisterCallback<MouseEnterEvent>(_OnMouseEnter);
            element.RegisterCallback<MouseLeaveEvent>(_OnMouseLeave);
        }

        _nameLabel = new Label();

        _active = true;
    }

    void OnDisable()
    {
        foreach (Image element in _targets)
        {
            element.UnregisterCallback<MouseEnterEvent>(_OnMouseEnter);
            element.UnregisterCallback<MouseLeaveEvent>(_OnMouseLeave);
        }

        _active = false;
    }
    
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        _nameLabel.style.left = mousePos.x;
        _nameLabel.style.top = (Screen.height - mousePos.y);
    }

    void _OnMouseEnter(MouseEnterEvent evt)
    {
        var element = evt.target as VisualElement;
        string elementName = element.name;
        if (_targetsTracker.IsAlive(TargetsTracker.EnumFromString(elementName)))
        {
            _nameLabel.AddToClassList("menu-target-description-name");
            _nameLabel.text = elementName;
        }
        else
        {
            _nameLabel.AddToClassList("menu-target-description-name-dead");
            _nameLabel.text = elementName + "\n(eliminated)";
        }

        _root.Add(_nameLabel);
    }
    
    void _OnMouseLeave(MouseLeaveEvent evt)
    {
        _nameLabel.ClearClassList();
        _root.Remove(_nameLabel);
    }
}
