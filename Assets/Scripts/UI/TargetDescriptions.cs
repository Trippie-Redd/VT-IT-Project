using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TargetDescriptions : MonoBehaviour
{
    struct Target
    {
        public string name;
        public bool eliminated;
    }

    List<Target> _targets = new();

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var targetElements = root.Query<Image>(className: "menu-target-description").ToList();

        foreach(VisualElement element in targetElements)
        {
            throw new NotImplementedException();
        }
    }

    void OnDisable()
    {

    }
}
