using Enemy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    // TODO - Fix positioning of name label
    [RequireComponent(typeof(UIDocument))]
    public class TargetDescriptions : MonoBehaviour
    {
        List<Image> _targets = new();
        List<string> _targetNames = new();

        VisualElement _root;

        Label _nameLabel;

        TargetsTracker _targetsTracker;

        bool _active;

        // this mad goofy
        void Awake()
        {
            if (!_active) _EnableTracker();
        }

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
                _targetNames.Add(element.name);

                element.RegisterCallback<MouseEnterEvent>(_OnMouseEnter);
                element.RegisterCallback<MouseLeaveEvent>(_OnMouseLeave);
            }

            _nameLabel = new Label();

            _active = true;
        }

        void OnDisable()
        {
            _DisableTracker();
        }

        void _DisableTracker()
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

            // this is inefficient and should NOT be done here but im lazy
            foreach (var name in _targetNames)
            {
                Image image = _root.Q<Image>(name);

                Target target = TargetsTracker.EnumFromString(name);
                if (!_targetsTracker.IsAlive(target))
                {
                    var grayscale = new FilterFunction(FilterFunctionType.Grayscale);
                    grayscale.AddParameter(new FilterParameter(1.0f));

                    image.style.filter = new StyleList<FilterFunction>(new List<FilterFunction> { grayscale });

                    _targetNames.Remove(name);
                }
            }
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
}