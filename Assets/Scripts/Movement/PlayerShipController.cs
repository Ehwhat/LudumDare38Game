using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : ShipMovementController {
    private Vector2 _axisInput;

    [SerializeField]
    private int _fuel = 0;
    [SerializeField]
    private int _fuelRequired = 3;

    // Update is called once per frame
    void Update () {
        
        _axisInput = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (_axisInput.y > 0.5f)
        {
            StartDash();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cannonController.Fire();
        }

        
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
