using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform _targetTransform;
    public Vector3 _cameraOffset;
    public Material _atmosphereMaterial;
    public Material _spaceMaterial;
    public float zoomSpaceTheshold = 60f;
    public float zoomSpaceLimit = 80f;
    private Material _skyboxMaterial;
    public float _minRadius = 8f;
    public float _maxRadius = 125f;

    [SerializeField]
    public bool _enableControls = true;
    [SerializeField]
    public bool _followTarget = true;
    [SerializeField]
    public bool _isActive = true;

    [SerializeField]
    private float _radius = 10;
    [SerializeField]
    private float _polar;
    [SerializeField]
    private float _elevation;

	// Use this for initialization
	void Start () {
        CameraMovementStep();
        _skyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = _skyboxMaterial;
        _skyboxMaterial = _atmosphereMaterial;
    }
	
	// Update is called once per frame
	void Update () {
        if (_isActive)
        {
            if (_enableControls)
            {
                HandlePlayerInput();
            }
            if (_followTarget)
            {
                CameraMovementStep();
            }
        }
    }

    void HandlePlayerInput()
    {
        float axisX = Input.GetAxis("Mouse X");
        float axisY = Input.GetAxis("Mouse Y");
        Vector2 mouseWheelDelta = -Input.mouseScrollDelta;

        _radius += mouseWheelDelta.y;

        _polar += axisX;
        _elevation += axisY;

        _elevation = Mathf.Clamp(_elevation, 0, Mathf.PI/3f);
        _radius = Mathf.Clamp(_radius, _minRadius, _maxRadius);

        /*if (_radius > zoomSpaceTheshold)
        {
            float maxAmount = _maxRadius - zoomSpaceLimit;
            float amount = (_maxRadius - _radius)-zoomSpaceTheshold;
            Debug.Log(amount / maxAmount);
            //SetSkyboxColour(Mathf.Abs(amount / maxAmount));
        }*/

    }

    void CameraMovementStep()
    {
        transform.position = _targetTransform.position + ((_targetTransform.rotation * Quaternion.Euler(_targetTransform.forward)) * SphericalToCartesian(_radius, _polar, _elevation));
        transform.LookAt(_targetTransform,_targetTransform.up);
    }

    /*void SetSkyboxColour(float t)
    {
        Material newMaterial = new Material(_skyboxMaterial);
        newMaterial.Lerp(_atmosphereMaterial, _spaceMaterial, 1);
        RenderSettings.skybox = newMaterial;
    }*/

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation)
    {
        Vector3 result = Vector3.zero;
        float a = radius * Mathf.Cos(elevation);
        result.x = a * Mathf.Cos(polar);
        result.y = radius * Mathf.Sin(elevation);
        result.z = a * Mathf.Sin(polar);
        return result;
    }


}
