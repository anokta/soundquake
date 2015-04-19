using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
    Camera camera;

    float targetRotationAngle = 0.0f;

	void Awake () {
        camera = Camera.main;	
	}

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

        camera.transform.rotation = 
           Quaternion.Slerp(camera.transform.rotation, Quaternion.AngleAxis(targetRotationAngle * Random.Range(-45.0f, 45.0f),
                    Vector3.forward), Time.deltaTime);
	}

    void GameStart()
    {
        camera.transform.rotation = Quaternion.identity;
    }
}
