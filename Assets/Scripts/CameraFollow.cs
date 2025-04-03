using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    private Vector3 offset;
    SpellManager spellManager;

    public Camera mainCamera;
    public Camera secondaryCamera;

    private bool isIn3DMode = false; // Tracks whether we are in 3D mode

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
        }
        spellManager = FindAnyObjectByType<SpellManager>(); // Works in Unity 2023+
        // Ensure the main camera starts active
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition;

            if (!isIn3DMode) // 2D mode (Only follow X-axis)
            {
                targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
            else // 3D mode (Follow X, Y, and Z axes)
            {
                targetPosition = player.position + offset; // Keep original offset
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        //SpellManager spellManager = FindAnyObjectByType<SpellManager>();
        // Toggle camera with 'T'
        if (Input.GetKeyDown(KeyCode.T) && spellManager != null && spellManager.IsSpellActive("DimensionTwistingSpell"))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        isIn3DMode = !isIn3DMode;

        mainCamera.enabled = !isIn3DMode;
        secondaryCamera.enabled = isIn3DMode;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (isIn3DMode)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation; // Allow full movement in 3D
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ; // Restrict Z movement
                player.position = new Vector3(player.position.x, player.position.y, 0); // Reset Z position
            }
        }
    }
    public bool IsIn3DMode()
    {
        return isIn3DMode;
    }
}
