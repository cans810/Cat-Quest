using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
    public Transform target;
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;
    [HideInInspector]
    public Vector3 minValues, maxValue;


    //Editors Fields
    [HideInInspector]
    public bool setupComplete = false;
    public enum SetupState { None,Step1,Step2}
    [HideInInspector]
    public SetupState ss = SetupState.None;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;

        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z));

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

    public void ResetValues()
    {
        setupComplete = false;
        minValues = Vector3.zero;
        maxValue = Vector3.zero;
    }
}