using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;
    
    public float hitWindow = 0.1f;
    [Range(0.0f, 1.0f)]
    public float currentCursorPos = 0.0f;

    public GameObject cursorPrefab;
    private GameObject cursorObject;

    private float laneValue;
    private bool noteShouldBeHit = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cursorObject = Instantiate(cursorPrefab, transform);
    }

    private void Update()
    {
        cursorObject.transform.localPosition = new Vector3(0, currentCursorPos * 12 * 0.2f, 0);
        if (noteShouldBeHit && currentCursorPos <= laneValue + hitWindow / 2 && currentCursorPos >= laneValue - hitWindow / 2)
        {
            Debug.Log("Perfect hit!");
        }
    }

    // Sets whether a note should be hit currently
    public void SetNoteShouldBeHit(bool shouldBeHit)
    {
        noteShouldBeHit = shouldBeHit;
    }

    // Sets current note, should be normalized between 0 and 1
    public void SetCurrentNote(float noteValue)
    {
        laneValue = noteValue;
    }
}
