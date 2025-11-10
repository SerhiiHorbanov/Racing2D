using System;
using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class RacingRoundState : MonoBehaviour
{
	[SerializeField] private GameObject _CarPrefab;
	
	[SerializeField] private int _LoopsToEndRacingState = 3;
	
	[SerializeField] private Transform _CarSpawnPoint;
	[SerializeField] private RaceLoop _RaceLoop;
	
	private readonly Dictionary<RaceCar, IRacer> _carsToRacers = new();

	public Action OnStateEnd;
	
	public void InitializeRacingState()
	{
		InitializeCars(RacersList.Instance.Racers);
	}

	public void UninitializeRacingState()
	{
		UninitializeCars();
	}

	private void InitializeCars(IEnumerable<IRacer> racers)
	{
		foreach (IRacer racer in racers)
		{
			GameObject carGameObject = Instantiate(_CarPrefab);
			RaceCar car = carGameObject.GetComponent<RaceCar>();
		
			carGameObject.transform.position = _CarSpawnPoint.position;
					
			racer.Car = car;
			racer.EnableCarController(car);
			racer.ConnectScoreToCar();

			CheckpointProgressTracker tracker = carGameObject.GetComponent<CheckpointProgressTracker>();
			tracker.OnFinishedALoop += CarFinishedALoop;
			
			_carsToRacers.Add(car, racer);
		}
	}
	
	private void UninitializeCars()
	{
		foreach ((RaceCar car, IRacer racer) in _carsToRacers)
		{
			racer.DisableCarController();
			racer.DisconnectScoreFromCar();
			Destroy(car.gameObject);
		}
		
		_carsToRacers.Clear();
	}

	private void CarFinishedALoop(CheckpointProgressTracker tracker)
	{
		RaceCar car = tracker.GetComponent<RaceCar>();
		
		if (car is null)
		{
			Debug.LogWarning("CarFinishedALoop called on a non-car object");
			return;
		}
		
		if (!_carsToRacers.TryGetValue(car, out IRacer racer))
			return;

		RacerScore score = racer.GetScore();
		score.AddLoop();

		if (tracker._LoopsDone >= _LoopsToEndRacingState)
		{
			score.AddWin();
			OnStateEnd?.Invoke();
		}
	}
}