using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Checkpoint : MonoBehaviour
{
    [NonSerialized] public int Index;
    
    public Action<CheckpointProgressTracker, int> OnTrackerEntered;
    public Action<CheckpointProgressTracker, int> OnTrackerExited;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckpointProgressTracker tracker = other.GetComponent<CheckpointProgressTracker>();
        
        if (tracker is null)
            return;
        
        OnTrackerEntered?.Invoke(tracker, Index);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CheckpointProgressTracker tracker = other.GetComponent<CheckpointProgressTracker>();
        
        if (tracker is null)
            return;
        
        OnTrackerExited?.Invoke(tracker, Index);
    }
}
