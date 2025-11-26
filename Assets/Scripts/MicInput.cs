using Unity.VisualScripting;
using UnityEngine;

public class MicInput : MonoBehaviour
{
    AudioClip microphoneInput;
    Lasp.AudioLevelTracker audioLevelTracker;
    bool microphoneInitialized;
    public float sensitivity = 0f;
    AudioSource audioSource;
    float ratio = 415.3f / 440f;
    [Range(-10f, 10f)] public float steps = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void Awake()
    //{
    //    if(Microphone.devices.Length > 0)
    //    {
    //        microphoneInput = Microphone.Start(Microphone.devices[0], true, 10, 44100);
    //        microphoneInitialized = true;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No microphone detected!");
    //        microphoneInitialized = false;
    //    }
    //}

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.volume = 0f;
        audioLevelTracker = GetComponent<Lasp.AudioLevelTracker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //int dec = 256;
        //float[] waveData = new float[dec];
        //int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
        //microphoneInput.GetData(waveData, micPosition);

        //// Getting a peak on the last 128 samples
        //float levelMax = 0;
        //for (int i = 0; i < dec; i++)
        //{
        //    float wavePeak = waveData[i] * waveData[i];
        //    if (levelMax < wavePeak)
        //    {
        //        levelMax = wavePeak;
        //    }
        //}
        //float level = Mathf.Sqrt(Mathf.Sqrt(levelMax));

        if (steps < 0)
        {
            audioSource.pitch = Mathf.Pow(ratio, -steps);
        }
        else if (steps > 0)
        {
            audioSource.pitch = 1f / Mathf.Pow(ratio, steps);
        }
        else
        {
            audioSource.pitch = 1f;
        }
        float level = audioLevelTracker.inputLevel;

        if (level > sensitivity || Input.GetKey("p"))
        {
            if (audioSource != null)
            {
                audioSource.volume = 1f;
            }
        }
        else
        {
            if (audioSource != null)
            {
                audioSource.volume = 0f;
            }
        }
    }
}
