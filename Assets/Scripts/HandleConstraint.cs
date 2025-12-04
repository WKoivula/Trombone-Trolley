using UnityEngine;
using Oculus.Interaction; // Required for OneGrabTranslateTransformer

public class HandleConstraint : OneGrabTranslateTransformer, ITransformer
{
    [Header("Custom Constraint Settings")]
    public bool debugOn;
    public Transform startPoint;
    public Transform endPoint;
    public Transform parentObject;
    private Vector3 restLocalPos;
    private float computedMinX;
    private float computedMaxX;

    public float SliderValue;

    void Awake()
    {
        restLocalPos = transform.localPosition;
        //OneGrabTranslateConstraints constraints = OneGrabTranslateTransformer.OneGrabTranslateConstraints._constraints;
        var constraints = this.Constraints;
        if (startPoint != null && endPoint != null)
        {
            computedMinX = transform.localPosition.x - constraints.MinX.Value; //0.001f
            computedMaxX = transform.localPosition.x + constraints.MaxX.Value; //0.003f
        }
    }

    private bool isTransforming = false;

    public new void BeginTransform()
    {
        isTransforming = true;
        base.BeginTransform();
    }

    public new void EndTransform()
    {
        isTransforming = false;
        base.EndTransform();
        transform.localPosition = restLocalPos;
    }

    private void Update()
    {
        if (debugOn && startPoint != null && endPoint != null)
        {
            Debug.DrawLine(startPoint.position, endPoint.position, Color.green);
        }
        if (isTransforming)
        {
            //claaampa
            float clampedX = Mathf.Clamp(transform.localPosition.x, computedMinX, computedMaxX);
            transform.localPosition = new Vector3(clampedX, transform.localPosition.y, transform.localPosition.z);
            //invert lerp
            float normalized = Mathf.InverseLerp(computedMinX, computedMaxX, clampedX);

            SliderValue = normalized;
            PlayerHandler.instance.currentCursorPos = normalized;
        }

    }



}