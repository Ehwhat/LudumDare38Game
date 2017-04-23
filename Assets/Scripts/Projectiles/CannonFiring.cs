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
        _projectile.Initialise(point.fireDirection, _currentShip._currentPlanet);
        _projectile._positionOnPlane = _currentShip._positionOnPlane;// + new Vector2(point.point.transform.localPosition.x, point.point.transform.localPosition.z);
        _projectile._direction = _currentShip._direction;
        _projectile._rotationalAngle = _currentShip._rotationalAngle;
        _projectile._rotationQuat = _currentShip._rotationQuat;
        _projectile.TranslateBy(point.point.transform.localPosition.x, point.point.transform.localPosition.z);
        _projectile._heightOffset = _projectile._currentPlanet.GetNormalToPlanet(point.point.transform).magnitude - _projectile._currentPlanet.GetRadius();

        point.cannonParticles.Play();

    }
	
}
