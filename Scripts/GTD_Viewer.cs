using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// author@htz
/// </summary>

public class GTD_Viewer : MonoBehaviour
{

    public List<GTD_TodoEntry> Entries = new List<GTD_TodoEntry>();

    public void AddNewTodoEntry()
    {
        GTD_TodoEntry entryToAdd = new GTD_TodoEntry();
        Entries.Add(entryToAdd);
    }

}

[Serializable]
public class GTD_TodoEntry
{
    protected int gameObjectHash;
    public string EntryName;
    public string EntryDesc;
    public DateTime createdTime;
    public bool isCompleted;
    public DateTime completedTime;

}

