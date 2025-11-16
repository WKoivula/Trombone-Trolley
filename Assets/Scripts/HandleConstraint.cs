using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.Mathematics;


public class HandleConstraint : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform parentObject; // Add reference to parent
    private Vector3 restLocalPosition;
    private quaternion restRotation;
    private Vector3 movementDistance;
    private bool isGrabbed = false;
    public InputAction FiringValue;

    void Start()
    {
        restLocalPosition = transform.localPosition;
        restRotation = transform.localRotation;
        movementDistance = endPoint.position - startPoint.position;
        Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), Color.red);
        if (parentObject == null)
        {
            parentObject = transform.parent;
        }
    }
    void OnEnable()
    {
        FiringValue.Enable();
    }

    void OnDisable()
    {
        FiringValue.Disable();
    }


    public void OnGrabEntered(SelectEnterEventArgs args)
    {
        Transform hand = args.interactorObject.transform;
        Transform handle = args.interactableObject.transform;
        Vector3 handleLocalPos = handle.localPosition;        
        handleLocalPos.x = Mathf.Clamp(hand.position.x,startPoint.localPosition.x,endPoint.localPosition.x);

    }
    
    public void OnGrabExited(SelectExitEventArgs args)
    {
        // Return to rest position in local space
        transform.localPosition = restLocalPosition;
        transform.localRotation = restRotation;
    }
    
    

}