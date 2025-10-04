using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName = "DrivingForces", menuName = "Racing2D/DrivingForces")]
	public class DrivingForces : ScriptableObject
	{
		[SerializeField] public float _AccelerationForce;
		[SerializeField] public float _BrakeForce;
    
		[SerializeField] public float _SteeringTorque;
		[SerializeField] public float _SpeedForSteering;
    
		[Space]
		[SerializeField] public float _OrthogonalFriction;
		[SerializeField] public float _ParallelFriction;
		[SerializeField] public float _AngularFriction;
	}
}
