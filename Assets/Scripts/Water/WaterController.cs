using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    public static WaterController instance;

    public bool isMoving;

    public float scale = 0.1f;
    public float speed = 1.0f;
    public float waveDistance = 1f;
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;

    private float o_scale = 0.1f;
    private float o_speed = 1.0f;
    private float o_waveDistance = 1f;
    private float o_noiseStrength = 1f;
    private float o_noiseWalk = 1f;

    // Use this for initialization
    void Start () {
        instance = this;
        o_scale = scale;
        o_speed = speed;
        o_waveDistance = waveDistance;
        o_noiseStrength = noiseStrength;
        o_noiseWalk = noiseWalk;
	}

    void Update()
    {
        Shader.SetGlobalFloat("_WaterScale", scale);
        Shader.SetGlobalFloat("_WaterSpeed", speed);
        Shader.SetGlobalFloat("_WaterDistance", waveDistance);
        Shader.SetGlobalFloat("_WaterTime", Time.time);
        Shader.SetGlobalFloat("_WaterNoiseStrength", noiseStrength);
        Shader.SetGlobalFloat("_WaterNoiseWalk", noiseWalk);
    }

    public void ApplyModifier(float mod = 1)
    {
        scale = o_scale / mod;
        speed = o_speed / mod;
        waveDistance = o_waveDistance / mod;
        noiseStrength = o_noiseStrength * mod;
        noiseWalk = o_noiseWalk * mod;
    }

    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        if (isMoving)
        {
            return WaveTypes.SinXWave(position, speed, scale, waveDistance, noiseStrength, noiseWalk, timeSinceStart);
        }
        else
        {
            return 0f;
        }

    }

    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float waterHeight = GetWaveYPos(position, timeSinceStart);

        float distanceToWater = waterHeight;

        return distanceToWater;
    }

}
