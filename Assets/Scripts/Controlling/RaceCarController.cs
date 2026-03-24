using System;
using UnityEngine;

namespace Controlling
{
	public class RaceCarController : MonoBehaviour
	{
		private RaceCar _car;

		[SerializeField]
		public RaceCar _Car
		{
			get => _car;
			set
			{
				RaceCar prevCar = _car;
				_car = value;
				OnCarSet(prevCar);
			}
		}

		public virtual void OnCarSet(RaceCar prevCar)
		{ }
	}
}
