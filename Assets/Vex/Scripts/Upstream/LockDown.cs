using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows other classes to enable / disable a component, without overlapping
/// </summary>
[Serializable]
public class LockDown
{
    [SerializeField]
    public List<string> lockingComponentIDs = new List<string>();

    public bool IsUnlocked
    {
        get
        {
            return lockingComponentIDs.Count <= 0;
        }
    }

    public void Lock(string id)
    {
        if (lockingComponentIDs.Contains(id) == false)
        {
            lockingComponentIDs.Add(id);
        }
    }

    public void Unlock(string id)
    {
        if (lockingComponentIDs.Contains(id))
        {
            lockingComponentIDs.Remove(id);
        }
    }
}

