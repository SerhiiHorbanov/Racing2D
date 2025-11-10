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

[RequireComponent(typeof(RacingRoundState))]
public class RoundStateManager : MonoBehaviour
{
	private RacingRoundState _racingState;
	
	[SerializeField] private GameObject _RacerCursorPrefab;

	private readonly Dictionary<Racer, RacerCursor> _cursors = new();
	
	public GameState _CurrentGameState = GameState.Racing;
	
	private void Awake()
	{
		if (RacersList.Instance == null)
		{
			Debug.LogWarning("RacersList is not initialized");
			return;
		}
		_racingState = GetComponent<RacingRoundState>();
		
		_racingState.OnStateEnd += ChangeStateFromRacingToPlacingItems;
		_racingState.InitializeRacingState();
	}

	private void Update()
	{
		
	}
	
	private void ChangeStateFromRacingToPlacingItems()
	{
		_racingState.UninitializeRacingState();
		_CurrentGameState = GameState.ItemsPlacing;
		InitializeItemsPlacingState();
	}

	private void ChangeStateFromPlacingItemsToRacing()
	{
		UninitializeItemsPlacingState();
		_CurrentGameState = GameState.Racing;
		_racingState.InitializeRacingState();
	}

	private void InitializeItemsPlacingState()
	{
		if (_cursors.Count == 0)
		{
			InitializeCursors();
		}

		foreach ((Racer racer, RacerCursor cursor) in _cursors)
		{
			racer.ConnectRacerCursorControllerTo(cursor);
			cursor._IsItemPlaced = false;
			cursor.gameObject.SetActive(true);
		}
	}
		
	private void InitializeCursors()
	{
		foreach (Racer racer in RacersList.Instance.Racers)
		{
			GameObject cursorGameObject = Instantiate(_RacerCursorPrefab);
			RacerCursor cursor = cursorGameObject.GetComponent<RacerCursor>();

			_cursors.Add(racer, cursor);
			cursor.OnItemPlaced += OnItemPlaced;
		}
	}

	private void UninitializeItemsPlacingState()
	{
		foreach ((Racer racer, RacerCursor cursor) in _cursors)
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
}