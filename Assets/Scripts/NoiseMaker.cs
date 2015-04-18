﻿using UnityEngine;
using System.Collections;

public class NoiseMaker : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;

    float averageForce = 0.0f;
    float lastForce;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lastForce = 0.2f;
    }

    void Update()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.25f, 0.25f), 1.0f, 0.0f) * averageForce * 100);

        lastForce = Mathf.Lerp(lastForce, averageForce, Time.deltaTime * 10.0f);
        GetComponent<Renderer>().material.color = Color.red * Mathf.Max(0.2f, 10.0f * lastForce);
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
