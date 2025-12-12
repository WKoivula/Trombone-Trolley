using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] public Canvas canvas;

    [Header("Integer Digits")]
    [SerializeField] public TMP_Text hundredsText;
    [SerializeField] public TMP_Text tensText;
    [SerializeField] public TMP_Text onesText;

    [Header("Decimals")]
    [SerializeField] public TMP_Text firstDecimal;
    [SerializeField] public TMP_Text secondDecimal;

    [Header("Debug Info")]
    [SerializeField] public TMP_Text currentSpeedText;
    [SerializeField] public TMP_Text newSpeedText;

    [Header("References")]
    [SerializeField] public PlayerScoremanager PlayerScoreManager;

    public bool DebugActive;

    void Start()
    {
        
    }

    void Update()
    {
      

        float score = PlayerScoreManager.Score;

        // Split integer part
        int intPart = Mathf.FloorToInt(score);

        int hundreds = (intPart / 100) % 10;
        int tens = (intPart / 10) % 10;
        int ones = intPart % 10;

        // Show hundreds only if non-zero
        hundredsText.text = (hundreds > 0) ? hundreds.ToString() : "";

        // Show tens only if hundreds is shown OR tens is non-zero
        tensText.text = (hundreds > 0 || tens > 0) ? tens.ToString() : "";

        // Ones always shown
        onesText.text = ones.ToString();

        // Decimal part (always 2 digits)
        int decimalPart = Mathf.FloorToInt((score - intPart) * 100f);

        int tenths = (decimalPart / 10) % 10;
        int hundredths = decimalPart % 10;

        firstDecimal.text = tenths.ToString();
        secondDecimal.text = hundredths.ToString();
    }
}