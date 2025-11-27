using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SongHandler;

public class SliderController : MonoBehaviour
{
    public LineRenderer line;
    public GameObject nodePrefab;

    private List<GameObject> nodeObjects = new List<GameObject>();
    private List<Vector3> startPositions = new List<Vector3>();

    private List<float> targetTimes;
    private bool[] alive;
    private double sliderStartTime;
    private double songStartTime;
    private Vector3 hitPosition = Vector3.zero;
    private float delay;
    private Slider slider;
    private float heightPerLane = 1.0f;
    private Vector3 origin;

    private int currentNodeIndex;
    public void Initialize(Slider slider, Vector3 startPos, float delay,
                            double songStartTime, double sliderStartTime, float arrivalSpeed,
                            float heightPerLane)
    {
        this.sliderStartTime = sliderStartTime;
        this.songStartTime = songStartTime;
        this.delay = delay;
        this.slider = slider;
        this.heightPerLane = heightPerLane;
        origin = startPos;
        hitPosition = startPos;

        slider.line = this.line;

        alive = new bool[slider.nodes.Count];

        targetTimes = new List<float>();

        for (int i = 0; i < slider.nodes.Count; i++)
        {
            GameObject n = Instantiate(nodePrefab, transform);
            nodeObjects.Add(n);
            double currSongTime = AudioSettings.dspTime - songStartTime;
            double timeToNode = slider.nodes[i].time - currSongTime;
            Debug.Log("Current song time: " + currSongTime);
            Debug.Log("Time to node: " + timeToNode);
            n.transform.Translate(LaneToPos(slider.nodes[i].lane) + new Vector3(delay * arrivalSpeed + (float)timeToNode * arrivalSpeed, 0, 0));
            startPositions.Add(n.transform.position);
            targetTimes.Add(slider.nodes[i].time);
            alive[i] = true;
            Debug.Log(startPositions.Count);
        }

        StartCoroutine(SetCurrentNode(slider.nodes, AudioSettings.dspTime - songStartTime));

        line.positionCount = nodeObjects.Count;
    }

    IEnumerator SetCurrentNode(List<SliderNode> nodes, double startTime)
    {
        double songTime = startTime;
        for (int i = 0; i < targetTimes.Count; i++)
        {
            double timeToNode = targetTimes[i] - songTime;
            double timeToWaitUntil = AudioSettings.dspTime + timeToNode;
            while (AudioSettings.dspTime < timeToWaitUntil)
            {
                yield return null;
            }
            PlayerHandler.instance.SetNoteShouldBeHit(true);
            currentNodeIndex = i;
            songTime = AudioSettings.dspTime - songStartTime;
        }
        PlayerHandler.instance.SetNoteShouldBeHit(false);
    }

    private Vector3 LaneToPos(float lane)
    {
        return origin + new Vector3(0, lane * heightPerLane, 0);
    }

    private Vector3[] cachedPositions;

    void FixedUpdate()
    {
        if (cachedPositions == null || cachedPositions.Length != nodeObjects.Count)
            cachedPositions = new Vector3[nodeObjects.Count];

        if (AudioSettings.dspTime >= (songStartTime + slider.startTime) && AudioSettings.dspTime <= (songStartTime + slider.endTime))
        {
            if (currentNodeIndex != slider.nodes.Count - 1)
            {
                SliderNode currentNode = slider.nodes[currentNodeIndex];
                SliderNode nextNode = slider.nodes[currentNodeIndex + 1];
                float t = Mathf.InverseLerp(currentNode.time, nextNode.time, (float)(AudioSettings.dspTime - songStartTime));
                Vector3 lanePos = Vector3.Lerp(LaneToPos(currentNode.lane), LaneToPos(nextNode.lane), t);
                PlayerHandler.instance.SetCurrentNote(lanePos.y / (12 * heightPerLane));
                PlayerHandler.instance.SetCurrentLine(slider.line);
            }
        }

        for (int i = 0; i < nodeObjects.Count; i++)
            {
                Vector3 startPos = startPositions[i];
                Vector3 hitPosWithOffset = hitPosition + new Vector3(0, startPos.y, 0);

                double t = InverseLerpUnclamped(sliderStartTime - delay, songStartTime + targetTimes[i], AudioSettings.dspTime);
                nodeObjects[i].transform.position = Vector3.LerpUnclamped(startPos, hitPosWithOffset, (float)t);

                cachedPositions[i] = nodeObjects[i].transform.localPosition;
            }
        line.SetPositions(cachedPositions);
    }
    public static double InverseLerpUnclamped(double a, double b, double value)
    {
        return (value - a) / (b - a);
    }
}
