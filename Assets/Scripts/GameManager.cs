using UnityEngine;
using Unity.Mathematics;
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public enum GameState { Start, GameOver, Playing }
    public GameState currentState;
    public float targetTime = 45.5f;
    public string playerName = "Player";
    public GameObject scoreboardOverlay;
    public GameObject woodSign;
    public bool scoreboardVisible = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        currentState = GameState.Start;
        woodSign.SetActive(false);
    }

    void Update()
    {
        if (targetTime > 0 && currentState == GameState.Playing)
        {
            targetTime -= Time.deltaTime;
        } else if (targetTime <= 0 && currentState == GameState.Playing && !scoreboardVisible)
        {
            scoreboardVisible = true;
            woodSign.SetActive(true);
        }
        else
        {
            
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
        scoreboardOverlay.SetActive(true);
        woodSign.SetActive(false);
    }

    public void ResetGame()
    {
        currentState = GameState.Start;
        woodSign.SetActive(true);
        scoreboardOverlay.SetActive(false);
    }
}
