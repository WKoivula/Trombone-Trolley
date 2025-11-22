using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;


public class HandleConstraint : MonoBehaviour
{
    public bool debugOn;
    public BoxCollider interactionZone;
    public Transform startPoint;
    public Transform endPoint;
    public Transform parentObject; // Add reference to parent

    private Vector3 restLocalPosition;
    private quaternion restRotation;
    private Vector3 movementDistance;
    public bool isDebug = false;
    public InputAction FiringValue;

    public Transform hand; 
    Transform currentGrabber;
    Transform handleTransform;
    private Vector3 handleTransform_local;
    Vector3 handleLocalStartPos;
    private bool isGrabbed;
    void Start()
    {
        restLocalPosition = transform.localPosition;
        handleTransform = transform;
        handleTransform_local = transform.localPosition;
        restRotation = transform.localRotation;
        Debug.DrawRay(startPoint.position, endPoint.position - startPoint.position, Color.red, 60f);
        if (interactionZone == null)
        {
            Debug.Log("No box collider was added, did u forget?");
        }
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
        if (debugOn) Debug.DrawRay(startPoint.position, endPoint.position - startPoint.position, Color.red, 60f);
        if (currentGrabber != null)
        {



        }
//* Time.deltaTime* 0.01f
        float t = Mathf.Sin(-hand.localPosition.z * 0.01f );
        Vector3 dir = (endPoint.position - startPoint.position).normalized;
        transform.localPosition = new Vector3(t, 0, 0);

    }

















    public void OnGrabEntered(SelectEnterEventArgs args)
    {
        onGrab(args.interactorObject.transform);

    }

    public void OnGrabExited(SelectExitEventArgs args)
    {
        exitGrab();
        // Return to rest position in local space
        transform.localPosition = restLocalPosition;
        transform.localRotation = restRotation;
    }



}