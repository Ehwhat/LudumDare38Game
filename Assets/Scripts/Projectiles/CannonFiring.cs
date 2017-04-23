using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFiring : MonoBehaviour {

    [System.Serializable]
    public struct CannonPoint
    {
        public GameObject point;
        public Vector3 fireDirection;
        public ParticleSystem cannonParticles;
        public LineRenderer previewLine;
    }

    public ShipMovementController _currentShip;
    public GameObject _cannonProjectile;
    public CannonPoint[] _cannonFiringPoints;
    public float _cannonFiringDelay;
    public float _cannonSpeed;

    private float _lastFire;

    void Start()
    {
        _lastFire = -_cannonFiringDelay;
    }

    void Update()
    {
        GeneratePreviewLines();
    }

    public void Fire()
    {
        if(Time.time - _lastFire > _cannonFiringDelay)
        {
            FireCannons();
            _lastFire = Time.time;
        }
    }

    void FireCannons()
    {
        foreach(CannonPoint point in _cannonFiringPoints)
        {
            FireCannon(point);
        }
    }

    void FireCannon(CannonPoint point)
    {
        ProjectileController _projectile = (ProjectileController)GameObject.Instantiate(_cannonProjectile).GetComponent<ProjectileController>();
        _projectile.Initialise(point.fireDirection + (Vector3)_currentShip._movementSpeed*Time.deltaTime, _currentShip._currentPlanet);
        _projectile._positionOnPlane = _currentShip._positionOnPlane;// + new Vector2(point.point.transform.localPosition.x, point.point.transform.localPosition.z);
        _projectile._direction = _currentShip._direction;
        _projectile._rotationalAngle = _currentShip._rotationalAngle;
        _projectile._rotationQuat = _currentShip._rotationQuat;
        _projectile.TranslateBy(point.point.transform.localPosition.x, point.point.transform.localPosition.z);
        _projectile._heightOffset = _projectile._currentPlanet.GetNormalToPlanet(point.point.transform).magnitude - _projectile._currentPlanet.GetRadius();

        point.cannonParticles.Play();

    }

    void GeneratePreviewLines()
    {
        foreach (CannonPoint point in _cannonFiringPoints)
        {
            GeneratePreviewLine(point);
        }
    }
    void GeneratePreviewLine(CannonPoint point, int length = 20)
    {
        point.previewLine.positionCount = length;
        Vector2[] pointsOnPlane = new Vector2[length];
        pointsOnPlane[0] = _currentShip._positionOnPlane + new Vector2(-point.point.transform.localPosition.x, point.point.transform.localPosition.z);// - _currentShip._movementSpeed*Time.deltaTime;
        Vector3[] points = new Vector3[length];
        points[0] = point.point.transform.position;
        for(int i = 1; i < length; i++)
        {
            pointsOnPlane[i] = new Vector2(pointsOnPlane[i - 1].x, pointsOnPlane[i - 1].y) - (Vector2)point.fireDirection; //- _currentShip._movementSpeed*Time.deltaTime;
            points[i] = SphericalMovementController.FindPositionRealativeToAngle(pointsOnPlane[i].x, pointsOnPlane[i].y, _currentShip) + (_currentShip.transform.up * (_currentShip._currentPlanet.GetNormalToPlanet(point.point.transform).magnitude - _currentShip._currentPlanet.GetRadius() - _currentShip._heightOffset)); 
        }
        point.previewLine.SetPositions(points);
    }
	
}
