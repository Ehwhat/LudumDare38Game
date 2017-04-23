using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    public float _radius = 15.5f;
    public float _radiusModifier = 1;
    public GameObject _meshHolder;
    public WaterController _waterController;

    public void Update()
    {
        _meshHolder.transform.localScale = Vector3.one * ((_radius* _radiusModifier) * 2);
        _waterController.ApplyModifier(_radiusModifier);
    }

    public void AlignToPlanet(Transform body)
    {
        Vector3 normalToPlanet = (body.position - transform.position).normalized;
        body.rotation = Quaternion.FromToRotation(body.transform.up, normalToPlanet) * body.rotation;
    }

    public float GetRadius()
    {
        return _radius * _radiusModifier;
    }

    public Vector3 GetNormalToPlanet(Transform body)
    {
        return (body.position - transform.position);
    }

}
