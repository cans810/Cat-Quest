public class Climbable : MonoBehaviour
{
    public float climbingSpeed = 4f;
    public float liftOffset = 0.1f;
    
    private bool isPlayerInRange = false;
    private Rigidbody2D playerRigidbody;
    private Cat catScript;
    private float ladderXPosition;
    private Vector3 originalPlayerScale;

    void Start()
    {
        ladderXPosition = transform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();
            catScript = collision.GetComponent<Cat>();
            originalPlayerScale = collision.transform.localScale;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (catScript != null && catScript.isClimbing)
            {
                catScript.isClimbing = false;
            }

            playerRigidbody = null;
            catScript = null;
        }
    }
}