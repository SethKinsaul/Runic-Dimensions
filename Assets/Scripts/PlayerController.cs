using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    // Movement variables
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public float gravityMultiplier = 9.8f;  // Gravity scaling

    // Ground detection
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    // Perspective tracking
    private bool isIn3DMode = false; // Tracks if the player is in 3D mode

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents falling over
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyGravity();
        MovePlayer();
    }

    void Update()
    {
        HandleJump();
        Debug.Log("PlayerController is running...");
        // Check if the player has fallen below y = -10
        if (transform.position.y < -10)
        {
            ResetPlayerPosition();
        }
    }
    void ResetPlayerPosition()
    {
        transform.position = new Vector3(-3f, 0f, 0f); // Reset position
        rb.linearVelocity = Vector3.zero; // Reset velocity to stop unwanted movement
    }

    void MovePlayer()
    {
        // Get the camera follow script and check if we are in 3D mode
        isIn3DMode = FindFirstObjectByType<CameraFollow>().IsIn3DMode();

        float moveX = 0f;
        float moveZ = 0f;
        Vector3 move;

        if (isIn3DMode)
        {
            // 3D Mode: A/D moves along Z-axis, W/S moves along X-axis
            moveZ = Input.GetAxis("Horizontal");
            moveX = Input.GetAxis("Vertical");
        }
        else
        {
            // 2D Mode: A/D moves along X-axis, W/S does nothing
            moveX = Input.GetAxis("Horizontal");
        }

        move = new Vector3(moveX * speed, rb.linearVelocity.y, -moveZ * speed);
        rb.linearVelocity = move;
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    void CheckGround()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 bottomCenter = new Vector3(
                boxCollider.bounds.center.x,
                boxCollider.bounds.min.y,
                boxCollider.bounds.center.z
            );

            isGrounded = Physics.CheckSphere(bottomCenter - Vector3.up * 0.1f, 0.2f, groundMask);
            Debug.DrawRay(bottomCenter, Vector3.down * 0.2f, isGrounded ? Color.green : Color.red);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 bottomCenter = new Vector3(
                boxCollider.bounds.center.x,
                boxCollider.bounds.min.y,
                boxCollider.bounds.center.z
            );

            Gizmos.DrawWireSphere(bottomCenter - Vector3.up * 0.1f, 0.2f);
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // Function to switch between 2D and 3D mode
    public void Set3DMode(bool enabled)
    {
        isIn3DMode = enabled;

        if (isIn3DMode)
        {
            // Unlock Z movement
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            // Freeze Z and reset position for 2D mode
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
