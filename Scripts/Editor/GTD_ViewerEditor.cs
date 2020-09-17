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

    bool setDescription = false;
    string entryName;
    string entryDesc;

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

        EditorGUILayout.Separator();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        showItems = EditorGUILayout.BeginToggleGroup("Show GoToDo's:", showItems);
        EditorGUILayout.Space();
        if (showItems)
        {
            EditorGUI.indentLevel = 0;
            if(Viewer.hasTODOEntries())
            {
                for (int i = 0; i < Viewer.Entries.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    entriesOpened[i] = EditorGUILayout.BeginToggleGroup(GetTODOEntryLabel(i), entriesOpened[i]);
                    if (entriesOpened[i])
                    {


                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("TODO Title:", CenteredLabel_TODOTitle());
                        EditorGUILayout.LabelField(Viewer.Entries[i].EntryName, CenteredLabel_TODOName());
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Separator();
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        Rect leftRect = new Rect(0 , 0, EditorGUIUtility.fieldWidth, 30);
                        EditorGUILayout.LabelField("Created date:", LeftLabel_CreatedDateLabel());
                        EditorGUILayout.LabelField(Viewer.Entries[i].createdTime, LeftLabel_CreatedDateLabel());
                        EditorGUILayout.EndHorizontal();
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
            }
            else
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("No TODOs yet :(", CenteredLabel_TODOTitle());
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
            entryName = EditorGUILayout.TextField("",entryName);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            GUILayout.FlexibleSpace();

            setDescription = EditorGUILayout.BeginToggleGroup("Use Description", setDescription);
            if (setDescription)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("New TODO description:", EditorStyles.boldLabel);
                entryDesc = EditorGUILayout.TextArea(entryDesc, GUILayout.Width(EditorGUIUtility.currentViewWidth-45), GUILayout.Height(100));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndToggleGroup();


            EditorGUILayout.BeginHorizontal();
            if(entryName != "")
            {
                if (GUILayout.Button("Add"))
                {
                    Debug.Log(entryName);
                    AddTODOEntry(entryName, entryDesc);
                    entryName = string.Empty;
                    isPanelOpened = false;
                    Debug.Log(entryName);
                }
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

    void AddTODOEntry(string _entryName, string _entryDesc)
    {
        GTD_TodoEntry newEntry = new GTD_TodoEntry();

        if(_entryName != "")
        {
            newEntry.EntryName = _entryName;
            newEntry.EntryDesc = _entryDesc;
            Debug.Log("Created: "+ newEntry.EntryName);
            Viewer.AddNewTodoEntry(newEntry);
        }
        else
        {
            Debug.Log("EntryName=null");
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
        result.fontSize = 13;
        result.normal.textColor = Color.white;
        return result;
    }

    GUIStyle LeftLabel_CreatedDateLabel()
    {
        GUIStyle result = new GUIStyle();
        result.alignment = TextAnchor.LowerLeft;
        result.fontSize = 9;
        result.normal.textColor = Color.black;
        return result;
    }

    #endregion
}
