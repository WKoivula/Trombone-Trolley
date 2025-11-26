using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;
    public float hitWindow = 0.4f;

    private float laneValue;
    private bool noteShouldBeHit = false;

    private void Awake()
    {
        instance = this;
    }

    public void SetNoteShouldBeHit(bool shouldBeHit)
    {
        noteShouldBeHit = shouldBeHit;
    }

    public void SetCurrentNote(float noteValue)
    {
        laneValue = noteValue;
    }
}
