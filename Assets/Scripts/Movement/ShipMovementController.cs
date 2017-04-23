using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : SphericalMovementController, IDamageableObject {

    public ParticleSystem _dashParticles;
    public Animator _shipAnimator;
    public CannonFiring _cannonController;

    public float _shipHealth = 100f;

    public Vector2 _movementSpeed = new Vector2(0,20f);

    public float _dashModifier = 2f;
    public float _dashTime = 1f;
    public float _dashCooldown = 5f;

    public float _turnSpeed = 2f;

    public Vector2 _currentVelocity;

    private float _originalRadius;
    private float _currentTurnAngle;
    private float _timeSinceLastDash;
    private bool _isDashing;
    private bool _isColliding;

    void Start()
    {
        base.Start();
        SetPosition(_positionOnPlane.x, _positionOnPlane.y);
        RotateBy(_currentTurnAngle);
        _timeSinceLastDash = -_dashCooldown;
        _originalRadius = _radius;
    }

	// Update is called once per frame
	/*void Update () {

        _axisInput = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        

        if(_axisInput.y > 0.5f)
        {
            StartDash();
        }

        ShipMovementStep();
        
    }*/

    protected void ShipMovementStep(float turnControl)
    {
        CalulateShipBuoyancy();
        RotateBy(GetCurrentTurnAngle(turnControl));
        _shipAnimator.SetFloat("Direction", Mathf.Clamp(_currentTurnAngle,-1,1));
        TranslateBy((GetCurrentVelocity() * Time.fixedDeltaTime).x, (GetCurrentVelocity() * Time.fixedDeltaTime).y);
        _rigidbody.velocity = GetCurrentVelocity();
        Movement();
    }

    void CalulateShipBuoyancy()
    {
        _radius = _originalRadius;//+((WaterController.instance.DistanceToWater(new Vector3(_positionOnPlane.x/2, 0, _positionOnPlane.y/2), Time.time))*20)+10f;
    }

    protected void StartDash()
    {
        if (!_isDashing)
        {
            if(Time.time - (_timeSinceLastDash) > _dashCooldown + _dashTime)
            {
                _isDashing = true;
                var main = _dashParticles.main;
                main.startLifetime = _dashTime; 
                _dashParticles.Play();
                _timeSinceLastDash = Time.time;
            }
        }
    }

    float GetCurrentTurnAngle(float input)
    {
        _currentTurnAngle = Mathf.Lerp(_currentTurnAngle, input * GetCurrentVelocity().y * Time.fixedDeltaTime * _turnSpeed, Time.deltaTime * 10);
        return _currentTurnAngle;
    }

    Vector2 GetCurrentVelocity()
    {
        if (!_isColliding)
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, CalculateShipVelocity(), Time.deltaTime * 20);
        }
        return _currentVelocity;
    }

    Vector2 CalculateShipVelocity()
    {
        Vector2 result = _movementSpeed;
        if (_isDashing)
        {
            if (Time.time - _timeSinceLastDash < _dashTime)
            {
                result *= _dashModifier;
            }else
            {
                _isDashing = false;
            }
        }
        return result;
    }

    public void OnHit(ProjectileController proj)
    {
        _shipHealth -= proj._damage;
        if(_shipHealth <= 0)
        {
            OnDeath();
        } 
    }

    public virtual void OnDeath()
    {

    }

    protected void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
        _currentVelocity -= new Vector2(collision.impulse.x, collision.impulse.z);
    }

    void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }
}
