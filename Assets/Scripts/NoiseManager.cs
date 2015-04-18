using UnityEngine;
using System.Collections;

public class NoiseManager : MonoBehaviour
{
    // Noise maker prefab.
    public GameObject noiseMakerPrefab;

    // Max recording size in seconds.
    public int frequency = 44100;
    public int maxSeconds = 6;
    float startTime;

    // Lastly created noise maker.
    NoiseMaker current;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (!Microphone.IsRecording(null))
        {
            // Start recording.
            if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
                current = GameObject.Instantiate(noiseMakerPrefab).GetComponent<NoiseMaker>();
                current.GetComponent<Renderer>().material.color = Color.red * Random.Range(0.5f, 1.0f);
                current.audioSource.clip = Microphone.Start(null, false, maxSeconds, frequency);
                current.gameObject.SetActive(false);
            }
            // Delete the clicked recording.
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hits.Length > 0)
                {
                    GameObject.Destroy(hits[0].transform.gameObject);
                }
            }
        }
        else  // is recording
        {
            // Finish recording and start the playback.
            if (Input.GetMouseButtonUp(0))
            {
                Microphone.End(null);
                int recordSamples = (int)((Time.time - startTime) * frequency);
                float[] recordData = new float[recordSamples];
                current.audioSource.clip.GetData(recordData, 0);
                current.audioSource.clip = AudioClip.Create("Noise", recordSamples, 1, frequency, false);
                current.audioSource.clip.SetData(recordData, 0);

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0.0f;
                current.transform.position = position;
                current.gameObject.SetActive(true);
                current.audioSource.Play();
            }
            // Stop (cancel) recording.
            else if (Input.GetMouseButtonUp(1))
            {
                Microphone.End(null);

                GameObject.Destroy(current.gameObject);
            }
        }
    }
}
