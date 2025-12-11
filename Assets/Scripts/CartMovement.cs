using UnityEngine;

public class CartMovement : MonoBehaviour
{
    public static CartMovement instance;



    [Header("Beatmap Parenting")]
    public GameObject beatmapContainer;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    //public bara för att används i UI
    public float currentSpeed;
    public float newSpeed;
    //max och min speed:
    [Header("Speed Settings")]
    public float minSpeed = 1f;
    public float maxSpeed = 12f;
    public float minSpeedFactor = 0.9f;
    public float maxSpeedFactor = 1.3f;
    float endgameSpeed = 0f;
    float slowdown = 0f;

    private void Awake()
    {
        instance = this;
        targetPosition = transform.position;
        newSpeed = currentSpeed;
        if (beatmapContainer == null)
        {
            beatmapContainer = new GameObject("BeatmapContainer");
        }

        ParentToCart(beatmapContainer.transform, true);
    }
    //lyssnar till missnotehandler on trigger
    private void OnEnable()
    {
        MissedNoteHandler.NoteMissed += OnNoteMissed;
    }

    private void OnDisable()
    {
        MissedNoteHandler.NoteMissed -= OnNoteMissed;
    }

    private void OnNoteMissed(Collider other)
    {
        Debug.Log($"CartMovement: OnNoteMissed received for {other.gameObject.name}");
        ApplySpeedDecrease();
    }

    private void Update()
    {
        // Only move the cart when the game is in Playing state
        if (GameManager._instance != null && GameManager._instance.currentState == GameManager.GameState.Playing)
        {
            if (GameManager._instance.targetTime <= 0f)
            {
                transform.position += Vector3.left * Time.deltaTime * endgameSpeed;
                endgameSpeed = Mathf.Clamp(endgameSpeed - (Time.deltaTime * slowdown), 0f, maxSpeed);
            }
            else
            {
                endgameSpeed = currentSpeed;
                slowdown = currentSpeed / 1.5f;
                transform.position += Vector3.left * Time.deltaTime * currentSpeed;
            }
        }
    }

    public void ApplySpeedIncrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * maxSpeedFactor, minSpeed, maxSpeed);
        currentSpeed = newSpeed;

    }
    public void ApplySpeedDecrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * minSpeedFactor, minSpeed, maxSpeed);
        currentSpeed = newSpeed;

    }


    public void PushForward()
    {
        ApplySpeedIncrease();
    }
    public void PushBackward()
    {
        ApplySpeedDecrease();
    }



    public void SetPosition(Vector3 position)
    {
        targetPosition = position;
        transform.position = position;
    }


    public void ParentToCart(Transform obj, bool preserveWorldPosition = true)
    {
        if (obj == null) return;

        Vector3 worldPos = obj.position;
        Quaternion worldRot = obj.rotation;
        Vector3 worldScale = obj.lossyScale;

        obj.SetParent(transform, preserveWorldPosition);
    }

    public void ParentToBeatmapContainer(Transform obj, bool preserveWorldPosition = true)
    {
        if (obj == null || beatmapContainer == null) return;
        obj.SetParent(beatmapContainer.transform, preserveWorldPosition);
    }

    public Transform GetBeatmapContainer()
    {
        return beatmapContainer != null ? beatmapContainer.transform : transform;
    }
}


