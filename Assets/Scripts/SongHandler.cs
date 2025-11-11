using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SongHandler : MonoBehaviour
{
    public TextAsset songFile;

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
    }

    Beatmap beatmap;
    private bool isPlaying = false;
    private float time = 0.0f;

    private PlayerInput playerInput;
    private InputAction spaceAction;

    private void Awake()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(songFile.text);
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
            }
        } else
        {
            time += Time.deltaTime;
            Debug.Log(time + " seconds have passed");
        }
    }
}
