using System;
using UnityEngine;

public class MissedNoteHandler : MonoBehaviour
{
    public static Action<Collider> NoteMissed;
    private Vector3 stopPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stopPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        NoteMissed?.Invoke(other);
        Debug.Log($"MissedNoteHandler: OnTriggerEnter with {other.gameObject.name}");
        stopPos = new Vector3(other.transform.position.x, stopPos.y, other.transform.position.z);
        var glow = other.GetComponent<NoteGlowOnHit>();
        if (glow == null)
            glow = other.GetComponentInParent<NoteGlowOnHit>();

        if (glow != null)
        {
            glow.SetRimColor(Color.red);
            glow.PlayGlowAndDespawn(stopPos);
        }
        Debug.Log("MissedNoteHandler: NoteMissed event invoked");
    }
}
