using UnityEngine;
using UnityEngine.AI;

namespace Controlling
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class AICarController : RaceCarController
	{
		private NavMeshAgent _agent;
		private CheckpointProgressTracker _tracker;
		private bool _hasTarget;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_agent.updatePosition = false;
			_agent.updateRotation = false;
			_agent.updateUpAxis = false;
		}
		
		public override void OnCarSet(RaceCar prevCar)
		{
			if (_tracker != null)
				_tracker.OnCheckpointUpdated -= OnCheckpointUpdated;
			
			_tracker = _Car?.GetComponent<CheckpointProgressTracker>();
			
			if (_tracker == null)
				return;
			
			_tracker.OnCheckpointUpdated += OnCheckpointUpdated;
			_hasTarget = false;
			UpdateTarget();
		}

		private void OnCheckpointUpdated(Checkpoint reachedCheckpoint)
		{
			UpdateTarget();
		}

		private void UpdateTarget()
		{
			if (_tracker == null || RaceLoop.Instance == null)
				return;
			
			int nextIndex = RaceLoop.Instance.NextCheckpointIndex(_tracker._CurrentCheckpoint);
			Checkpoint nextCheckpoint = RaceLoop.Instance._Checkpoints[nextIndex];
			
			Vector3 targetPos = nextCheckpoint.transform.position;
			targetPos.z = 0;
			_agent.SetDestination(targetPos);
			_hasTarget = true;
		}

		private void Update()
		{
			if (_Car is null)
				return;
			
			if (!_hasTarget && _tracker != null)
				UpdateTarget();

			_agent.nextPosition = _Car.transform.position;
			
			Vector2 desiredVelocity = _agent.desiredVelocity;
			//print($"desired velocity: {desiredVelocity}");
			if (desiredVelocity.sqrMagnitude > 0.01f)
			{
				Vector2 forward = _Car.transform.up;
				//print($"forward: {_Car.transform.forward}");
				//print($"right: {_Car.transform.right}");
				//print($"up: {_Car.transform.up}");
				float angle = -Vector2.SignedAngle(forward, desiredVelocity);
				
				_Car.SetSteering(Mathf.Clamp(angle / 45f, -1f, 1f));
				_Car.SetAccelerationBrake(Mathf.Clamp01(Vector2.Dot(forward, desiredVelocity.normalized)));
				//print($"angle: {Mathf.Clamp(angle / 45f, -1f, 1f)}");
				//print($"accel: {Mathf.Clamp01(Vector2.Dot(forward, desiredVelocity.normalized))}");
				
			}
			else
			{
				_Car.SetSteering(0);
				_Car.SetAccelerationBrake(0);
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (_agent == null || _agent.path == null || _agent.path.corners.Length < 2)
				return;
			
			Gizmos.color = Color.cyan;
			for (int i = 0; i < _agent.path.corners.Length - 1; i++)
			{
				Gizmos.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1]);
			}
		}
	}
}
