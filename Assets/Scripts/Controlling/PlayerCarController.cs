using UnityEngine;
using UnityEngine.InputSystem;

namespace Controlling
{
	public class PlayerCarController : RaceCarController
	{
		private float _currentAcceleration;
		private float _currentBrake;
		private float _currentSteering;
		
		private void Update()
		{
			if (_Car is null)
			{
				Debug.LogWarning("Car is null in a PlayerCarController");
				return;
			}
			
			_Car.SetAccelerationBrake(_currentAcceleration - _currentBrake);
			_Car.SetSteering(_currentSteering);
		}

		public void OnAcceleration(InputAction.CallbackContext context)
		{
			_currentAcceleration = context.ReadValue<float>();
		}
		
		public void OnBrake(InputAction.CallbackContext context)
		{
			_currentBrake = context.ReadValue<float>();
		}

		public void OnSteer(InputAction.CallbackContext context)
		{
			_currentSteering = context.ReadValue<float>();
		}
	}
}
