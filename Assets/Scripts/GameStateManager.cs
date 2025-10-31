using System.Collections.Generic;
using Lobby;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	[SerializeField] private GameObject _CarPrefab;
	[SerializeField] private Transform _CarSpawnPoint;
	[SerializeField] private RaceLoop _RaceLoop;
	
	private void Awake()
	{
		if (RacersList.Instance is null)
		{
			Debug.LogWarning("RacersList is not initialized");
			return;
		}

		InitializeRacingState();
	}

	private void InitializeRacingState()
	{
		InitializeCarsForEachRacer(RacersList.Instance.Racers);
	}

	private void InitializeCarsForEachRacer(IEnumerable<IRacer> racers)
	{
		foreach (IRacer racer in racers)
		{
			RaceCar car = SpawnAndInitializeNextCar();
			racer.ConnectCarControllerTo(car);
			racer.Car = car;

			_RaceLoop.AddCar(car);
		}
	}

	private RaceCar SpawnAndInitializeNextCar()
	{
		GameObject carGameObject = Instantiate(_CarPrefab);
		RaceCar car = carGameObject.GetComponent<RaceCar>();
		
		carGameObject.transform.position = _CarSpawnPoint.position;
		
		return car;
	}
}