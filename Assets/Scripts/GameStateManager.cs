using System.Collections.Generic;
using Lobby;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
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
	[SerializeField] private GameObject _RacerCursorPrefab;
	[SerializeField] private Transform _CarSpawnPoint;
	[SerializeField] private RaceLoop _RaceLoop;

	private readonly Dictionary<RaceCar, IRacer> _carsToRacers = new();
	private readonly Dictionary<IRacer, RacerCursor> _cursors = new();
	
	[SerializeField] private int _LoopsToEndRacingState = 3;

	public GameState _CurrentGameState = GameState.Racing;
	
	private void Awake()
	{
		if (RacersList.Instance is null)
		{
			Debug.LogWarning("RacersList is not initialized");
			return;
		}

		_RaceLoop.CarFinishedALoop += CarFinishedALoop;
		InitializeRacingState();
	}

	private void Update()
	{
		
	}
	
	private void ChangeStateFromRacingToPlacingItems()
	{
		UninitializeRacingState();
		_CurrentGameState = GameState.ItemsPlacing;
		InitializeItemsPlacingState();
	}

	private void ChangeStateFromPlacingItemsToRacing()
	{
		UninitializeItemsPlacingState();
		_CurrentGameState = GameState.Racing;
		InitializeRacingState();
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
		if (_cursors.Count == 0)
		{
			InitializeCursors();
		}

		foreach ((IRacer racer, RacerCursor cursor) in _cursors)
		{
			racer.ConnectRacerCursorControllerTo(cursor);
			cursor._IsItemPlaced = false;
			cursor.gameObject.SetActive(true);
		}
	}
		
	private void InitializeCursors()
	{
		foreach (IRacer racer in RacersList.Instance.Racers)
		{
			GameObject cursorGameObject = Instantiate(_RacerCursorPrefab);
			RacerCursor cursor = cursorGameObject.GetComponent<RacerCursor>();

			_cursors.Add(racer, cursor);
			cursor.OnItemPlaced += OnItemPlaced;
		}
	}

	private void UninitializeItemsPlacingState()
	{
		foreach ((IRacer racer, RacerCursor cursor) in _cursors)
		{
			racer.DisconnectRacerCursorControllerFromCursor();
			cursor.gameObject.SetActive(false);
		}
	}

	private void OnItemPlaced()
	{
		foreach (RacerCursor cursor in _cursors.Values)
		{
			if (!cursor._IsItemPlaced)
				return;
		}
		
		ChangeStateFromPlacingItemsToRacing();
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
	}

	private void CarFinishedALoop(RaceCar car)
	{
		if (!_carsToRacers.TryGetValue(car, out IRacer carsToRacer))
			return;
		
		IRacer racer = carsToRacer;
		RacerScore score = racer.GetScore();
		score._Loops++;
		print(score._Loops);

		if (score._Loops >= _LoopsToEndRacingState)
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