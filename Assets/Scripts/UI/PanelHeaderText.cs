using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PanelHeaderText : MonoBehaviour
    {
        Label _panelHeaderText;

        List<string> _headerContent;

        void OnEnable()
        {
            _panelHeaderText = GetComponent<UIDocument>().rootVisualElement.Q<Label>("panel-header-text");
            Debug.Assert(_panelHeaderText != null);

            _headerContent = new List<string>();
        }

        public void Enter(string str)
        {
            _headerContent.Add(str);
            _UpdateText();
        }

        public void Exit()
        {
            _headerContent.RemoveAt(_headerContent.Count - 1);
            _UpdateText();
        }
        
        void _UpdateText()
        {
            if (_headerContent.Count == 0)
                return;
            
            string result = "";

            result += _headerContent[0].ToUpper();

            for (int i = 1; i < _headerContent.Count; i++)
            {
                result += "/";
                result += _headerContent[i].ToUpper();
            }

            _panelHeaderText.text = result;
        }
    }
}