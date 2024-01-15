using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class CreateTemplateWindow : UnityEditor.EditorWindow
{
    string name,text;
    Vector2 _position = Vector2.zero;
    Vector2 _rotation = Vector2.zero;
    Vector2 _scale = new Vector2(2,2);
    Vector2 scrollPosition;
    private int selectedOptionIndex;
    TemplateClass template = new TemplateClass();

   [MenuItem("MyTools/Create Template")]
    public static void showCreateTemplateEditorWindow(){
         CreateTemplateWindow window = GetWindow<CreateTemplateWindow>();
         window.position = new Rect(100, 100, 500, 600);
    }

    public void OnGUI(){
        AddElementUI();
        GUILayout.Label("Template Preview(Add elements to see the preview)");
         scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Width(0), GUILayout.Height(0));
        printEditableTemplate(template.elements,0);
        GUILayout.EndScrollView();
    }

    void AddElementUI(){
        selectedOptionIndex = EditorGUILayout.Popup("Select UI element type: ", selectedOptionIndex, Enum.GetNames(typeof(UIElementType)));
        name = EditorGUILayout.TextField("Name: ",name);
        text = EditorGUILayout.TextField("Text: ",text);
        _position = EditorGUILayout.Vector2Field("Position: ",_position);
        _rotation = EditorGUILayout.Vector2Field("Rotation: ",_rotation);
        _scale = EditorGUILayout.Vector2Field("Scale: ",_scale);
        if(GUILayout.Button("Add Element")){
            if(!string.IsNullOrWhiteSpace(name))
                template.elements.Add(new UIElement((UIElementType)selectedOptionIndex,name,text,_position,_rotation,_scale));
        }

    }

    void printEditableTemplate(List<UIElement> elements,int depth){
        foreach(UIElement element in elements){
            element.name = EditorGUILayout.TextField("Name: ",element.name);
            element.text = EditorGUILayout.TextField("Text: ",element.text);
            element.position = EditorGUILayout.Vector2Field("Position: ",element.position);
            element.rotation = EditorGUILayout.Vector2Field("Rotation: ",element.rotation);
            element.scale = EditorGUILayout.Vector2Field("Scale: ",element.scale);
            if(GUILayout.Button("Remove this element")) elements.Remove(element);
        }
    }
}
