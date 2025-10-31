using System;
using System.Collections.Generic;
using UnityEngine;

public class RaceLoop : MonoBehaviour
{
	[SerializeField] private List<Checkpoint> _Checkpoints;

	private Dictionary<RaceCar, int> _reachedCheckpointForCar = new();
	
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
			checkpoint.OnCarEntered += OnCarEnteredCheckpoint;
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _Checkpoints.Count; i++)
		{
			Checkpoint checkpoint = _Checkpoints[i];
			checkpoint.OnCarEntered -= OnCarEnteredCheckpoint;
		}
	}

	private void OnCarEnteredCheckpoint(RaceCar car, int newCheckpoint)
	{
		if (!_reachedCheckpointForCar.TryGetValue(car, out int prevCheckpoint))
			return;

		bool shouldMoveToNewCheckpoint = Math.Abs(prevCheckpoint - newCheckpoint) == 1;
		if (shouldMoveToNewCheckpoint)
		{
			_reachedCheckpointForCar[car] = newCheckpoint;
			return;
		}
		
		bool finishedALoop = prevCheckpoint == _Checkpoints.Count - 1 & newCheckpoint == 0;
		if (finishedALoop)
		{
			_reachedCheckpointForCar[car] = 0;
			CarFinishedALoop(car);
		}
	}

	private void CarFinishedALoop(RaceCar car)
	{
		print($"car {car.gameObject.name} finished a loop");
	}

	public void AddCar(RaceCar car)
	{
		_reachedCheckpointForCar.Add(car, 0);
	}
}