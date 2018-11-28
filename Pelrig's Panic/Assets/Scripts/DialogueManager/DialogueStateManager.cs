using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public enum ManagerState
{
    Offline, Initializing, completed
}

public interface DialogueStateManager
{
    ManagerState currentState
    {
        get;
    }

    void BootSequence();
}