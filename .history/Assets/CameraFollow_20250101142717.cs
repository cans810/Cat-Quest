using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.1f;
    public float groundedYOffset = 1f;
    private Vector3 velocity = Vector3.zero;
    private float minY;

    private void Start()
    {
        minY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Cat playerScript = target.GetComponent<Cat>();
            float yPosition;

            if (playerScript && playerScript.IsGrounded())
            {
                yPosition = target.position.y + groundedYOffset;
                
                
                
            }
            
            else
            {
                yPosition = target.position.y;
                
                

                
                
            }
             
            if (yPosition <= minY){
                yPosition = Mathf.Max(yPosition, minY);
            }
            
            
            
           

            Vector3 desiredPosition = new Vector3(target.position.x, yPosition, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }
}