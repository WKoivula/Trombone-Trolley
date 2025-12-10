using UnityEngine;
using Unity.Mathematics;
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public enum GameState { Start, GameOver, Playing }
    public GameState currentState;
    
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
