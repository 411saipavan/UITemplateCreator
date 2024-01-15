using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TemplateClass
{
    public List<UIElement> elements;
    public TemplateClass(){
        this.elements = new List<UIElement>();
    }
}
