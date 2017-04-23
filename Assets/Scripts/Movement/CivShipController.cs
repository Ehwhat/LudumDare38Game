using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivShipController : ShipMovementController {

    [System.Serializable]
    public struct AvoidPoint
    {
        public AvoidPoint(Vector3 point, float importance, float creationTime)
        {
            _point = point;
            _importance = importance;
            _creationTime = creationTime;
        }

        public Vector2 _point;
        public float _importance;
        public float _creationTime;
    }

    public float _detectionRadius;
    public float _avoidPointCareTime = 10f;
    public Collider _shipCollider;
    public GameObject _meshHolder;
    public GameObject _fuelDrop;
    public ParticleSystem _deathParticle;

    public List<AvoidPoint> avoidPoints = new List<AvoidPoint>();

    public Vector2 targetPoint;

	// Use this for initialization
	void Start () {
        GetNewTargetPoint();
        StartCoroutine(DetectStep());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        ShipMovementStep(SteerToPoint(targetPoint));
	}

    IEnumerator DetectStep()
    {
        while (true)
        {
            CullAvoidPoints();
            Collider[] detectedColliders = Physics.OverlapSphere(transform.position, _detectionRadius);
            if (detectedColliders.Length > 0)
            {
                foreach (Collider c in detectedColliders)
                {
                    ProcessDetection(c);
                }
            }
            targetPoint = GetNewTargetPoint();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void ProcessDetection(Collider detectedCollider)
    {
        if(detectedCollider == _shipCollider) { return; }
        if(detectedCollider.tag == "Player")
        {
            SphericalMovementController controller = detectedCollider.transform.root.GetComponent<SphericalMovementController>();
            if (controller)
            {
                AddAvoidPoint(new AvoidPoint(controller._positionOnPlane, 1, Time.time));
            }
        }
        if(detectedCollider.tag == "Projectile")
        {
            SphericalMovementController controller = detectedCollider.transform.root.GetComponent<SphericalMovementController>();
            if (controller)
            {
                AddAvoidPoint(new AvoidPoint(controller._positionOnPlane, 0.2f, Time.time));
            }
        }
    }

    Vector2 GetNewTargetPoint()
    {
        Vector2 bestPoint = _positionOnPlane + (Random.insideUnitCircle * 10) + _direction * 10;
        float bestScore = ScorePoint(bestPoint);
        for (int i = 0; i < 10; i++)
        {
            Vector2 newPoint = _positionOnPlane + (Random.insideUnitCircle * 10) + _direction * 10;
            float score = ScorePoint(bestPoint);
            if(score > bestScore)
            {
                bestPoint = newPoint;
                bestScore = score;
            }
        }
        return bestPoint;
    }

    float ScorePoint(Vector2 point)
    {
        float score = 0;
        
        foreach (AvoidPoint avoid in avoidPoints)
        {
            score -= ((point - avoid._point) * avoid._importance).magnitude;
        }
        return score;
    }

    float SteerToPoint(Vector2 pointOnPlane)
    {
        float angleStep = Mathf.Lerp(_rotationalAngle, Vector2.Angle(_positionOnPlane, pointOnPlane), Time.deltaTime);
        return(Vector2.Angle(_positionOnPlane, pointOnPlane)*Time.deltaTime);
    }

    void AddAvoidPoint(AvoidPoint point)
    {
        avoidPoints.Add(point);
    }

    void CullAvoidPoints()
    {
        for(int i = 0; i < avoidPoints.Count; i++)
        {
            if(Time.time - avoidPoints[i]._creationTime > _avoidPointCareTime)
            {
                avoidPoints.RemoveAt(i);
            }
        }
    }

    public override void OnDeath()
    {
        _deathParticle.Play();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(_meshHolder);
        Destroy(gameObject, _deathParticle.main.duration);
        Instantiate(_fuelDrop,transform.position + transform.up,transform.rotation);
    }

}
