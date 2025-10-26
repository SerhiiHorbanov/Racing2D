using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RaceCar : MonoBehaviour
{
    [SerializeField] private DrivingForces _DefaultDrivingForces;
    [SerializeField] private DrivingForces _DriftingDrivingForces;
    
    private const float SteeringThresholdForFriction = 0.01f;

    private float _currentAcceleration;
    private float _currentBrake;
    private float _currentSteering;
    
    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void SetAccelerationBrake(float value)
    {
        bool positive = value > 0;
        
        _currentAcceleration = positive ? value : 0;
        _currentBrake        = positive ? 0 : -value;
    }

    public void SetSteering(float value)
        => _currentSteering = value;

    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        
        ApplyMovement(_DefaultDrivingForces);
    }

    private void ApplyFriction(DrivingForces forces)
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;
        Vector2 sideVelocity = right * Vector2.Dot(_rigidbody.linearVelocity, right);
        
        Vector2 forwardVelocity = forward * Vector2.Dot(_rigidbody.linearVelocity, forward);
        
        _rigidbody.AddForce(-sideVelocity * forces._OrthogonalFriction, ForceMode2D.Force);
        _rigidbody.AddForce(-forwardVelocity * forces._ParallelFriction, ForceMode2D.Force);
        
        if (_currentSteering > SteeringThresholdForFriction)
            return;
        
        _rigidbody.AddTorque(Mathf.Sign(_rigidbody.angularVelocity) * -forces._AngularFriction * Mathf.Deg2Rad, ForceMode2D.Force);
        
    }
    
    private void ApplyMovement(DrivingForces forces)
    {
        Vector2 dir = transform.up;
        Vector2 braking = -dir * (_currentBrake * forces._BrakeForce);
        Vector2 acceleration = dir * (_currentAcceleration * forces._AccelerationForce);

        float speedForward = Vector2.Dot(_rigidbody.linearVelocity, dir);
        
        float steering = -(_currentSteering) * forces._SteeringTorque * Mathf.Deg2Rad * (speedForward / forces._SpeedForSteering);

        ApplyFriction(forces);
        
        _rigidbody.AddForce(acceleration, ForceMode2D.Force);
        _rigidbody.AddForce(braking, ForceMode2D.Force);
        
        _rigidbody.AddTorque(steering, ForceMode2D.Force);
    }
}
