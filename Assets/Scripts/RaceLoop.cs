using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class RaceLoop : MonoBehaviour
{
	[SerializeField] private List<Checkpoint> _Checkpoints;
	
	private void Awake()
	{
		if (_Checkpoints.Count == 0)
		{
			Debug.LogWarning("No checkpoints assigned");
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

	private void OnTrackerReachedCheckpoint(CheckpointProgressTracker tracker, int reachedCheckpoint)
	{
		int prevCheckpoint = tracker._CurrentCheckpoint;
		bool shouldMoveToNewCheckpoint = Math.Abs(prevCheckpoint - reachedCheckpoint) == 1;
		
		if (shouldMoveToNewCheckpoint)
		{
			tracker._CurrentCheckpoint = reachedCheckpoint;
			return;
		}
		
		bool finishedALoop = prevCheckpoint == _Checkpoints.Count - 1 & reachedCheckpoint == 0;
		if (finishedALoop)
		{
			tracker._CurrentCheckpoint = 0;
			tracker.InvokeOnFinishedALoop();
		}
	}
}