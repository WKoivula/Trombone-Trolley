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
    private float songStartTime = 0.0f;

    private PlayerInput playerInput;
    private InputAction spaceAction;

    private void Awake()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(songFile.text);
        beatmap.arrivalSpeed = noteArrivalSpeed;
        playerInput = GetComponent<PlayerInput>();
        spaceAction = playerInput.actions["Jump"];
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
                songStartTime = Time.time;
                StartCoroutine(PlayBeatmap(beatmap, songStartTime));
            }
        }
    }

    IEnumerator PlayBeatmap(Beatmap beatmap, float songStartTime)
    {
        for (int i = 0; i < beatmap.sliders.Count; i++)
        {
            float spawnTime = beatmap.sliders[i].startTime - beatmap.arrivalSpeed - delayToStartSlider;

            while (Time.time - songStartTime < spawnTime)
                yield return null;

            SpawnSlider(beatmap.sliders[i], songStartTime);
        }
    }

    void SpawnSlider(Slider slider, float songStartTime)
    {
        GameObject sliderObj = Instantiate(sliderPrefab);
        SliderController controller = sliderObj.GetComponent<SliderController>();

        controller.Initialize(slider, transform.position, delayToStartSlider, songStartTime, Time.time, beatmap.arrivalSpeed);
    }
}
