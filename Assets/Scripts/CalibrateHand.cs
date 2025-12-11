using UnityEngine;

public class CalibrateHand : MonoBehaviour
{
    [SerializeField]
    private Transform CalibrationPoint;

    [SerializeField]
    public Transform TromboneTransform;
    [SerializeField]
    public Transform RightHandTransform;
    [SerializeField]
    public Transform Head;

    private Vector3 HeadToHand;
    private Vector3 HeadToHandDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RightHandTransform != null && Head != null)
        {
            HeadToHand = RightHandTransform.position - Head.position;
        }
    }
    //offset rotation by 90 throguh quaternion
    // Update is called once per frame
    void Update()
    {
        if (RightHandTransform == null || Head == null)
        {
            return;
        }

        HeadToHand = RightHandTransform.position - Head.position;
        HeadToHandDir = HeadToHand.normalized;
        Debug.DrawRay(Head.position, HeadToHand, Color.green);
        if (Input.GetKey("c")){
            CalibrationPoint.position = RightHandTransform.position;
        }
       /*  if (TromboneTransform != null && HeadToHandDir.sqrMagnitude > 0.0001f)
        {
            TromboneTransform.LookAt(Head.position + HeadToHandDir);
        } */
    }
}
