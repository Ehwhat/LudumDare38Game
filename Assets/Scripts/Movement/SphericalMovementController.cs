using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovementController : MonoBehaviour {

    public PlanetController _currentPlanet;
    public float _heightOffset = 0.5f;

    public Rigidbody _rigidbody;
    public bool useRigidbody = true;

    public Vector2 _direction;

    public float _radius;

    public Vector2 _positionOnPlane;
    public float _rotationalAngle;
    public Quaternion _rotationQuat = Quaternion.identity;

    public void Start()
    {
        if(_rigidbody == null)
        {
            useRigidbody = false;
        }
        SetPlanet(_currentPlanet);
    }

    public void SetPlanet(PlanetController planet)
    {
        _currentPlanet = planet;
        _radius = _currentPlanet.GetRadius();
    }

    public void RotateBy(float amount)
    {
        _rotationalAngle += amount * Mathf.Deg2Rad;
    }

    public void TranslateBy(float x, float y)
    {
        _positionOnPlane += new Vector2(x, y);
        _direction = new Vector2(Mathf.Sin(_rotationalAngle), Mathf.Cos(_rotationalAngle));
        Vector2 perpendicular = new Vector2(-_direction.y, _direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y / _currentPlanet._radiusModifier, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x / _currentPlanet._radiusModifier, _direction);
        _rotationQuat *= hRot * vRot;
    }

    public void SetPosition(float x, float y)
    {
        _positionOnPlane = new Vector2(x, y);
        _direction = new Vector2(Mathf.Sin(_rotationalAngle), Mathf.Cos(_rotationalAngle));
        Vector2 perpendicular = new Vector2(-_direction.y, _direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x, _direction);
        _rotationQuat = hRot * vRot;
    }



    public void Movement()
    {
        _radius = _currentPlanet.GetRadius();
        if (useRigidbody)
        {
            _rigidbody.MovePosition(_rotationQuat * Vector3.forward * (_radius + _heightOffset));
            _rigidbody.MoveRotation(_rotationQuat * Quaternion.LookRotation(_direction, Vector3.forward));
        }else
        {
            transform.position = _rotationQuat * Vector3.forward * (_radius + _heightOffset);
            transform.rotation = _rotationQuat * Quaternion.LookRotation(_direction, Vector3.forward);
        }
    }

    

}
