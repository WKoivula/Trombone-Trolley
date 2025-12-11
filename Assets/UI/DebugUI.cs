using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public TMP_Text PlayerScoreText;
    [SerializeField] public TMP_Text newSpeedText;
    [SerializeField] public TMP_Text distanceText;
    [SerializeField] public CartMovement cartClass;
    [SerializeField] public PlayerScoremanager PlayerScoreManager;
    [SerializeField] public GameManager GameManager;
    [SerializeField] public GameObject Cart;

    public bool DebugActive;
    LineRenderer lineRenderer;

    void Start()
    {
        if (DebugActive)
        {
            PlayerScoreText.text = $"{cartClass.currentSpeed}";
            newSpeedText.text = $"{cartClass.newSpeed}";
        }
    }
    void Update()
    {
        if (DebugActive)
        {
            PlayerScoreText.text = $"{cartClass.currentSpeed}";
        }
        else
        {
            PlayerScoreText.text = $"{PlayerScoreManager.Score}";
        }
    }

}