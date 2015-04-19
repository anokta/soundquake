// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
    float targetRotationAngle = 0.0f;

    void OnEnable()
    {
        GameEventManager.GameStart += GameStart;
    }

    void OnDisable()
    {
        GameEventManager.GameStart -= GameStart;
    }
	
	void Update () {
        float totalForce = 0.0f;
        foreach (NoiseMaker noiseMaker in FindObjectsOfType<NoiseMaker>())
        {
            totalForce += noiseMaker.averageForce;
        }
        targetRotationAngle = totalForce;

        Camera.main.transform.rotation =
           Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.AngleAxis(targetRotationAngle * Random.Range(-60.0f, 60.0f),
                    Vector3.forward), 2.0f * Time.deltaTime);
	}

    void GameStart()
    {
        Camera.main.transform.rotation = Quaternion.identity;
    }
}
