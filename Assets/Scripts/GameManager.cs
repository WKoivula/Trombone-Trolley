using UnityEngine;
using Unity.Mathematics;
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public enum GameState { Start, GameOver, Playing }
    public GameState currentState;
    public float targetTime = 5f;
    public string playerName = "Player";

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
        
    }

    void Update()
    {
        if (targetTime > 0 && currentState == GameState.Playing)
        {
            targetTime -= Time.deltaTime;
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
    }
    
    public void EndGame()
    {
        currentState = GameState.GameOver;
    }
    
    public void ResetGame()
    {
        currentState = GameState.Start;
    }
}
