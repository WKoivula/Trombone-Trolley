using Unity.Collections;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using jp.keijiro.lasp;
public class MicInput : MonoBehaviour
{
    //public float PlayerHandler.instance.currentCursorPos();
    AudioClip microphoneInput;
    Lasp.AudioLevelTracker audioLevelTracker;
    bool microphoneInitialized;
    public float sensitivity = 0f;
    public float volume = 0.1f;
    AudioSource audioSource;
    Lasp.SpectrumAnalyzer spectrumAnalyzer;
    float ratio = 415.3f / 440f;
    //[Range(-10f, 10f)] public float steps = 0f;
    [Range(0f, 1f)] public float slider = 0f;

    [Range(-2, 2)] public float octaveBase = 0;
    public int octave = 0;

    public static MicInput instance;

    public bool isBlowing = false;

    // Public safe accessor for current output volume so other scripts
    // don't need to reference the AudioSource field directly.
    public float CurrentVolume
    {
        get { return audioSource != null ? audioLevelTracker.inputLevel : 0f; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

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
        spectrumAnalyzer = GetComponent<Lasp.SpectrumAnalyzer>();
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
        NativeArray<float> spectrum = spectrumAnalyzer.spectrumArray;
        if (Input.GetKey("p"))
        {
            int maxIndex = -1;
            float maxValue = float.MinValue;
            for (int i = 0; i < spectrum.Length; i++)
            {
                if (spectrum[i] > maxValue)
                {
                    maxValue = spectrum[i];
                    maxIndex = i;
                }
            }
            if (maxIndex <= 10)
            {
                octave = -1;
            } else if (maxIndex <= 20)
            {
                octave = 0;
            } else if (maxIndex <= 30)
            {
                octave = 1;
            }
            // print("Max index: " + maxIndex + ", Max Value: " + maxValue);
        }
        

        float steps = -10 + 12 * PlayerHandler.instance.currentCursorPos + 12 * ((float)octave + octaveBase);
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
        if (level > sensitivity && CurrentVolume > -70f)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
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
