using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : SphericalMovementController {


    public float _movementSpeed = 20f;

    public float _dashModifier = 2f;
    public float _dashTime = 1f;
    public float _dashCooldown = 5f;

    public float _turnSpeed = 2f;
    public int _requiredBoost = 3;

    public float _currentSpeed;

    private float _originalRadius;
    private float _timeSinceLastDash;
    private bool _isDashing;

    private Vector2 _axisInput;

    void Start()
    {
        base.Start();
        _timeSinceLastDash = -_dashCooldown;
        _originalRadius = _radius;
    }

	// Update is called once per frame
	void Update () {

        _axisInput = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if(_axisInput.y > 0.5f)
        {
            StartDash();
        }

        ShipMovementStep();
        
    }

    void ShipMovementStep()
    {
        CalulateShipBuoyancy();
        RotateBy(_axisInput.x * GetCurrentSpeed() * Time.deltaTime * _turnSpeed);
        TranslateBy(0, GetCurrentSpeed() * Time.deltaTime);
        Movement();
    }

    void CalulateShipBuoyancy()
    {
        _radius = _originalRadius;//+((WaterController.instance.DistanceToWater(new Vector3(_positionOnPlane.x/2, 0, _positionOnPlane.y/2), Time.time))*20)+10f;
    }

    void StartDash()
    {
        if (!_isDashing)
        {
            if(Time.time - (_timeSinceLastDash) > _dashCooldown + _dashTime)
            {
                _isDashing = true;
                _timeSinceLastDash = Time.time;
            }
        }
    }

    float GetCurrentSpeed()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, CalculateShipSpeed(), Time.deltaTime * 20);
        return _currentSpeed;
    }

    float CalculateShipSpeed()
    {
        float result = _movementSpeed;
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

}
