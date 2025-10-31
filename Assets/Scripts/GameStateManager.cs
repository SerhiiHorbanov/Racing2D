using System.Collections.Generic;
using Lobby;
using Unity.VisualScripting;
using UnityEngine;

enum GameState
{
	Racing,
	ItemsChoosing,
	ItemsPlacing,
	PointsDisplaying,
	MatchEnding,
}

public class GameStateManager : MonoBehaviour
{
	[SerializeField] private GameObject _CarPrefab;
	[SerializeField] private Transform _CarSpawnPoint;
	[SerializeField] private RaceLoop _RaceLoop;

	private readonly Dictionary<RaceCar, IRacer> _carsToRacers = new();

	private const int LoopsToEndRacingState = 3;
	
	private void Awake()
	{
		if (RacersList.Instance is null)
		{
			Debug.LogWarning("RacersList is not initialized");
			return;
		}

		InitializeRacingState();
	}
	
	private void ChangeStateFromRacingToPlacingItems()
	{
		UninitializeRacingState();
		InitializeItemsPlacingState();
	}

	private void InitializeRacingState()
	{
		InitializeCars(RacersList.Instance.Racers);
	}

	private void UninitializeRacingState()
	{
		UninitializeCars(RacersList.Instance.Racers);
	}

	private void InitializeItemsPlacingState()
	{
		
	}
	
	private void UninitializeCars(IEnumerable<IRacer> racers)
	{
		foreach (IRacer racer in racers)
		{
			racer.DisconnectCarControllerFromCar();
		}
		foreach (RaceCar car in _carsToRacers.Keys)
		{
			Destroy(car.gameObject);
		}
		
		_carsToRacers.Clear();
	}

	private void InitializeCars(IEnumerable<IRacer> racers)
	{
		foreach (IRacer racer in racers)
		{
			RaceCar car = SpawnAndInitializeNextCarForRacer(racer);
			racer.ConnectCarControllerTo(car);
			racer.Car = car;

			_RaceLoop.AddCar(car);
			_carsToRacers.Add(car, racer);
		}

		_RaceLoop.CarFinishedALoop += CarFinishedALoop;
	}

	private void CarFinishedALoop(RaceCar car)
	{
		IRacer racer = _carsToRacers[car];
		RacerScore score = racer.GetScore();
		score._Loops++;
		print(score._Loops);

		if (score._Loops >= LoopsToEndRacingState)
		{
			ChangeStateFromRacingToPlacingItems();
		}
	}

	private RaceCar SpawnAndInitializeNextCarForRacer(IRacer racer)
	{
		GameObject carGameObject = Instantiate(_CarPrefab);
		RaceCar car = carGameObject.GetComponent<RaceCar>();
		
		carGameObject.transform.position = _CarSpawnPoint.position;
		
		return car;
	}
}