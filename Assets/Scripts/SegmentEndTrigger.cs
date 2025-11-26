using UnityEngine;

public class SegmentEndTrigger : MonoBehaviour
{
    private SegmentGenerator generator;

    void Start()
    {
        generator = FindAnyObjectByType<SegmentGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject segmentRoot = transform.root.gameObject;
            generator.OnSegmentPassed(segmentRoot);
        }
    }
}
