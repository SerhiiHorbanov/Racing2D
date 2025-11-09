using System;
using UnityEngine;

public class CheckpointProgressTracker : MonoBehaviour
{
    public int _CurrentCheckpoint = 0;
    public Action<CheckpointProgressTracker> OnFinishedALoop;
    
    public void InvokeOnFinishedALoop()
    {
        OnFinishedALoop?.Invoke(this);
    }
}
