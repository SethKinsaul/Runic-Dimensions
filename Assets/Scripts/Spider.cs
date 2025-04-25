using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public GameObject fireProjectilePrefab;  // Assign your fire projectile prefab in the Inspector
    public Transform firePoint;              // Assign a child transform where the projectile spawns
    public float fireInterval = 3f;          // Time between shots
    public float shootRangeX = 15f;          // X-axis range for shooting
    public float projectileSpeed = 10f;      // Speed of the projectile

    private Transform player;
    private float fireTimer;

    [HideInInspector]
    public Vector3 originalPosition;

    // Freeze Variables
    private bool isFrozen = false;
    [Header("Freeze Visuals")]
    public Color frozenColor = Color.cyan;
    private Color originalColor;
    [SerializeField] private Renderer[] limbRenderers; // Assign in inspector

    public Transform spawnPoint;
    private PlayerLives playerLives;
    public float moveSpeed = 2f;
    private float originalSpeed;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        originalPosition = transform.position;
        if (playerObj != null)
            player = playerObj.transform;
        playerLives = player.GetComponent<PlayerLives>();
    }

    void Update()
    {
        if (player == null || isFrozen) return;

        float xDistance = Mathf.Abs(transform.position.x - player.position.x);
        if (xDistance <= shootRangeX)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                ShootAtPlayer();
                fireTimer = 0f;
            }
        }
    }

    void ShootAtPlayer()
    {
        if (fireProjectilePrefab == null || firePoint == null || player == null) return;

        GameObject projectile = Instantiate(fireProjectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.position - firePoint.position).normalized;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player died!");

            if (spawnPoint != null)
            {
                playerLives.TakeDamage();
                //player.position = spawnPoint.position;
            }
        }
    }

    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            originalSpeed = moveSpeed;
            moveSpeed = 0f;

            // Change material color or swap material on all child renderers
            SetFrozenColor(true);
            StartCoroutine(UnfreezeAfterDelay(duration));
        }
    }

    private void SetFrozenColor(bool frozen)
    {
        Color freezeColor = frozen ? Color.cyan : Color.white;

        foreach (Renderer rend in limbRenderers)
        {
            if (rend != null)
            {
                foreach (var mat in rend.materials)
                {
                    mat.color = freezeColor;
                }
            }
        }
    }
    private IEnumerator UnfreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveSpeed = originalSpeed;
        isFrozen = false;

        // Restore material color
        SetFrozenColor(false);
    }

    private System.Collections.IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;
        Debug.Log("Skeleton frozen!");

        // Optional: Stop walk animation while frozen
        //if (animator != null)
        //{
        //    animator.SetBool("Mini Simple Characters Armature|Walk", false);
        //}

        yield return new WaitForSeconds(duration);

        isFrozen = false;
        Debug.Log("Skeleton unfrozen.");
    }
}
