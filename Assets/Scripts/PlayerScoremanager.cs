using UnityEngine;
using Unity.Mathematics;
public class PlayerScoremanager : MonoBehaviour
{
    public float Score;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    public float totalDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalDistance =0;
        Score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager._instance.currentState ==GameManager.GameState.Playing ) {
            
        totalDistance = Vector3.Distance(startPosition.position, endPosition.position);
        
        Score = Mathf.Round(totalDistance*100f)/100f; 
        }
    }
}
