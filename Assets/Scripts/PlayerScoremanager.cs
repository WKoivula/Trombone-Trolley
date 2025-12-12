using UnityEngine;
using Unity.Mathematics;
public class PlayerScoremanager : MonoBehaviour
{
    public float Score;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    public float totalDistance;
    public string[] highscoreNames = new string[5];
    public string[] highscoreNamesKeys = { "HighscoreName1", "HighscoreName2", "HighscoreName3", "HighscoreName4", "HighscoreName5" };
    public string[] highscoresKeys = { "Highscore1", "Highscore2", "Highscore3", "Highscore4", "Highscore5" };
    public float[] highscores = new float[5];
    bool scoreUpdated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalDistance =0;
        Score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager._instance.currentState ==GameManager.GameState.Playing) {
            if (GameManager._instance.targetTime > 0)
            {
                totalDistance = Vector3.Distance(startPosition.position, endPosition.position);
                Score = Mathf.Round(totalDistance*100f)/100f; 
            } else if (!scoreUpdated)
            {
                scoreUpdated = true;
                PlayerPrefs.SetString("LastPlayerName", GameManager._instance.playerName);
                PlayerPrefs.SetFloat("LastPlayerScore", Score);
                for (int i = 0; i < highscoreNames.Length; i++)
                {
                    highscoreNames[i] = PlayerPrefs.GetString(highscoreNamesKeys[i]);
                    highscores[i] = PlayerPrefs.GetFloat(highscoresKeys[i]);
                }
                string lowestName = GameManager._instance.playerName;
                float lowestScore = Score;

                for (int i = 0; i < highscores.Length; i++)
                {
                    if (Score > highscores[i])
                    {
                        // Swap the scores and names
                        float tempScore = highscores[i];
                        string tempName = highscoreNames[i];
                        highscores[i] = lowestScore;
                        highscoreNames[i] = lowestName;
                        lowestScore = tempScore;
                        lowestName = tempName;
                    }
                }
                for (int i = 0; i < highscores.Length; i++)
                {
                    PlayerPrefs.SetString(highscoreNamesKeys[i], highscoreNames[i]);
                    PlayerPrefs.SetFloat(highscoresKeys[i], highscores[i]);
                }
                for (int i = 0; i < highscores.Length; i++)
                {
                    Debug.Log($"Highscore {i + 1}: {highscoreNames[i]} - {highscores[i]}");
                }
            }
        
        }
    }
}
