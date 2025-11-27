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
        public List<SliderNode> nodes = new List<SliderNode>();

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

    private PlayerInput playerInput;
    private InputAction spaceAction;

    private void Awake()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(songFile.text);
        beatmap.arrivalSpeed = noteArrivalSpeed;
        playerInput = GetComponent<PlayerInput>();
        spaceAction = playerInput.actions["Jump"];
        mapLineRenderer = GetComponentInChildren<LineRenderer>();
        songAudioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if (spaceAction.WasPressedThisFrame())
            {
                isPlaying = true;
                songStartTime = AudioSettings.dspTime;
                songAudioSource.PlayScheduled(songStartTime);
                StartCoroutine(PlayBeatmap(beatmap, songStartTime));
            }
        }
    }

    public Vector3 LaneToPos(float lane)
    {
        return new Vector3(0, lane * heightPerLane, 0);
    }

    IEnumerator PlayBeatmap(Beatmap beatmap, double songStartTime)
    {
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

        controller.Initialize(slider, transform.position, delayToStartSlider, songStartTime, AudioSettings.dspTime, beatmap.arrivalSpeed, heightPerLane);
    }
}
