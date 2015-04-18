using UnityEngine;
using System.Collections;

public class VictimController : MonoBehaviour
{
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material.color = Color.blue;
    }

    void Update()
    {
        foreach (NoiseMaker noiseMaker in FindObjectsOfType<NoiseMaker>())
        {
            rigid.AddForce(noiseMaker.GetNoiseForce(transform.position));
        }
    }
}
