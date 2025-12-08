using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SongHandler : MonoBehaviour
{
    public TextAsset songFile;
    public GameObject sliderPrefab;

    [Header("Beatmap parameters")]
    public float delayToStartSlider = 3.0f;
    public float noteArrivalSpeed = 3.0f;
    public float heightPerLane = 0.2f;
    public float delayToStartMap = 0.4f;

    private LineRenderer mapLineRenderer;
    private AudioSource songAudioSource;

    [System.Serializable]
    public class SliderNode
    {
        public float time;
        public float lane;
    }

    [System.Serializable]
    public class Slider
    {
        public int id;
        public int octave;
        public List<SliderNode> nodes = new List<SliderNode>();
        public LineRenderer line;

        public float startTime => nodes.Count > 0 ? nodes[0].time : 0f;
        public float endTime => nodes.Count > 0 ? nodes[^1].time : 0f;
    }

    [System.Serializable]
    public class Beatmap
    {
        public List<Slider> sliders = new List<Slider>();
        public float length;
        public float arrivalSpeed;
    }

    Beatmap beatmap;
    private bool isPlaying = false;
    private double songStartTime = 0.0f;

    private void Awake()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(songFile.text);
        beatmap.arrivalSpeed = noteArrivalSpeed;
        mapLineRenderer = GetComponentInChildren<LineRenderer>();
        songAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = Vector3.zero;
        linePositions[1] = new Vector3(0, 12 * heightPerLane, 0); ;
        mapLineRenderer.SetPositions(linePositions);
        
        if (CartMovement.instance != null)
        {
            CartMovement.instance.ParentToBeatmapContainer(transform, true);
        }
    }

    void Update()
    {
        // Start spawning notes when game state changes to Playing
        if (!isPlaying && GameManager._instance != null && GameManager._instance.currentState == GameManager.GameState.Playing)
        {
            isPlaying = true;
            songStartTime = AudioSettings.dspTime;
            songAudioSource.PlayScheduled(songStartTime);
            StartCoroutine(PlayBeatmap(beatmap, songStartTime));
        }
        
        // Reset isPlaying when game state is not Playing (for restart functionality)
        if (isPlaying && GameManager._instance != null && GameManager._instance.currentState != GameManager.GameState.Playing)
        {
            isPlaying = false;
            // Stop the audio if needed
            if (songAudioSource.isPlaying)
            {
                songAudioSource.Stop();
            }
            // Stop all coroutines to prevent spawning
            StopAllCoroutines();
        }
    }

    IEnumerator PlayBeatmap(Beatmap beatmap, double songStartTime)
    {
        yield return new WaitForSeconds(delayToStartMap);
        songStartTime = AudioSettings.dspTime;
        for (int i = 0; i < beatmap.sliders.Count; i++)
        {
            float spawnTime = beatmap.sliders[i].startTime - beatmap.arrivalSpeed - delayToStartSlider;

            while (AudioSettings.dspTime - songStartTime < spawnTime)
                yield return null;

            SpawnSlider(beatmap.sliders[i], songStartTime);
        }
    }

    void SpawnSlider(Slider slider, double songStartTime)
    {
        GameObject sliderObj = Instantiate(sliderPrefab);
        SliderController controller = sliderObj.GetComponent<SliderController>();
        
        if (CartMovement.instance != null)
        {
            CartMovement.instance.ParentToBeatmapContainer(sliderObj.transform, true);
        }

        controller.Initialize(slider, transform.position, delayToStartSlider, songStartTime, AudioSettings.dspTime, beatmap.arrivalSpeed, heightPerLane);
    }
    
}
