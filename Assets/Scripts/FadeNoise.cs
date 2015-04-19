// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class FadeNoise : MonoBehaviour {

    AudioSource audioSource;
    float targetVolume = 0.0f;

    void Awake () {
        audioSource = GetComponent<AudioSource>();
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

	void Update () {
        if (audioSource.volume != targetVolume)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, 4.0f * Time.deltaTime);
        }
	}

    void GameOver()
    {
        targetVolume = 0.25f;
    }

    void GameStart()
    {
        targetVolume = 0.0f;
    }
}
