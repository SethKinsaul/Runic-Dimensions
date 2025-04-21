using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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

    // Ice Spell Variables
    public GameObject iceProjectilePrefab;
    public Transform projectileSpawnPoint; // empty GameObject in front of player
    public float iceCooldown = 1.5f;
    private float lastIceCastTime = -Mathf.Infinity;

    // Fire Spell Variables
    public GameObject fireballPrefab;

    // Perspective tracking
    private bool isIn3DMode = false; // Tracks if the player is in 3D mode

    // Animator
    private Animator animator;
    public float spellCastDelay = 0.5f;  // Delay in seconds before the sphere is cast (adjust as needed)

    //Sounds
    public AudioClip FireSound;  // Assign an audio clip in the Inspector
    float shortPlayTime = 3f; // How many seconds to play
    private PlayerLives playerLives;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents falling over
        animator = GetComponent<Animator>();
        playerLives = GetComponent<PlayerLives>();
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
        if (Input.GetKeyDown(KeyCode.G) && SpellManager.Instance.IsSpellActive("IceSpell"))
        {
            if (Time.time >= lastIceCastTime + iceCooldown)
            {
                animator.Play("Idle01");
                animator.SetTrigger("CastSpell");  // Trigger animation
                // Start the coroutine to spawn the projectile with a delay
                StartCoroutine(ActivateIceSpell());
                lastIceCastTime = Time.time;
            }
            else
            {
                Debug.Log("Ice spell on cooldown!");
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && SpellManager.Instance.IsSpellActive("FireballSpell"))
        {
            if (Time.time >= lastIceCastTime + iceCooldown)
            {
                animator.Play("Idle01");
                animator.SetTrigger("CastSpell");  // Trigger animation
                                                   // Start the coroutine to spawn the projectile with a delay
                StartCoroutine(CastFireball());
                lastIceCastTime = Time.time;
            }
            else
            {
                Debug.Log("Fire spell on cooldown!");
            }
        }
        // Check if the player has fallen below y = -10
        if (transform.position.y < -10)
        {
            playerLives.TakeDamage();
            //ResetPlayerPosition();
        }
    }
    private IEnumerator ActivateIceSpell()
    {
        yield return new WaitForSeconds(spellCastDelay);  // Wait for the delay duration
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
        if (iceProjectilePrefab != null && projectileSpawnPoint != null)
        {
            AudioManager.Instance.PlayIceSound();
            Debug.Log("Ice Spell Cast!");
            GameObject projectile = Instantiate(iceProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 shootDirection = transform.forward; // Forward in world space
            projectile.GetComponent<IceProjectile>().Initialize(shootDirection);
        }
        else
        {
            Debug.LogWarning("IceProjectilePrefab or SpawnPoint not assigned.");
        }
    }
    private IEnumerator CastFireball()
    {
        yield return new WaitForSeconds(spellCastDelay);  // Wait for the delay duration
        if (fireballPrefab != null && projectileSpawnPoint != null)
        {
            AudioManager.Instance.PlayFireSound();
            Debug.Log("Fireball Spell Cast!");
            GameObject fireball = Instantiate(fireballPrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 shootDirection = transform.forward;
            fireball.GetComponent<FireballProjectile>().Initialize(shootDirection);
        }
        else
        {
            Debug.LogWarning("FireballPrefab or SpawnPoint not assigned.");
        }
    }
    void PlayShortenedFireSound()
    {
        if (FireSound != null)
        {
            GameObject tempAudioGO = new GameObject("TempFireSound");
            tempAudioGO.transform.position = transform.position;

            AudioSource audioSource = tempAudioGO.AddComponent<AudioSource>();
            audioSource.clip = FireSound;
            audioSource.Play();

            StartCoroutine(StopAudioAfterDelay(audioSource, tempAudioGO, shortPlayTime));
        }
    }

    IEnumerator StopAudioAfterDelay(AudioSource source, GameObject objToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop(); // Stops the sound early
        Destroy(objToDestroy); // Clean up temp GameObject
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
        if (moveX > 0)
        {
            // Moving right
            transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Adjust if facing another way
        }
        else if (moveX < 0)
        {
            // Moving left
            transform.rotation = Quaternion.Euler(0f, -90f, 0f); // Flip direction
        }
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
