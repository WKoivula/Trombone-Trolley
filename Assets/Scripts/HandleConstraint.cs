using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;


public class HandleConstraint : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform parentObject; // Add reference to parent

    private Vector3 restLocalPosition;
    private quaternion restRotation;
    private Vector3 movementDistance;
    public bool isDebug = false;
    public InputAction FiringValue;
    Transform currentGrabber;
    Transform handleTransform;
    Vector3 handleLocalStartPos;

    void Start()
    {
        restLocalPosition = transform.localPosition;
        handleTransform = transform;
        restRotation = transform.localRotation;
        movementDistance = endPoint.position - startPoint.position;
        //Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), Color.red);

    }
    /*  */

    private float onGrab(Transform grabber)
    {
        currentGrabber = grabber;
        handleLocalStartPos = handleTransform.localPosition;
        Vector3 handLocalSpace = handleTransform.parent.InverseTransformPoint(grabber.position);
        return handLocalSpace.x;
    }
    private void exitGrab()
    {
        currentGrabber = null;
 
    }

    public void OnGrabEntered(SelectEnterEventArgs args)
    {
        Debug.Log(args.interactableObject.transform.parent); // null
        Debug.Log(args.interactorObject.transform.parent); //right hand
        Debug.Log(transform); //handle om bara transform
        
        /* handleLocalStartPos = handleTransform.localPosition;
        Transform hand = args.interactableObject.transform;
        Vector3 handLocalSpace = handleTransform.parent.InverseTransformPoint(hand.position);
        handleTransform = transform;
        Vector3 handlepos = handleTransform.localPosition;
        handlepos.x = handLocalSpace.x; */

       /*  Transform hand = args.interactorObject.transform;
        float input = onGrab(hand);
        handleTransform = args.interactableObject.transform;
        float handle_local_x = handleTransform.transform.localPosition.x;
        handle_local_x = input; */
        
        //Vector3 handloc = args.interactorObject.transform.position;
        //Transform handle = args.interactableObject.transform;

       // Vector3 handleLocalPos = handle.localPosition;
        //handleLocalPos.x = Mathf.Clamp(hand.position.x, startPoint.localPosition.x, endPoint.localPosition.x);

    }

    public void OnGrabExited(SelectExitEventArgs args)
    {
        exitGrab();
        // Return to rest position in local space
        transform.localPosition = restLocalPosition;
        transform.localRotation = restRotation;
    }



}