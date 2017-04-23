using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : ShipMovementController {
    private Vector2 _axisInput;

    public CameraController _cameraController;
    public ParticleSystem _jumpParticle;

    [SerializeField]
    private int _fuel = 0;
    [SerializeField]
    private int _fuelRequired = 3;
    private bool _isJumping = true;
    private bool _isStart = true;


    [SerializeField]
    private float _jumpTotalHeight = 80f;
    private float _originalHeightOffset;
    private float _targetHeight;


    void Start()
    {
        _originalHeightOffset = _heightOffset;
        _targetHeight = _heightOffset;
        _heightOffset = _jumpTotalHeight;
        _isActive = false;
    }

    // Update is called once per frame
    void Update () {
        
        _axisInput = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_isActive)
        {
            if (_axisInput.y > 0.5f)
            {
                StartDash();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cannonController.Fire();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_fuel >= _fuelRequired)
                {
                    StartJump();
                }
            }
        }

        if (_isJumping)
        {
            _heightOffset = Mathf.Lerp(_heightOffset, _targetHeight, Time.deltaTime);
            if(_heightOffset > _jumpTotalHeight / 2)
            {
                if (!_isStart)
                {
                    _cameraController._isActive = false;
                }
            }else if(_heightOffset <= _jumpTotalHeight / 2 && _heightOffset != _originalHeightOffset)
            {
                _cameraController._isActive = true;
                _cameraController._enableControls = true;
                if (_isStart)
                {
                    _isStart = false;
                    _isActive = true;
                }
            }else
            {
                _isJumping = false;
            }
        }
        
    }

    void StartJump()
    {
        _jumpParticle.Play(true);
        _isJumping = true;
        _isActive = false;
        _cameraController._enableControls = false;
        _targetHeight = _jumpTotalHeight;
    }

    public void AddFuel()
    {
        _fuel++;
    }

    void FixedUpdate()
    {
       ShipMovementStep(_axisInput.x);
    }
}
