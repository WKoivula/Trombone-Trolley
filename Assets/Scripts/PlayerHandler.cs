using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;
    public float hitWindow = 0.1f;
    [Range(0.0f, 1.0f)]
    public float currentCursorPos = 0.0f;
    
    [Header("Debug Settings")]
    public bool enableMouseDebug = true;
    public float mouseDebugWindow = 2.0f; // Time window in seconds for mouse click debug (how long after clicking counts as a hit)
    
    public GameObject cursorPrefab;
    private GameObject cursorObject;

    private float laneValue;
    private bool noteShouldBeHit = false;
    private float lastMouseClickTime = -999f;

    private LineRenderer currentSliderLine;
    private GameObject currentNoteObject;
    private bool currentNoteWasHit = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cursorObject = Instantiate(cursorPrefab, transform);
        
        if (CartMovement.instance != null)
        {
            CartMovement.instance.ParentToBeatmapContainer(transform, true);
        }
    }

    private void Update()
    {
        cursorObject.transform.localPosition = new Vector3(0, currentCursorPos * 12 * 0.2f, 0);
        
        // Handle mouse click for debug mode (works with trackpad too)
        if (enableMouseDebug && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            lastMouseClickTime = Time.time;
        }
        
        bool micHit = MicInput.instance != null && MicInput.instance.CurrentVolume > -70f;
        bool mouseDebugHit = enableMouseDebug && (Time.time - lastMouseClickTime) < mouseDebugWindow;
        bool positionCorrect = currentCursorPos <= laneValue + hitWindow / 2 && currentCursorPos >= laneValue - hitWindow / 2;
        
        bool hitDetected = noteShouldBeHit && ((positionCorrect && micHit) || mouseDebugHit);
        
        if (hitDetected && !currentNoteWasHit)
        {
            if (currentSliderLine != null)
            {
                currentSliderLine.startColor = Color.blue;
                currentSliderLine.endColor = Color.blue;
            }
            
            currentNoteWasHit = true;
            
            if (CartMovement.instance != null)
            {
                CartMovement.instance.PushForward();
            }
        }
        else
        {
            if (currentSliderLine != null)
            {
                currentSliderLine.startColor = Color.white;
                currentSliderLine.endColor = Color.white;
            }
        }
    }

    public void SetNoteShouldBeHit(bool shouldBeHit)
    {
        noteShouldBeHit = shouldBeHit;
    }

    public void SetCurrentNote(float noteValue)
    {
        laneValue = noteValue;
    }

    public void SetCurrentLine(LineRenderer line)
    {
        currentSliderLine = line;
    }
    
    public void SetCurrentNoteObject(GameObject noteObject)
    {
        currentNoteObject = noteObject;
        currentNoteWasHit = false;
    }
    
    public bool WasCurrentNoteHit()
    {
        return currentNoteWasHit;
    }
}