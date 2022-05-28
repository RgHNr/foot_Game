using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;



//public class AddPostions : EditorWindow
//{

    
//    public FormationPlayersUI formation;
//    public GameObject[] players=new GameObject[4];
    
//    string test;
//    [MenuItem("Window/UI Toolkit/AddPostions")]
//    public static void ShowExample()
//    {
//        AddPostions wnd = GetWindow<AddPostions>();
//        wnd.titleContent = new GUIContent("AddPostions");
//    }


//    private void OnGUI()
//    {
        
//        test = EditorGUILayout.TextField("test 2");
//        formation = EditorGUILayout.ObjectField(formation, typeof( FormationPlayersUI), GUILayout.ExpandWidth(true))as FormationPlayersUI;

//        SerializedObject so = new SerializedObject(this);
//        SerializedProperty sp= so.FindProperty("players");
//        EditorGUILayout.PropertyField(sp, true);
//        so.ApplyModifiedProperties();




//        if (GUILayout.Button("makeformation", GUILayout.Width(200))) {

//            for (int i = 0; i < players.Length; i++)
//            {
//                formation.PlayersPositions[i] = players[i].transform.position;
//            }
//            EditorUtility.SetDirty(formation);

//        }
//    }
//    //public void CreateGUI()
//    //{
//    //    // Each editor window contains a root VisualElement object
//    //    VisualElement root = rootVisualElement;

//    //    // VisualElements objects can contain other VisualElement following a tree hierarchy.
//    //    VisualElement label = new Label("put Positons");
//    //    root.Add(label);

//    //    VisualElement text = new ObjectField("test text");
//    //    root.Add(text);
        
        
//    //    VisualElement button = new Button();
//    //    button.Add(label);
//    //    root.Add(button);
//    //    // Import UXML
//    //    var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/AddPostions.uxml");
//    //    VisualElement labelFromUXML = visualTree.Instantiate();
//    //    root.Add(labelFromUXML);

//    //    // A stylesheet can be added to a VisualElement.
//    //    // The style will be applied to the VisualElement and all of its children.
//    //    var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/AddPostions.uss");
//    //    VisualElement labelWithStyle = new Label("Hello World! With Style");
//    //    labelWithStyle.styleSheets.Add(styleSheet);
//    //    root.Add(labelWithStyle);
//    //}
//}