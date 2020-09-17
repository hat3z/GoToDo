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

    public void AddNewTodoEntry(GTD_TodoEntry _entryToAdd)
    {
        _entryToAdd.createdTime = DateTime.Now.ToShortDateString();
        Entries.Add(_entryToAdd);
    }

    public GTD_TodoEntry GetEntryByIndex(int _index)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if (i == _index)
            {
                return Entries[i];
            }
        }
        return null;
    }

    public bool hasTODOEntries()
    {
        if(Entries.Count != 0)
        {
            return true;
        }
        return false;
    }

    public void SetEntryToCompleted(int _index)
    {
        for (int i = 0; i < Entries.Count; i++)
        {
            if(i == _index)
            {
                Entries[i].isCompleted = true;
                Entries[i].completedTime = DateTime.Now.ToShortDateString();
            }
        }
    }

}

[Serializable]
public class GTD_TodoEntry
{
    public string EntryName;
    public string EntryDesc;
    public string createdTime;
    public bool isCompleted;
    public string completedTime;

}

