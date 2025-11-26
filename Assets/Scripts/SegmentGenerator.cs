using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] segmentPrefabs;
    public float segmentLength = 50f;
    public int maxSegments = 4;

    [Header("Debug")]
    public float nextXPos = -50f;

    private readonly List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < maxSegments-1; i++)
        {
            SpawnSegment();
        }
    }

    public void SpawnSegment()
    {
        int index = Random.Range(0, segmentPrefabs.Length); 
        GameObject newSeg = Instantiate(
            segmentPrefabs[index],
            new Vector3(nextXPos, 0f, 0f),
            Quaternion.identity
        );

        activeSegments.Add(newSeg);
        nextXPos -= segmentLength; 
    }

    public void OnSegmentPassed(GameObject segment)
    {
        SpawnSegment();

        if (activeSegments.Count > maxSegments)
        {
            GameObject oldest = activeSegments[0];
            activeSegments.RemoveAt(0);
            Destroy(oldest);
        }
    }
}
