using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VR_UI : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public TMP_Text buttonText;
    [SerializeField] public Button button;
    [SerializeField] public PlayerScoremanager PlayerScoreManager;

    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        Debug.Log("VR UI Button Clicked!");
        // Example action: Reset the score
        PlayerScoreManager.Score = 0f;
        GameManager._instance.StartGame();
    }


}