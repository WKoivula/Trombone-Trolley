using UnityEngine;

public class CartMovement : MonoBehaviour
{
    public static CartMovement instance;

    [Header("Movement Settings")]
    public float pushDistance = 2f;
    public float moveSpeed = 5f;
    public int Amount = 0;

    [Header("Beatmap Parenting")]
    public GameObject beatmapContainer;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    //public bara för att används i UI
    public float currentSpeed;
    public float newSpeed;
    //max och min speed:
    public float minSpeed = 0.01f;
    public float maxSpeed = 12f;

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
            transform.position += Vector3.left * Time.deltaTime * currentSpeed;
         
        }
    }

    public void ApplySpeedIncrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * 1.6f, minSpeed, maxSpeed);
        currentSpeed = newSpeed;

    }
    public void ApplySpeedDecrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * 0.9f, minSpeed, maxSpeed);
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


