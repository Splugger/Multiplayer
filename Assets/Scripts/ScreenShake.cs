using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{

    Vector3 originalPosition;

    public float amplitude = 0;

    float pulseTimer = 0f;
    float maxPulseTimer;
    float pulseAmplitude = 0.5f;

    void Update()
    {
        if (amplitude > 0)
        {
            Vector3 pos = transform.position;
            pos.x += Random.value * amplitude * 2 - amplitude;
            pos.y += Random.value * amplitude * 2 - amplitude;
            transform.position = pos;
        }

        //pulse
        if (pulseTimer <= maxPulseTimer)
        {
            pulseTimer += Time.deltaTime;
        }
        amplitude = (-(pulseTimer + maxPulseTimer) * (pulseTimer - maxPulseTimer)) * pulseAmplitude;
    }

    public void Pulse(float duration, float amplitude)
    {
        pulseTimer = 0f;
        maxPulseTimer = duration;
        pulseAmplitude = amplitude;
    }

}