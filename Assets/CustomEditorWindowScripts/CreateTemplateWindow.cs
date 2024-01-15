using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class CreateTemplateWindow : UnityEditor.EditorWindow
{
    string name,text,fileName,filePath;
    Vector2 _position = Vector2.zero;
    Vector2 _rotation = Vector2.zero;
    Vector2 _scale = new Vector2(2,2);
    Vector2 scrollPosition;
    private int selectedOptionIndex;
    TemplateClass template = new TemplateClass();
    TemplateClass childTemplate = new TemplateClass();

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

        CreateTemplateFile();

        SelectTemplateFile();

        if(GUILayout.Button("Instantiate this template")){
            instatiateTemplate();
        }
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
            element.position = new SerializableVector2(EditorGUILayout.Vector2Field("Position: ",element.position.GetVector2()));
            element.rotation = new SerializableVector2(EditorGUILayout.Vector2Field("Rotation: ",element.rotation.GetVector2()));
            element.scale = new SerializableVector2(EditorGUILayout.Vector2Field("Scale: ",element.scale.GetVector2()));
            if(element.instantiatedElement!=null && GUILayout.Button("Update this element")){
                element.instantiatedElement.name = element.name;
                element.instantiatedElement.transform.localPosition = element.position.GetVector2();
                element.instantiatedElement.transform.localRotation = element.rotation.GetQuaternion();
                element.instantiatedElement.transform.localScale = element.scale.GetVector2();
                switch(element.elementType){
                    case UIElementType.Text:
                        element.instantiatedElement.GetComponent<Text>().text = element.text;
                        break;
                    case UIElementType.InputField:
                        element.instantiatedElement.transform.GetChild(1).GetComponent<Text>().text = element.text;
                        break;
                    case UIElementType.Button:
                        element.instantiatedElement.transform.GetChild(0).GetComponent<Text>().text = element.text;
                        break;
                    }
            }
            if(GUILayout.Button("Add Child Template")){
                filePath = EditorUtility.OpenFilePanel("Select File", "", "");
                if(!string.IsNullOrWhiteSpace(filePath)){
                    childTemplate = JsonConvert.DeserializeObject<TemplateClass>(File.ReadAllText(filePath));
                    element.children.AddRange(childTemplate.elements);
                }
                
            }
            if(GUILayout.Button("Remove this element")) elements.Remove(element);
            string indentation = new string(' ', depth * 8);
            GUILayout.Label(indentation + "children:[");
            printEditableTemplate(element.children,depth + 1);
            GUILayout.Label(indentation + "]");
        }
    }

    void CreateTemplateFile(){
        fileName = EditorGUILayout.TextField("File Name: ",fileName);
        GUILayout.Label("Type something in Fine Name to enable the 'Create Template' Button");
        try{
            if(!string.IsNullOrWhiteSpace(fileName) && GUILayout.Button("Create Template")){
                File.WriteAllText(Application.dataPath + "/UI Templates/" + fileName + ".txt",JsonConvert.SerializeObject(template));
            }
        }catch(Exception e){
            Debug.Log("Couldn't create template\n" + e.Message);
        }
    }

    void SelectTemplateFile(){
        try{
            if(GUILayout.Button("Select File")){
                filePath = EditorUtility.OpenFilePanel("Select File", "", "");
                if(!string.IsNullOrWhiteSpace(filePath)){
                    template = JsonConvert.DeserializeObject<TemplateClass>(File.ReadAllText(filePath));
                }
            }
        }catch(Exception e){
            Debug.Log("Couldn't load template \n" + e.Message);
        }
    }

    void instatiateTemplate(){
        Canvas canvas = CreateCanvas();
        instantiateChildrenElements(template.elements, canvas.transform);
    }

    void instantiateChildrenElements(List<UIElement> elements, Transform parent){
        foreach(UIElement element in elements){
            switch(element.elementType){
            case UIElementType.Text:
                var textElement = CreateText(element,parent);
                element.instantiatedElement = textElement.textGO;
                instantiateChildrenElements(element.children, textElement.textGO.transform);
                break;
            case UIElementType.InputField:
                var inputFieldElement = CreateInputField(element,parent);
                element.instantiatedElement = inputFieldElement.inputFieldGO;
                instantiateChildrenElements(element.children, inputFieldElement.inputFieldGO.transform);
                break;
            case UIElementType.Button:
                var buttonElement = CreateButton(element,parent);
                element.instantiatedElement = buttonElement.buttonGO;
                instantiateChildrenElements(element.children, buttonElement.buttonGO.transform);
                break;
            }
        }
    }

    Canvas CreateCanvas()
    {
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    (Text textElement, GameObject textGO) CreateText(UIElement element, Transform parent)
    {
        GameObject textGO = new GameObject(element.name);
        textGO.transform.SetParent(parent);
        textGO.transform.localPosition = element.position.GetVector2();
        textGO.transform.localRotation = element.rotation.GetQuaternion();
        textGO.transform.localScale = element.scale.GetVector2()/(parent.transform.localScale==Vector3.zero?Vector3.one:parent.transform.localScale);

        Text textElement = textGO.AddComponent<Text>();
        textElement.text = element.text;
        textElement.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        return (textElement,textGO);
    }

    (Button buttonElement, GameObject buttonGO) CreateButton(UIElement element, Transform parent)
    {
        GameObject buttonGO = new GameObject(element.name);

        buttonGO.transform.SetParent(parent);

        buttonGO.transform.localPosition = element.position.GetVector2();
        buttonGO.transform.localRotation = element.rotation.GetQuaternion();
        buttonGO.transform.localScale = element.scale.GetVector2()/(parent.transform.localScale==Vector3.zero?Vector3.one:parent.transform.localScale);
        Image backgroundImage = buttonGO.AddComponent<Image>();
        backgroundImage.color = Color.white;

        Button buttonElement = buttonGO.AddComponent<Button>();

        Text textElement = CreateTextComponent(buttonGO.transform, "ButtonText", element.text,Color.black);
        textElement.text = element.text;

        RectTransform textRectTransform = textElement.rectTransform;
        textRectTransform.sizeDelta = new Vector2(200f, 30f);

        RectTransform imageRectTransform = backgroundImage.rectTransform;
        imageRectTransform.sizeDelta = new Vector2(200f * element.scale.x, 30f * element.scale.y);

        imageRectTransform.anchorMin = textRectTransform.anchorMin;
        imageRectTransform.anchorMax = textRectTransform.anchorMax;
        imageRectTransform.pivot = textRectTransform.pivot;

        return (buttonElement, buttonGO);
    }

    (InputField inputFieldElement, GameObject inputFieldGO) CreateInputField(UIElement element, Transform parent)
    {
        GameObject inputFieldGO = new GameObject(element.name);

        inputFieldGO.transform.SetParent(parent);

        inputFieldGO.transform.localScale = Vector2.one;
        inputFieldGO.transform.localPosition = element.position.GetVector2();
        inputFieldGO.transform.localRotation = element.rotation.GetQuaternion();
        inputFieldGO.transform.localScale = element.scale.GetVector2()/(parent.transform.localScale==Vector3.zero?Vector3.one:parent.transform.localScale);

        InputField inputFieldElement = inputFieldGO.AddComponent<InputField>();
        Image backgroundImage = inputFieldGO.AddComponent<Image>();
        backgroundImage.color = Color.white;

        inputFieldElement.textComponent = CreateTextComponent(inputFieldGO.transform, "Text","",Color.black);
        inputFieldElement.placeholder = CreateTextComponent(inputFieldGO.transform, "Placeholder",element.text,Color.grey);

        RectTransform textRectTransform = inputFieldElement.textComponent.rectTransform;
        textRectTransform.sizeDelta = new Vector2(200f, 30f);
        
        RectTransform placeholderRectTransform = inputFieldElement.placeholder.rectTransform;
        placeholderRectTransform.sizeDelta = new Vector2(200f, 30f);

        RectTransform imageRectTransform = backgroundImage.rectTransform;
        imageRectTransform.sizeDelta = new Vector2(200f * element.scale.x, 30f * element.scale.y);

        imageRectTransform.anchorMin = textRectTransform.anchorMin;
        imageRectTransform.anchorMax = textRectTransform.anchorMax;
        imageRectTransform.pivot = textRectTransform.pivot;

        return (inputFieldElement,inputFieldGO);
    }

     Text CreateTextComponent(Transform parent, string name ,string defaultText, Color textColor)
    {
        GameObject textGO = new GameObject(name);

        textGO.transform.SetParent(parent);
        textGO.transform.localPosition = parent.localPosition;
        textGO.transform.localRotation = parent.localRotation;
        textGO.transform.localScale = parent.localScale;

        Text textComponent = textGO.AddComponent<Text>();
        textComponent.text = defaultText;
        textComponent.color = textColor;

        return textComponent;
    }
}
