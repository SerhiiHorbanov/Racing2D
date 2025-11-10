using System.Collections.Generic;
using Lobby;
using UnityEngine;

public enum GameState
{
	Racing,
	ItemsChoosing,
	ItemsPlacing,
	PointsDisplaying,
	MatchEnding,
}

public class RoundStateManager : MonoBehaviour
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
		if (RacersList.Instance == null)
		{
			Debug.LogWarning("RacersList is not initialized");
			return;
		}
		
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
		UninitializeCars();
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
			ChangeStateFromRacingToPlacingItems();
		}
	}
}