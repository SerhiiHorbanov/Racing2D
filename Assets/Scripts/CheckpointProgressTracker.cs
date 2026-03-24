using System;
using UnityEngine;

public class CheckpointProgressTracker : MonoBehaviour
{
    public int _CurrentCheckpoint = 0;
    public int _LoopsDone;
    public Action<CheckpointProgressTracker> OnFinishedALoop;
    
    public Action<Checkpoint> OnCheckpointUpdated;
    
    public void InvokeOnFinishedALoop()
    {
        _LoopsDone++;
        OnFinishedALoop?.Invoke(this);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        _CurrentCheckpoint = checkpoint.Index;
        OnCheckpointUpdated?.Invoke(checkpoint);
    }
}
