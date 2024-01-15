using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



public class UIElement
{
    [JsonConverter(typeof(StringEnumConverter))]
    public UIElementType elementType;
    public string name,text;
    public SerializableVector2 position,rotation,scale;

    public UIElement(UIElementType elementType, string name, string text, Vector2 position, Vector2 rotation, Vector2 scale){
        this.elementType = elementType;
        this.name = name;
        this.text = text;
        this.position = new SerializableVector2(position);
        this.rotation = new SerializableVector2(rotation);
        this.scale = new SerializableVector2(scale);
    }
}
