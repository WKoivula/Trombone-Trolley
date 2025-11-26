using UnityEngine;
using Oculus.Interaction; // Required for OneGrabTranslateTransformer

public class HandleConstraint : OneGrabTranslateTransformer,ITransformer
{
    [Header("Custom Constraint Settings")]
    public bool debugOn;
    public Transform startPoint;
    public Transform endPoint;
    public Transform parentObject;
    private Vector3 restLocalPos;
    private float computedMinX;
    private float computedMaxX;

    // We use Start to calculate and inject the constraints
    // explicitly because we cannot override the base logic.
    /*     protected virtual void Start()
        {
            restLocalPos = transform.localPosition;
            if (startPoint == null || endPoint == null)
            {
                Debug.LogError("StartPoint and EndPoint must be assigned.");
                return;
            }



            // 2. Calculate Local Positions
            // We need to know where the start and end points are relative to the parent
            float startX = -transform.InverseTransformPoint(startPoint.position).x*50f;
            float endX = -transform.InverseTransformPoint(endPoint.position).x*50f;

            // Determine which is min and max
            computedMinX = Mathf.Min(startX, endX);
            computedMaxX = Mathf.Max(startX, endX);


            // 3. Create the Constraints Object
            // Note: OneGrabTranslateConstraints is a nested class inside OneGrabTranslateTransformer
            var newConstraints = new OneGrabTranslateConstraints();

            // --- Configure X Axis (Movement) ---
            newConstraints.MinX = new FloatConstraint();
            newConstraints.MaxX = new FloatConstraint();

            newConstraints.MinX.Constrain = true;
            newConstraints.MinX.Value = -0.002f;

            newConstraints.MaxX.Constrain = true;
            newConstraints.MaxX.Value = 0.0002f;

            // --- Configure Y and Z Axis (Locked) ---
            // We want to lock Y and Z to the object's current local position
            float lockedY = transform.localPosition.y;
            float lockedZ = transform.localPosition.z;

            newConstraints.MinY = new FloatConstraint();
            newConstraints.MaxY = new FloatConstraint();
            newConstraints.MinZ = new FloatConstraint();
            newConstraints.MaxZ = new FloatConstraint();

            newConstraints.MinY.Constrain = true;
            newConstraints.MinY.Value = lockedY;
            newConstraints.MaxY.Constrain = true;
            newConstraints.MaxY.Value = lockedY;

            newConstraints.MinZ.Constrain = true;
            newConstraints.MinZ.Value = lockedZ;
            newConstraints.MaxZ.Constrain = true;
            newConstraints.MaxZ.Value = lockedZ;

            newConstraints.ConstraintsAreRelative = true;

            this.InjectOptionalConstraints(newConstraints);
        } */

    void Awake()
    {
                    restLocalPos = transform.localPosition;

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
        //float x_pos = Mathf.Clamp(transform.localPosition.x,-2,2);
        //transform.localPosition = new Vector3(x_pos,transform.localPosition.y,transform.localPosition.z);
            
        }
    } 


    
}