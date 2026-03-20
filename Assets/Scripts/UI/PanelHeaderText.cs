using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PanelHeaderText : MonoBehaviour
{
    Label _panelHeaderText;

    List<string> _headerContent;

    void OnEnable()
    {
        _panelHeaderText = GetComponent<UIDocument>().rootVisualElement.Q<Label>("panel-header-text");
        Debug.Assert(_panelHeaderText != null);
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
        string result = "";

        result += _headerContent[0].ToUpper();

        for (int i = 1; i < _headerContent.Count; i++)
        {
            result += "/ ";
            result += _headerContent[i].ToUpper();
        }

        _panelHeaderText.text = result;
    }
}
