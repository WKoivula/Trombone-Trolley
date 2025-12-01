using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SongHandler;

public class SliderController : MonoBehaviour
{
    public LineRenderer line;
    public GameObject nodePrefab;

    [Header("Pitch → Color")]
    public Color lowPitchColor = Color.blue;
    public Color highPitchColor = Color.red;
    public float minLaneValue = 0f;
    public float maxLaneValue = 12f;

    private List<GameObject> nodeObjects = new List<GameObject>();
    private List<Vector3> startPositions = new List<Vector3>();

    private List<float> targetTimes;
    private bool[] alive;
    private double sliderStartTime;
    private double songStartTime;
    private Vector3 hitPosition = Vector3.zero;
    private List<Vector3> prevPositions = new List<Vector3>();
    private float delay;
    private Slider slider;
    private float heightPerLane = 1.0f;
    private Vector3 origin;

    private int currentNodeIndex;
    private Material lineMatInstance;
    private List<Color> noteColors = new List<Color>();

    
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
            if (i == 0 && line != null)
            {
                var noteRenderer = n.GetComponentInChildren<Renderer>();
                if (noteRenderer != null)
                {
                    lineMatInstance = new Material(noteRenderer.sharedMaterial);
                    line.material = lineMatInstance;
                }
            }
            double currSongTime = AudioSettings.dspTime - songStartTime;
            double timeToNode = slider.nodes[i].time - currSongTime;
            Debug.Log("Current song time: " + currSongTime);
            Debug.Log("Time to node: " + timeToNode);
            n.transform.Translate(LaneToPos(slider.nodes[i].lane) + new Vector3(delay * arrivalSpeed + (float)timeToNode * arrivalSpeed, 0, 0));
            n.transform.position = new Vector3(-n.transform.position.x,n.transform.position.y,n.transform.position.z);
            startPositions.Add(n.transform.position);
            prevPositions.Add(n.transform.position);
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
        if (slider == null || nodeObjects.Count == 0)
            return;

        if (cachedPositions == null || cachedPositions.Length != nodeObjects.Count)
            cachedPositions = new Vector3[nodeObjects.Count];

        if (AudioSettings.dspTime >= (songStartTime + slider.startTime) &&
            AudioSettings.dspTime <= (songStartTime + slider.endTime))
        {
            if (currentNodeIndex != slider.nodes.Count - 1)
            {
                SliderNode currentNode = slider.nodes[currentNodeIndex];
                SliderNode nextNode = slider.nodes[currentNodeIndex + 1];

                float tLine = Mathf.InverseLerp(
                    currentNode.time,
                    nextNode.time,
                    (float)(AudioSettings.dspTime - songStartTime)
                );

                Vector3 lanePos = Vector3.Lerp(
                    LaneToPos(currentNode.lane),
                    LaneToPos(nextNode.lane),
                    tLine
                );

                PlayerHandler.instance.SetCurrentNote(lanePos.y / (12 * heightPerLane));
                PlayerHandler.instance.SetCurrentLine(slider.line);
            }
        }

        bool anyAlive = false; // <--- track if any note in this slider is still active

        for (int i = 0; i < nodeObjects.Count; i++)
        {
            GameObject note = nodeObjects[i];
            if (note == null)
                continue;

            Vector3 lastPos = prevPositions[i];
            Vector3 startPos = startPositions[i];
            Vector3 hitPosWithOffset = hitPosition + new Vector3(0, startPos.y, 0);

            double tDouble = InverseLerpUnclamped(
                sliderStartTime - delay,
                songStartTime + targetTimes[i],
                AudioSettings.dspTime
            );
            float t = (float)tDouble;

            float tClamped = Mathf.Clamp01(t);
            Vector3 lineWorldPos = Vector3.LerpUnclamped(startPos, hitPosWithOffset, tClamped);
            Vector3 lineLocalPos = transform.InverseTransformPoint(lineWorldPos);
            cachedPositions[i] = lineLocalPos;

            if (alive[i])
            {
                anyAlive = true; // still at least one live note in this slider

                note.transform.position = lineWorldPos;

                if (t >= 1f)
                {
                    Vector3 velocity = note.transform.position - lastPos;
                    float brakeFactor = 3f;
                    Vector3 stopPos = note.transform.position + velocity * brakeFactor;

                    var glow = note.GetComponent<NoteGlowOnHit>();
                    if (glow != null)
                        glow.PlayGlowAndDespawn(stopPos);
                    else
                        Destroy(note);

                    alive[i] = false;
                }
            }

            prevPositions[i] = note.transform.position;
        }

        // If there are still live notes, keep drawing the line.
        // If ALL notes in this slider have hit, kill the line.
        if (anyAlive)
        {
            line.positionCount = cachedPositions.Length;
            line.SetPositions(cachedPositions);
        }
        else
        {
            line.positionCount = 0;
            // Optional: also disable or destroy the line GameObject if you want it truly gone:
            // line.enabled = false;
            // Destroy(line.gameObject);
        }
    }


    
    public static double InverseLerpUnclamped(double a, double b, double value)
    {
        return (value - a) / (b - a);
    }

}
