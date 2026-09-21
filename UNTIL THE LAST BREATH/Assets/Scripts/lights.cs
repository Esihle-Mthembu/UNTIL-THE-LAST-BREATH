using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 5f;
    public float maxIntensity = 60f;
    public float speed = 8f;

    void Reset() { targetLight = GetComponent<Light>(); }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * speed, 0f);
        // occasional hard cut-out
        if (Random.value < 0.01f) noise = 0f;
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}