using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// author@htz
/// </summary>


public class GOTODO : MonoBehaviour
{

    public List<GTD_TodoEntry> Entries = new List<GTD_TodoEntry>();

    public List<GTD_TodoEntry> NotCompletedEntries = new List<GTD_TodoEntry>();
    public List<GTD_TodoEntry> CompletedEntries = new List<GTD_TodoEntry>();

    StringRandomizer sr;
    public void AddNewTodoEntry(GTD_TodoEntry _entryToAdd)
    {
         sr= new StringRandomizer();
        _entryToAdd.createdTime = DateTime.Now.ToShortDateString();
        _entryToAdd.ID = sr.GetRandomString(4);
        Entries.Add(_entryToAdd);
        NotCompletedEntries.Add(_entryToAdd);
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

    public GTD_TodoEntry GetCompletedEntryByIndex(int _index)
    {
        for (int i = 0; i < CompletedEntries.Count; i++)
        {
            if(CompletedEntries[i].ID == GetEntryByIndex(_index).ID)
            {
                return CompletedEntries[i];
            }
        }
        return null;
    }

    public GTD_TodoEntry GetNotCompletedEntryByIndex(int _index)
    {
        for (int i = 0; i < NotCompletedEntries.Count; i++)
        {
            if (NotCompletedEntries[i].ID == GetEntryByIndex(_index).ID)
            {
                return NotCompletedEntries[i];
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
                CompletedEntries.Add(Entries[i]);
                NotCompletedEntries.Remove(GetNotCompletedEntryByIndex(i));
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
    public string ID;
}

