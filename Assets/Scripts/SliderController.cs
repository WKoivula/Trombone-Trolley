using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static SongHandler;
using static UnityEditor.PlayerSettings;

public class SliderController : MonoBehaviour
{
    public LineRenderer line;
    public GameObject nodePrefab;

    private List<GameObject> nodeObjects = new List<GameObject>();
    private List<Vector3> startPositions = new List<Vector3>();

    private List<float> targetTimes;
    private bool[] alive;
    private float sliderStartTime;
    private float songStartTime;
    private Vector3 hitPosition = Vector3.zero;
    private float delay;
    private SongHandler.Slider slider;
    private float heightPerLane = 1.0f;

    public float heightOffset;

    private int currentNodeIndex;
    public void Initialize(SongHandler.Slider slider, Vector3 startPos, float delay,
                            float songStartTime, float sliderStartTime, float arrivalSpeed,
                            float heightPerLane)
    {
        this.sliderStartTime = sliderStartTime;
        this.songStartTime = songStartTime;
        this.delay = delay;
        this.slider = slider;
        this.heightPerLane = heightPerLane;

        alive = new bool[slider.nodes.Count];

        targetTimes = new List<float>();

        for (int i = 0; i < slider.nodes.Count; i++)
        {
            GameObject n = Instantiate(nodePrefab, transform);
            nodeObjects.Add(n);
            float currSongTime = Time.time - songStartTime;
            float timeToNode = slider.nodes[i].time - currSongTime;
            Debug.Log("Current song time: " + currSongTime);
            Debug.Log("Time to node: " + timeToNode);
            //bbytte riktninng och höjd y till 1f
            n.transform.Translate(LaneToPos(slider.nodes[i].lane) + new Vector3(-(delay * arrivalSpeed + timeToNode * arrivalSpeed), 1f, 0));
            startPositions.Add(n.transform.position);
            targetTimes.Add(slider.nodes[i].time);
            alive[i] = true;
            Debug.Log(startPositions.Count);
        }

        StartCoroutine(SetCurrentNode(slider.nodes, Time.time - songStartTime));

        line.positionCount = nodeObjects.Count;
    }

    IEnumerator SetCurrentNode(List<SliderNode> nodes, float startTime)
    {
        float songTime = startTime;
        for (int i = 0; i < targetTimes.Count; i++)
        {
            float timeToNode = targetTimes[i] - songTime;
            yield return new WaitForSeconds(timeToNode);
            PlayerHandler.instance.SetNoteShouldBeHit(true);
            currentNodeIndex = i;
            songTime = Time.time - songStartTime;
        }
        PlayerHandler.instance.SetNoteShouldBeHit(false);
    }

    private Vector3 LaneToPos(float lane)
    {
        return new Vector3(0, lane * heightPerLane, 0);
    }

    private Vector3[] cachedPositions;

    void FixedUpdate()
    {
        if (slider == null || nodeObjects.Count == 0)
            return;

        if (cachedPositions == null || cachedPositions.Length != nodeObjects.Count)
            cachedPositions = new Vector3[nodeObjects.Count];

        if (Time.time >= (songStartTime + slider.startTime) && Time.time <= (songStartTime + slider.endTime))
        {
            if (currentNodeIndex != slider.nodes.Count - 1)
            {
                SliderNode currentNode = slider.nodes[currentNodeIndex];
                SliderNode nextNode = slider.nodes[currentNodeIndex + 1];
                float t = Mathf.InverseLerp(currentNode.time, nextNode.time, Time.time - songStartTime);
                Vector3 lanePos = Vector3.Lerp(LaneToPos(currentNode.lane), LaneToPos(nextNode.lane), t);
                PlayerHandler.instance.SetCurrentNote(lanePos.y / (12 * heightPerLane));
            }
        }

        for (int i = 0; i < nodeObjects.Count; i++)
            {
                // la till + new Vector3(0, heightOffset, 0);
                Vector3 startPos = startPositions[i] + new Vector3(0, heightOffset, 0);
                Vector3 hitPosWithOffset = hitPosition + new Vector3(0, startPos.y, 0);

                float t = InverseLerpUnclamped(sliderStartTime - delay, songStartTime + targetTimes[i], Time.time);
                nodeObjects[i].transform.position = Vector3.LerpUnclamped(startPos, hitPosWithOffset, t);

                cachedPositions[i] = nodeObjects[i].transform.localPosition;
            }
        line.SetPositions(cachedPositions);
    }
    public static float InverseLerpUnclamped(float a, float b, float value)
    {
        return (value - a) / (b - a);
    }
}
