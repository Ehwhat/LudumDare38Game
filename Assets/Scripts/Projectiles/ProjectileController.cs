using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SphericalMovementController {

    public ParticleSystem _hitParticles;
    public float _gravity = 0.981f;
    public LayerMask _hitMask;
    public float _hitRadius = 0.5f;

    public float _damage = 10f;

    private bool isAlive = true;
    private Vector3 _velocity = Vector3.zero;

	// Use this for initialization
	void Awake () {
        base.Awake();
        //Initialise(_direction, transform.forward);
	}

    public void Initialise(Vector3 direction, PlanetController planet)
    {
        SetPlanet(planet);
        _velocity = direction;
    }
	
	// Update is called once per frame
	void Update () {
        if (isAlive)
        {
            if (_heightOffset + _radius < _radius - 3)
            {
                Destroy(gameObject);
            }

            _heightOffset -= _gravity * Time.deltaTime;

            TranslateBy(_velocity.x, _velocity.y);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _hitRadius);
            if (hitColliders.Length > 0)
            {
                foreach (Collider c in hitColliders)
                {
                    IDamageableObject damage = c.transform.root.GetComponent<IDamageableObject>();
                    if (damage != null)
                    {
                        damage.OnHit(this);
                    }
                }
                OnHit();
            }
            Movement();
        }
    }

    void OnHit()
    {
        _hitParticles.Play();
        isAlive = false;
        Destroy(gameObject, _hitParticles.main.duration);
    }

}
