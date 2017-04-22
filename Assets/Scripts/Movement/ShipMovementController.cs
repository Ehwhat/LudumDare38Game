using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : SphericalMovementController {


    public float _minMovementSpeed = 20f;
    public float _maxMovementSpeed = 100f;
    public float _turnSpeed = 2f;
    public float _accelrationPerSecond = 10f;

    public int _requiredBoost = 3;

    public float[] _speedNotches = {
        50,
        20,
        -20
    };

    public int _currentSpeedNotch = 1;

    private float _lastSpeedChangeTime;
    public float _currentSpeed;

    private float _originalRadius;


    private Vector2 _axisInput;

    void Start()
    {
        base.Start();
        _lastSpeedChangeTime = Time.time;
        _originalRadius = _radius;
    }

	// Update is called once per frame
	void Update () {

        _axisInput = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.W))
        {
            IncreaseSpeed();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DecreaseSpeed();
        }
        //Debug.Log(GetCurrentSpeed());
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

    float GetCurrentSpeed()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, CalculateShipSpeed(), Time.deltaTime * _accelrationPerSecond);
        return _currentSpeed;
    }

    float CalculateShipSpeed()
    {
        return _speedNotches[_currentSpeedNotch];
    }

    void DecreaseSpeed()
    {
        if(_currentSpeedNotch < _speedNotches.Length-1)
        {
            _currentSpeedNotch++;
            _lastSpeedChangeTime = Time.time;
        }
    }

    void IncreaseSpeed()
    {
        if(_currentSpeedNotch > 0)
        {
            _currentSpeedNotch--;
            _lastSpeedChangeTime = Time.time;
        }
    }

}
