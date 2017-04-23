using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalMovementController : MonoBehaviour {

    public PlanetController _currentPlanet;
    public float _heightOffset = 0.5f;

    public Rigidbody _rigidbody;
    public bool useRigidbody = true;
    public bool _isActive = true;

    public Vector2 _direction;

    public float _radius;

    public Vector2 _positionOnPlane;
    public float _rotationalAngle;

    public Vector3 _offsetRotation = Vector3.zero;
    public Quaternion _rotationQuat = Quaternion.identity;

    public void Awake()
    {
        if(_rigidbody == null)
        {
            useRigidbody = false;
        }
        //SetPlanet(_currentPlanet);
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
        
        _direction = new Vector2(Mathf.Sin(_rotationalAngle), Mathf.Cos(_rotationalAngle));
        Vector2 perpendicular = new Vector2(-_direction.y, _direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y / _currentPlanet._radiusModifier, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x / _currentPlanet._radiusModifier, _direction);
        _rotationQuat *= hRot * vRot;
        _positionOnPlane = new Vector2(x,y);//GetPlanePosition();
    }

    public void SetPosition(float x, float y)
    {
       
        _direction = new Vector2(Mathf.Sin(_rotationalAngle), Mathf.Cos(_rotationalAngle));
        Vector2 perpendicular = new Vector2(-_direction.y, _direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x, _direction);
        _rotationQuat = Quaternion.identity;
        _rotationQuat *= hRot * vRot;
        _positionOnPlane = new Vector2(x, y);
    }

    public Vector2 GetPlanePosition(){
        Vector3 direction = _direction;
        Vector3 perpendicular = new Vector3(-_direction.y, _direction.x);
        float x, y = 0;
        _rotationQuat.ToAngleAxis(out x, out direction);
        _rotationQuat.ToAngleAxis(out y, out perpendicular);

        Debug.Log("X: " + x + " Y:" + y);

        return new Vector2(x, y);
    }

    public static Vector3 FindPositionRealativeToAngle(float x, float y, SphericalMovementController sphereController)
    {
        Vector2 direction = new Vector2(Mathf.Sin(sphereController._rotationalAngle), Mathf.Cos(sphereController._rotationalAngle));
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y / sphereController._currentPlanet._radiusModifier, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x / sphereController._currentPlanet._radiusModifier, direction);
        Quaternion rotationQuat = sphereController._rotationQuat;
        rotationQuat *= hRot * vRot;
        return rotationQuat * Vector3.forward * (sphereController._radius + sphereController._heightOffset);
    }

    public void Movement()
    {
        _radius = _currentPlanet.GetRadius();
        if (useRigidbody)
        {
            _rigidbody.MovePosition(_rotationQuat * Vector3.forward * (_radius + _heightOffset));
            _rigidbody.MoveRotation(_rotationQuat * Quaternion.LookRotation(_direction, Vector3.forward) * Quaternion.Euler(_offsetRotation)) ;
        }else
        {
            transform.position = _rotationQuat * Vector3.forward * (_radius + _heightOffset);
            transform.rotation = _rotationQuat * Quaternion.LookRotation(_direction, Vector3.forward) * Quaternion.Euler(_offsetRotation);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(FindPositionRealativeToAngle(_positionOnPlane.x, _positionOnPlane.y, this), 1);
    }

    

}
