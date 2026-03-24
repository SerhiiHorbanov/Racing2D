using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class RaceLoop : MonoBehaviour
{
	[SerializeField] public List<Checkpoint> _Checkpoints;
	public static RaceLoop Instance;
	
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("RaceLoop is already initialized");
			Destroy(this);
			return;
		}
		
		Instance = this;
		
		if (_Checkpoints.Count < 2)
		{
			Debug.LogWarning("Not enough checkpoints assigned in RaceLoop");
			return;
		}

		for (int i = 0; i < _Checkpoints.Count; i++)
		{
			Checkpoint checkpoint = _Checkpoints[i];
			checkpoint.Index = i;
			checkpoint.OnTrackerEntered += OnTrackerReachedCheckpoint;
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _Checkpoints.Count; i++)
		{
			Checkpoint checkpoint = _Checkpoints[i];
			checkpoint.OnTrackerEntered -= OnTrackerReachedCheckpoint;
		}
	}

	private void OnTrackerReachedCheckpoint(CheckpointProgressTracker tracker, int reachedCheckpointIdx)
	{
		int prevCheckpoint = tracker._CurrentCheckpoint;
		bool isReachedCheckpointAdjacentToPrevious = Math.Abs(prevCheckpoint - reachedCheckpointIdx) == 1;
		Checkpoint reachedCheckpoint = _Checkpoints[reachedCheckpointIdx];
		
		if (isReachedCheckpointAdjacentToPrevious)
		{
			tracker.SetCheckpoint(reachedCheckpoint);
			return;
		}
		
		bool finishedALoop = prevCheckpoint == _Checkpoints.Count - 1 & reachedCheckpointIdx == 0;
		if (finishedALoop)
		{
			tracker.SetCheckpoint(reachedCheckpoint);
			tracker.InvokeOnFinishedALoop();
		}
	}
	
	public int NextCheckpointIndex(int currentCheckpoint) 
		=> (currentCheckpoint + 1) % _Checkpoints.Count;
}