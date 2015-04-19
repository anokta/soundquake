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

    [HideInInspector]
    public float averageForce;

    float lastForce;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
        averageForce = 0.0f;
        lastForce = 0.0f;
        GetComponent<Renderer>().material.color = Color.red * 0.25f;
    }

    void Update()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.25f, 0.25f), 1.0f, 0.0f) * Mathf.Min(transform.position.y > -0.5f ? 7.5f : 20.0f, averageForce * 100.0f));

        lastForce = Mathf.Lerp(lastForce, averageForce, Time.deltaTime * 10.0f);
        GetComponent<Renderer>().material.color = new Color(1.0f, 0.25f, 0.25f) * Mathf.Max(0.25f, 25.0f * lastForce);
        GetComponent<Light>().range = Mathf.Min(1000.0f, 50.0f * lastForce);
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

    public Vector3 GetNoiseForce(Vector3 target)
    {
        return 5.0f * averageForce * (target - transform.position) / Vector3.Distance(target, transform.position);
    }
}
