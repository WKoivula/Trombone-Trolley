using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;


public class HandleConstraint : MonoBehaviour
{
    public bool debugOn;
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
        Debug.DrawRay(startPoint.position,endPoint.position-startPoint.position,Color.red,60f);

    }
    /*  */

    private void onGrab(Transform grabber)
    {
        currentGrabber = grabber;

    }
    private void exitGrab()
    {
        currentGrabber = null;
 
    }
    //move logic to update
    void Update()
    {
            if(debugOn) Debug.DrawRay(startPoint.position,endPoint.position-startPoint.position,Color.red,60f);
        while ( currentgrabber != null)
        {
            handleLocalStartPos
        }

    }
    public void OnGrabEntered(SelectEnterEventArgs args)
    {
        onGrab(args.interactorObject.transform);
        Vector3 handleLocalPos = transform.localPosition;
        Vector3 handLocalSpace = transform.InverseTransformPoint(args.interactorObject.transform.parent.position);
        
        handleLocalPos.x = Mathf.Lerp(handLocalSpace.x,startPoint.position.x,endPoint.position.x);
        handleLocalPos.y = (endPoint.position.y-startPoint.position.y);
        Debug.Log(args.interactableObject.transform.parent); // null
        Debug.Log(args.interactorObject.transform.parent); // hand
        Debug.Log(transform); //handle om bara transform 
    }

    public void OnGrabExited(SelectExitEventArgs args)
    {
        exitGrab();
        // Return to rest position in local space
        transform.localPosition = restLocalPosition;
        transform.localRotation = restRotation;
    }



}