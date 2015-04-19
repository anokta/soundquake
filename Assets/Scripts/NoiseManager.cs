// ----------------------------------------------------------------------
//   soundquake - Ludum Dare 32 Compo Entry
//
//     Copyright 2015 Alper Gungormusler. All rights reserved.
//
// -----------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class NoiseManager : MonoBehaviour
{
    // Noise maker prefab.
    public GameObject noiseMakerPrefab;

    [HideInInspector]
    public bool recording;

    // Max recording size in seconds.
    public int frequency = 44100;
    public int maxSeconds = 4;
    float startTime;

    // Lastly created noise maker.
    NoiseMaker current;

    void Update()
    {
        if (!recording)
        {
            // Start recording.
            if (Input.GetMouseButtonDown(0))
            {
                recording = true;
                startTime = Time.time;
                current = GameObject.Instantiate(noiseMakerPrefab).GetComponent<NoiseMaker>();
                current.transform.parent = GameManager.levelTransform;
                current.audioSource.clip = Microphone.Start(null, false, maxSeconds, frequency);
                current.gameObject.SetActive(false);
            }
            // Delete the clicked recording.
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hits.Length > 0 && hits[0].transform.tag == "NoiseMaker")
                {
                    GameObject.Destroy(hits[0].transform.gameObject);
                }
            }
        }
        else  // is recording
        {
            // Finish recording and start the playback.
            if (Input.GetMouseButtonUp(0) || !Microphone.IsRecording(null))
            {
                Microphone.End(null);
                int recordSamples = (int)((Time.time - startTime) * frequency);
                float[] recordData = new float[recordSamples];
                current.audioSource.clip.GetData(recordData, 0);
                current.audioSource.clip = AudioClip.Create("Noise", recordSamples, 1, frequency, false);
                current.audioSource.clip.SetData(recordData, 0);
                recording = false; 

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.y = Mathf.Max(-current.transform.localScale.y, position.y);
                position.z = 0.0f;
                current.transform.position = position;
                current.gameObject.SetActive(true);
                current.audioSource.Play();
            }
            // Stop (cancel) recording.
            else if (Input.GetMouseButtonUp(1))
            {
                Microphone.End(null);
                recording = false; 

                GameObject.Destroy(current.gameObject);
            }
        }
    }
}
