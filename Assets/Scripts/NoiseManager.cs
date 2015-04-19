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
    public GameObject noiseMakerPrefab;
    
    // Recording properties.
    public int frequency = 44100;
    public int maxSeconds = 4;
    
    float startTime;
    bool recording;

    // Lastly created noise maker.
    NoiseMaker current;

    void OnDisable()
    {
        recording = false;
        Microphone.End(null);
    }

    void Update()
    {
        if (!recording)
        {
            if (Input.GetMouseButtonDown(0) && FindObjectsOfType<NoiseMaker>().Length < 32)
            {
                // Start recording.
                recording = true;
                startTime = Time.time;
                current = GameObject.Instantiate(noiseMakerPrefab).GetComponent<NoiseMaker>();
                current.transform.parent = GameManager.levelTransform;
                current.audioSource.clip = Microphone.Start(null, false, maxSeconds, frequency);
                current.gameObject.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                // Delete the clicked noise maker.
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hits.Length > 0 && hits[0].transform.tag == "NoiseMaker")
                {
                    GameObject.Destroy(hits[0].transform.gameObject);
                }
            }
        }
        else  // is recording
        {
            if (Input.GetMouseButtonUp(0) || !Microphone.IsRecording(null))
            {
                // Finish recording and start the playback.
                Microphone.End(null);
                int recordSamples = (int)((Time.time - startTime) * frequency);
                float[] recordData = new float[recordSamples];
                current.audioSource.clip.GetData(recordData, 0);
                current.audioSource.clip = AudioClip.Create("Noise", recordSamples, 1, frequency, false);
                current.audioSource.clip.SetData(recordData, 0);
                recording = false; 

                // Spawn the noise maker.
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                current.transform.position = new Vector3(position.x, Mathf.Max(-current.transform.localScale.y, position.y), 0.0f);
                current.gameObject.SetActive(true);
                current.audioSource.Play();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                // Stop (cancel) recording.
                Microphone.End(null);
                recording = false; 

                GameObject.Destroy(current.gameObject);
            }
        }
    }
}
