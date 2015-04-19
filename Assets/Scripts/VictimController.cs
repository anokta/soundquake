// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class VictimController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rigid;

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
