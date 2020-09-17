﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// author@htz
/// </summary>

public class GTD_Viewer : MonoBehaviour
{

    public List<GTD_TodoEntry> Entries = new List<GTD_TodoEntry>();

    public void AddNewTodoEntry(GTD_TodoEntry _entryToAdd)
    {
        _entryToAdd.createdTime = DateTime.Now.ToShortDateString();
        Entries.Add(_entryToAdd);
    }

    public bool hasTODOEntries()
    {
        if(Entries.Count != 0)
        {
            return true;
        }
        return false;
    }

}

[Serializable]
public class GTD_TodoEntry
{
    protected int gameObjectHash;
    public string EntryName;
    public string EntryDesc;
    public string createdTime;
    public bool isCompleted;
    public string completedTime;

}

