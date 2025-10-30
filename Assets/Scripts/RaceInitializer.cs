using Lobby;
using UnityEngine;

public class RaceInitializer : MonoBehaviour
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
		
		foreach (IRacer racer in RacersList.Instance.Racers)
		{
			RaceCar car = SpawnAndInitializeNextCar();
			racer.SpawnController(car);
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
