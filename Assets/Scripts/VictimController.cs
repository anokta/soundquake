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
    public float maxVolume = 0.35f;

    AudioSource audioSource;
    float targetVolume = 0.0f;

    Rigidbody rigid;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material.color = Color.white;
    }

    void OnEnable()
    {
        GameEventManager.GameMenu += GameOver;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void OnDisable()
    {
        GameEventManager.GameMenu -= GameOver;
        GameEventManager.GameStart -= GameStart;
        GameEventManager.GameOver -= GameOver;
    }

    void Update()
    {
        if (GameEventManager.CurrentState == GameEventManager.GameState.Running)
        {
            foreach (NoiseMaker noiseMaker in FindObjectsOfType<NoiseMaker>())
            {
                rigid.AddForce(noiseMaker.GetNoiseForce(transform.position));
            }
        }

        // Update the noise sfx.
        targetVolume = (GameManager.level == 1 && Mathf.Abs(GameManager.levelTransform.position.y) > 1.0f) ?
            maxVolume :
            Mathf.Min(maxVolume, 0.1f * rigid.velocity.magnitude);
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, 4.0f * Time.deltaTime);
    }

    void GameOver()
    {
        targetVolume = maxVolume;
    }

    void GameStart()
    {
        targetVolume = 0.0f;
    }
}
