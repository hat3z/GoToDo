using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CustomEditor(typeof(GOTODO))]
/// <summary>
/// author@htz
/// </summary>

public class GTD_ViewerEditor : Editor
{
    Vector2 scrollPos;
    private GOTODO Viewer;
    bool showItems = false;
    List<bool> entriesOpened = new List<bool>();

    // New Entry panel
    bool isPanelOpened = false;

    bool setDescription = false;
    string entryName = "";
    string entryDesc = "";

    // Colors
    Color original = new Color();


    //Buttons
    Color customGreen = new Color(0.4f, 0.79f, 0.18f);
    Color customRed = new Color(0.8f, 0.12f, 0.07f);
    Color darkBackground = new Color(0.25f, 0.24f, 0.24f);

    public override void OnInspectorGUI()
    {
        // Startup
        Viewer = target as GOTODO;
        for (int i = 0; i < Viewer.Entries.Count; i++)
        {
            entriesOpened.Add(false);
        }

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
                    EditorGUILayout.BeginHorizontal();
                    //entriesOpened[i] = EditorGUILayout.BeginToggleGroup(GetTODOEntryLabel(i), entriesOpened[i]);
                    entriesOpened[i] = GUILayout.Toggle(entriesOpened[i], GetTODOEntryLabel(i));
                    GUI.backgroundColor = original;
                    if (Viewer.Entries[i].isCompleted)
                    {
                        EditorGUILayout.LabelField("Completed", EntryStatusLabel_Completed(), GUILayout.MaxWidth(100));
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Not Completed", EntryStatusLabel_NotCompleted(), GUILayout.MaxWidth(100));
                    }


                    GUI.backgroundColor = customRed;
                    if (GUILayout.Button("X", DeleteEntryButton(), GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                    {
                        ShowDialogEntry(i);
                    }
                    GUI.backgroundColor = original;

                    EditorGUILayout.EndHorizontal();

                    if (entriesOpened[i])
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("TODO Title:", CenteredLabel_TODOTitle());
                        EditorGUILayout.LabelField(Viewer.Entries[i].EntryName, EntryName_Viewer());
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Separator();
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("TODO Description:", CenteredLabel_TODOTitle());

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(EditorGUIUtility.currentViewWidth-70), GUILayout.Height(100));
                        GUILayout.Label(Viewer.Entries[i].EntryDesc, EntryDesc_Viewer(), GUILayout.ExpandHeight(true));
                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndVertical();
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

                        if(Viewer.Entries[i].isCompleted)
                        {

                            EditorGUILayout.BeginHorizontal();
                            Rect leftRect2 = new Rect(0, 0, EditorGUIUtility.fieldWidth, 30);
                            EditorGUILayout.LabelField("Completed date:", LeftLabel_CreatedDateLabel());
                            EditorGUILayout.LabelField(Viewer.Entries[i].createdTime, LeftLabel_CreatedDateLabel());
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.BeginHorizontal();
                        if(!Viewer.Entries[i].isCompleted)
                        {
                            if (GUILayout.Button("Set To Completed"))
                            {
                                Debug.Log("modifyEvent");
                                Viewer.SetEntryToCompleted(i);
                            }
                        }

                        EditorGUILayout.EndHorizontal();


                    }
                    //EditorGUILayout.EndToggleGroup();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }

            }
            else
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("No TODOs yet :( \n\n Click on 'Add New TODO' button. ", NoTODOSLabel());
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

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("New TODO title:", EditorStyles.boldLabel);
            entryName = EditorGUILayout.TextField("",entryName);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
            GUILayout.FlexibleSpace();

            setDescription = EditorGUILayout.BeginToggleGroup("Add Description", setDescription);
            if (setDescription)
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("New TODO description:", EditorStyles.boldLabel);
                entryDesc = EditorGUILayout.TextArea(entryDesc, GUILayout.MaxWidth(600), GUILayout.Height(100));
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
                    entryDesc = string.Empty;
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
        GUI.backgroundColor = customGreen;

        if(!isPanelOpened)
        {
            if (GUILayout.Button("Add new TODO", AddTODOButton(), GUILayout.MaxHeight(35)))
            {
                isPanelOpened = true;
            }
        }

        GUI.backgroundColor = original;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("GOTODO 1.0", FooterLabel_Left());
        EditorGUILayout.LabelField("htzprdcts", FooterLabel_Right(), GUILayout.MaxWidth(40));
        EditorGUILayout.EndHorizontal();

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
        bool option = EditorUtility.DisplayDialog("Are you sure?", "This will delete this TODO Entry.", "Ok", "Cancel");
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
        string result;
        result = "TODO - " + num;

        return result;
    }

    #region --- GUI STYLES ---

    GUIStyle NoTODOSLabel()
    {
        GUIStyle result = new GUIStyle();
        result.alignment = TextAnchor.MiddleCenter;
        result.fontStyle = FontStyle.Italic;
        result.normal.textColor = customRed;
        result.wordWrap = true;
        return result;
    }

    GUIStyle CenteredLabel_TODOTitle()
    {
        GUIStyle result = new GUIStyle();
        result.alignment = TextAnchor.MiddleCenter;
        result.fontStyle = FontStyle.Bold;
        result.normal.textColor = Color.black;
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

    GUIStyle EntryDesc_Viewer()
    {
        GUIStyle result = new GUIStyle();
        result.fontSize = 12;
        result.normal.textColor = Color.black;
        result.wordWrap = true;
        return result;
    }

    GUIStyle EntryName_Viewer()
    {
        GUIStyle result = new GUIStyle();
        result.fontSize = 12;
        result.alignment = TextAnchor.MiddleCenter;
        result.normal.textColor = Color.white;
        result.wordWrap = true;
        return result;
    }

    GUIStyle EntryStatusLabel_NotCompleted()
    {
        GUIStyle result = new GUIStyle();
        result.fontSize = 10;
        result.alignment = TextAnchor.MiddleCenter;
        result.normal.textColor = Color.yellow;
        return result;
    }

    GUIStyle EntryStatusLabel_Completed()
    {
        GUIStyle result = new GUIStyle();
        result.fontSize = 10;
        result.alignment = TextAnchor.MiddleCenter;
        result.normal.textColor = Color.white;
        GUI.backgroundColor = Color.clear;
        return result;
    }

    GUIStyle FooterLabel_Left()
    {
        GUIStyle result = new GUIStyle();
        result.fontSize = 8;
        result.alignment = TextAnchor.LowerLeft;
        result.normal.textColor = Color.grey;
        return result;
    }

    GUIStyle FooterLabel_Right()
    {
        GUIStyle result = new GUIStyle();
        
        result.fontSize = 8;
        result.alignment = TextAnchor.LowerLeft;
        result.normal.textColor = Color.grey;
        return result;
    }

    GUIStyle DeleteEntryButton()
    {
        GUIStyle result = new GUIStyle(GUI.skin.button);
        result.fontSize = 10;
        result.hover.textColor = Color.white;
        result.alignment = TextAnchor.MiddleCenter;
        result.padding.left = 7;
        result.padding.top = 3;
        result.normal.textColor = Color.white;
        return result;
    }

    GUIStyle AddTODOButton()
    {
        GUIStyle result = new GUIStyle(GUI.skin.button);
        result.fontSize = 12;
        result.hover.textColor = Color.white;
        result.alignment = TextAnchor.MiddleCenter;
        result.normal.textColor = Color.white;
        return result;
    }

    // To DARK Mode switching option
    //GUIStyle ApplicationBackgroundBox()
    //{
    //    GUIStyle result = new GUIStyle();
    //    result.fontSize = 12;
    //    result.normal.textColor = Color.white;
    //    result.alignment = TextAnchor.MiddleCenter;
    //    GUI.backgroundColor = Color.grey;
    //    return result;
    //}

    #endregion
}
