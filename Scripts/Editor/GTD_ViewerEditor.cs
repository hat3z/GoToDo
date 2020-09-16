using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CustomEditor(typeof(GTD_Viewer))]
/// <summary>
/// author@htz
/// </summary>

public class GTD_ViewerEditor : Editor
{
    Vector2 scrollPos;
    private GTD_Viewer Viewer;
    bool showItems = false;
    SerializedProperty GTDViewerList;

    // Colors
    Color original = new Color();
    Color green = new Color();
    Color red = new Color();

    private void OnEnable()
    {
        Viewer = target as GTD_Viewer;
        GTDViewerList = serializedObject.FindProperty("Entries");

        green = Color.green;
        red = Color.red;
        original = GUI.color;

    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUI.backgroundColor = green;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUI.backgroundColor = original;
        showItems = EditorGUILayout.BeginToggleGroup("Show GoToDo's:", showItems);
        if (showItems)
        {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;
            for (int i = 0; i < GTDViewerList.arraySize; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                SerializedProperty item = GTDViewerList.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(item, true);

                GUI.backgroundColor = red;
                if (GUILayout.Button("Delete"))
                {
                    ShowDialogEntry(i);
                }
                GUI.backgroundColor = original;
                GUILayout.EndVertical();
                EditorGUILayout.Space();

            }

            GUI.backgroundColor = green;
            if (GUILayout.Button("Add new Item to Database"))
            {
                Viewer.AddNewTodoEntry();
            }
            GUI.backgroundColor = original;

            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowDialogEntry(int _itemIndex)
    {
        bool option = EditorUtility.DisplayDialog("Are you sure?", "This will delete the selected item from FoodItems", "Ok", "Cancel");
        if (option)
        {
            Viewer.Entries.RemoveAt(_itemIndex);
            GUIUtility.ExitGUI();
            //EditorUtility.SetDirty(Viewer);
        }
    }
}
