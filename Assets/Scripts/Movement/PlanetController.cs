using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

    public float _radius = 15.5f;

    public void AlignToPlanet(Transform body)
    {
        Vector3 normalToPlanet = (body.position - transform.position).normalized;
        body.rotation = Quaternion.FromToRotation(body.transform.up, normalToPlanet) * body.rotation;
    }

}
