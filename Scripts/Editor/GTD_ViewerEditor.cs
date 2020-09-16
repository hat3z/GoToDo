using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    List<bool> entriesOpened = new List<bool>();

    // New Entry panel
    bool isPanelOpened = false;
    GTD_TodoEntry newEntry = new GTD_TodoEntry();
    bool setDescription = false;

    // Colors
    Color original = new Color();
    Color green = new Color();
    Color red = new Color();

    public override void OnInspectorGUI()
    {
        // Startup
        Viewer = target as GTD_Viewer;
        for (int i = 0; i < Viewer.Entries.Count; i++)
        {
            entriesOpened.Add(false);
        }

        green = Color.green;
        red = Color.red;
        original = GUI.color;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        showItems = EditorGUILayout.BeginToggleGroup("Show GoToDo's:", showItems);
        EditorGUILayout.Space();
        if (showItems)
        {
            EditorGUI.indentLevel = 0;

            for (int i = 0; i <Viewer.Entries.Count; i++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                entriesOpened[i] = EditorGUILayout.BeginToggleGroup(GetTODOEntryLabel(i), entriesOpened[i]);
                if(entriesOpened[i])
                {
      

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("TODO Title:", CenteredLabel_TODOTitle());
                    EditorGUILayout.LabelField(Viewer.Entries[i].EntryName, CenteredLabel_TODOName());
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PrefixLabel("Created date:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.LabelField(Viewer.Entries[i].createdTime);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Modify"))
                    {
                        Debug.Log("modifyEvent");
                    }
                    GUI.backgroundColor = red;
                    if (GUILayout.Button("Delete"))
                    {
                        ShowDialogEntry(i);
                    }
                    GUI.backgroundColor = original;
                    EditorGUILayout.EndHorizontal();

       
                }
                EditorGUILayout.EndToggleGroup();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Separator();
        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        EditorGUILayout.Space();

        if (isPanelOpened)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("New TODO title:", EditorStyles.boldLabel);
            newEntry.EntryName = EditorGUILayout.TextField(newEntry.EntryName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            GUILayout.FlexibleSpace();

            setDescription = EditorGUILayout.BeginToggleGroup("Use Description", setDescription);
            if (setDescription)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("New TODO description:", EditorStyles.boldLabel);
                newEntry.EntryDesc = EditorGUILayout.TextArea(newEntry.EntryDesc, GUILayout.Width(EditorGUIUtility.currentViewWidth-30), GUILayout.Height(100));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndToggleGroup();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                AddTODOEntry(newEntry);
                isPanelOpened = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                isPanelOpened = false;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
        GUI.backgroundColor = green;

        if(!isPanelOpened)
        {
            if (GUILayout.Button("Add new TODO"))
            {
                isPanelOpened = true;
            }
        }

        GUI.backgroundColor = original;

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }

    void AddTODOEntry(GTD_TodoEntry _entryData)
    {
        if(_entryData.EntryName != string.Empty)
        {
            Viewer.AddNewTodoEntry(_entryData);
        }
    }

    public void ShowDialogEntry(int _itemIndex)
    {
        bool option = EditorUtility.DisplayDialog("Are you sure?", "This will delete the selected item from FoodItems", "Ok", "Cancel");
        if (option)
        {
            Viewer.Entries.RemoveAt(_itemIndex);
            GUIUtility.ExitGUI();           
        }
        else
        {
            GUIUtility.ExitGUI();
        }
    }

    string GetTODOEntryLabel(int _index)
    {
        int num = _index + 1;
        string result = "TODO - " + num;
        return result;
    }

    #region --- GUI STYLES ---
    GUIStyle CenteredLabel_TODOTitle()
    {
        GUIStyle result = new GUIStyle();
        result.alignment = TextAnchor.MiddleCenter;
        result.fontStyle = FontStyle.Bold;
        result.normal.textColor = Color.white;
        return result;
    }

    GUIStyle CenteredLabel_TODOName()
    {
        GUIStyle result = new GUIStyle();
        result.alignment = TextAnchor.MiddleCenter;
        result.fontSize = 14;
        result.normal.textColor = Color.white;
        return result;
    }
    #endregion
}
