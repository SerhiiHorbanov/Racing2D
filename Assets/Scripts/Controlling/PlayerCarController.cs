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

		public void OnMove(InputAction.CallbackContext context)
		{
			Vector2 value = context.ReadValue<Vector2>();
			float l1 = value.magnitude;

			if (value.x == 0 || value.y == 0)
			{
				ApplyMoveVector(value);
				return;
			}

			if (l1 > 1)
			{
				value /= l1;
				l1 = 1;
			}
			
			float x2 = value.x / value.y;
			float y2 = value.y / value.x;

			float x3 = Mathf.Clamp(x2, -1, 1);
			float y3 = Mathf.Clamp(y2, -1, 1);
			
			float l3 = Mathf.Sqrt(x3*x3 + y3*y3);

			float x4 = value.x * l3;
			float y4 = value.y * l3;
			Vector2 processedMove = new Vector2(x4, y4);
			
			ApplyMoveVector(processedMove);
			print(processedMove);
		}

		public void ApplyMoveVector(Vector2 value)
		{
			_currentSteering = value.x;
			
			if (value.y > 0)
			{
				_currentAcceleration = value.y;
				_currentBrake = 0;
			}
			else
			{
				_currentAcceleration = 0;
				_currentBrake = -value.y;
			}
		}
	}
}
