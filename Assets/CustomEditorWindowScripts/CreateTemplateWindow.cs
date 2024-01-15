using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateTemplateWindow : UnityEditor.EditorWindow
{
   [MenuItem("MyTools/Create Template")]
    public static void showCreateTemplateEditorWindow(){
         CreateTemplateWindow window = GetWindow<CreateTemplateWindow>();
         window.position = new Rect(100, 100, 500, 600);
    }
}
