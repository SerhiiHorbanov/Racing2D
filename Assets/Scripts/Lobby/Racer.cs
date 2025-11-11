using System;
using Data;
using UnityEngine;

namespace Lobby
{
	public abstract class Racer : MonoBehaviour
	{
		[NonSerialized] public RaceCar Car;
		
		public RacerConfiguration Configuration = new();
		public readonly RacerScore Score = new();

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void ConnectScoreToCar()
		{
			Car.OnDriftedDistance += Score.AddDrifting;
		}
		
		public void DisconnectScoreFromCar()
		{
			Car.OnDriftedDistance -= Score.AddDrifting;
		}

		public abstract void EnableCarController(RaceCar car);

		public abstract void DisableCarController();

		public abstract void ConnectRacerCursorControllerTo(RacerCursor cursor);

		public abstract void DisconnectRacerCursorControllerFromCursor();
	}
}