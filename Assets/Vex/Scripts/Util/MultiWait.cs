using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiWait
{
    public Action OnComplete;
    private int count;
    private bool invoked;

    public MultiWait(int startingCount)
    {
        this.count = startingCount;
    }

    public void IncCount()
    {
        if (invoked)
        {
            Debug.LogWarning("MultiWait: Attempting to join mutliwait but it has already completed");
        }
        else
        {
            count++;
        }
    }

    public void CouldComplete()
    {
        count--;

        if(count <= 0 && (invoked == false))
        {
            invoked = true;
            OnComplete?.Invoke();
        }
    }
}
