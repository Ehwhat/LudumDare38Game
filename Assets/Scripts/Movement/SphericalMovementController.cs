using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovementController : MonoBehaviour {

    public PlanetController _currentPlanet;
    public float _heightOffset = 0.5f;

    public Vector2 _direction;

    public float _radius;

    protected Vector2 _positionOnPlane;
    private float _rotationalAngle;
    private Quaternion _rotationQuat = Quaternion.identity;

    public void Start()
    {
        _radius = _currentPlanet._radius;
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
        Quaternion vRot = Quaternion.AngleAxis(y, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x, _direction);
        _rotationQuat *= hRot * vRot;
    }

    public void Movement()
    {
        transform.position =  _rotationQuat * Vector3.forward * _radius;
        transform.rotation =  _rotationQuat * Quaternion.LookRotation(_direction, Vector3.forward);
    }

}
