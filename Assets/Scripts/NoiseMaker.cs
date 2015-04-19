// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class NoiseMaker : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;

    float averageForce, lastForceMagnitude;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
        averageForce = 0.0f;
        lastForceMagnitude = 0.0f;
        GetComponent<Renderer>().material.color = Color.red * 0.25f;
    }

    void Update()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.25f, 0.25f), 1.0f, 0.0f) * Mathf.Min(Physics.Raycast(transform.position, Vector3.down, 1.0f) ? 20.0f : 7.5f, averageForce * 110.0f));

        lastForceMagnitude = Mathf.Lerp(lastForceMagnitude, averageForce, Time.deltaTime * 10.0f);
        GetComponent<Renderer>().material.color = new Color(1.0f, 0.25f, 0.25f) * Mathf.Max(0.25f, 25.0f * lastForceMagnitude);
        GetComponent<Light>().range = Mathf.Min(1000.0f, 50.0f * lastForceMagnitude);
    }

    void OnAudioFilterRead(float[] data, int length)
    {
        float total = 0.0f;
        for (int i = 0; i < data.Length; ++i)
        {
            total += Mathf.Abs(data[i]);
        }
        averageForce = total / data.Length;
    }

    public float GetForceMagnitude()
    {
        return averageForce;
    }

    public Vector3 GetNoiseForce(Vector3 target)
    {
        return 5.0f * averageForce * (target - transform.position) / Vector3.Distance(target, transform.position);
    }
}
