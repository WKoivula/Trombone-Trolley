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
    
    private void Awake()
    {
        instance = this;
        targetPosition = transform.position;
        
        if (beatmapContainer == null)
        {
            beatmapContainer = new GameObject("BeatmapContainer");
        }
        
        ParentToCart(beatmapContainer.transform, true);
    }
    
    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                targetPosition, 
                ref currentVelocity, 
                1f / moveSpeed
            );
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
    
    public void PushForward()
    {
        PushForward(pushDistance);
    }
    
    public void PushForward(float distance)
    {
        targetPosition += -transform.forward * distance;
        isMoving = true;
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


