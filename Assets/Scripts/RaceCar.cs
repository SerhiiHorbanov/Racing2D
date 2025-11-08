using System;
using Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RaceCar : MonoBehaviour
{
    [SerializeField] private DrivingForces _DefaultDrivingForces;
    [SerializeField] private DrivingForces _DriftingDrivingForces;
    
    private const float SteeringThresholdForFriction = 0.01f;

    private bool _isDrifting;
    private float _currentAcceleration;
    private float _currentBrake;
    private float _currentSteering;
    
    private Rigidbody2D _rigidbody;

    public Action<float> OnDriftedDistance;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void SetDrift(bool value)
    {
        _isDrifting = value;
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
        
        DrivingForces currentDrivingForces = _isDrifting ? _DriftingDrivingForces : _DefaultDrivingForces;
        
        ApplyMovement(currentDrivingForces);
        
        if (_isDrifting)
            OnDriftedDistance?.Invoke(_rigidbody.linearVelocity.magnitude * deltaTime);
    }

    private void ApplyFriction(DrivingForces forces)
    {
        Vector2 forward = transform.up;
        Vector2 right = transform.right;
        Vector2 sideVelocity = right * Vector2.Dot(_rigidbody.linearVelocity, right);
        
        Vector2 forwardVelocity = forward * Vector2.Dot(_rigidbody.linearVelocity, forward);
        
        _rigidbody.AddForce(-sideVelocity * forces._OrthogonalFriction, ForceMode2D.Force);
        _rigidbody.AddForce(-forwardVelocity * forces._ParallelFriction, ForceMode2D.Force);
        
        if (Mathf.Abs(_currentSteering) > SteeringThresholdForFriction)
            return;

        float angularFriction = Mathf.Sign(_rigidbody.angularVelocity) * -forces._AngularFriction * Mathf.Deg2Rad;
        _rigidbody.AddTorque(angularFriction, ForceMode2D.Force);
    }
    
    private void ApplyMovement(DrivingForces forces)
    {
        Vector2 dir = transform.up;
        Vector2 braking = -dir * (_currentBrake * forces._BrakeForce);
        Vector2 acceleration = dir * (_currentAcceleration * forces._AccelerationForce);

        Vector2 right = transform.right;
        float forwardSpeed = Vector2.Dot(_rigidbody.linearVelocity, dir);
        float orthogonalSpeed = Mathf.Abs(Vector2.Dot(_rigidbody.linearVelocity, right));

        float velocityBasedTurningMultiplier = Mathf.Sqrt(forwardSpeed * forwardSpeed + (orthogonalSpeed * orthogonalSpeed) * forces._OrthogonalSpeedSteeringInfluence) * Mathf.Sign(forwardSpeed);
        
        float steering = -(_currentSteering) * forces._SteeringTorque * Mathf.Deg2Rad * (velocityBasedTurningMultiplier / forces._SpeedForSteering);

        ApplyFriction(forces);
        
        _rigidbody.AddForce(acceleration, ForceMode2D.Force);
        _rigidbody.AddForce(braking, ForceMode2D.Force);
        
        _rigidbody.AddTorque(steering, ForceMode2D.Force);
    }
}
