using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public TMP_Text currentSpeedText;
    [SerializeField] public TMP_Text newSpeedText;
    [SerializeField] public TMP_Text distanceText;
    [SerializeField] public CartMovement cartClass;
    [SerializeField] public DistanceHandler distanceHandlerClass;

    public bool DebugActive;
    LineRenderer lineRenderer;

    void Start()
    {
        if (DebugActive)
        {
            currentSpeedText.text = $"{cartClass.currentSpeed}";
            newSpeedText.text = $"{cartClass.newSpeed}";
        }
    }
    void Update()
    {
        if (DebugActive)
        {
            distanceHandlerClass.
            currentSpeedText.text = $"{cartClass.currentSpeed}";
            newSpeedText.text = $"{cartClass.newSpeed}";
        }
        distanceText.text = $"Distance: {distanceHandlerClass.distanceTraveled}";
    }

}