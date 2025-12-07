using UnityEngine;

public class CartMovement : MonoBehaviour
{
    public static CartMovement instance;

    [Header("Movement Settings")]
    public float pushDistance = 2f;
    public float moveSpeed = 5f;

    [Header("Beatmap Parenting")]
    public GameObject beatmapContainer;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private bool isMoving = false;
    //public bara för att används i UI
    public float currentSpeed;
    public float newSpeed;
    //max och min speed:
    public float minSpeed = 0.01f;
    public float maxSpeed = 8f;

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
        
        transform.position += Vector3.left * Time.deltaTime * currentSpeed;
        if (isMoving)
        {
            ApplySpeedIncrease();
            /*  transform.position = Vector3.SmoothDamp(
                 transform.position, 
                 targetPosition, 
                 ref currentVelocity, 
                 1f / moveSpeed
             );

             if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
             {
                 transform.position = targetPosition;
                 isMoving = false;
             } */
        }

    }

    public void ApplySpeedIncrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * 1.3f, minSpeed, maxSpeed);
        currentSpeed = newSpeed;
        isMoving = false;

    }
    public void ApplySpeedDecrease()
    {
        newSpeed = Mathf.Clamp(currentSpeed * 0.95f, minSpeed, maxSpeed);
        currentSpeed = newSpeed;
        isMoving = false;

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
        isMoving = false;
    }

    public bool IsMoving()
    {
        return isMoving;
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


