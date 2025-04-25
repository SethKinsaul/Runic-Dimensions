using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Skeleton : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float moveSpeed = 2f;
    private float originalSpeed;
    public Transform spawnPoint;
    private Animator animator;
    public bool isChasing = false;

    // Freeze Variables
    private bool isFrozen = false;
    [Header("Freeze Visuals")]
    //public Renderer skeletonRenderer; // Assign in Inspector
    public Color frozenColor = Color.cyan;
    private Color originalColor;
    [SerializeField] private Renderer[] limbRenderers; // Assign in inspector

    private PlayerLives playerLives;

    //Audio Variables
    public AudioClip deathSound; // Assign in Inspector
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        playerLives = player.GetComponent<PlayerLives>();
        animator = GetComponent<Animator>();
        //animator.applyRootMotion = false;
    }

    private void Update()
    {
        if (player == null || isFrozen) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, 0).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            if (direction.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
            }

            if (animator != null)
            {
                animator.SetBool("Mini Simple Characters Armature|Walk", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Mini Simple Characters Armature|Walk", false);
            }
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
            animator.SetBool("Mini Simple Characters Armature|Walk", false);
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
        if (animator != null)
        {
            animator.SetBool("Mini Simple Characters Armature|Walk", false);
        }

        yield return new WaitForSeconds(duration);

        isFrozen = false;
        Debug.Log("Skeleton unfrozen.");
    }

    public void ResetState()
    {
        animator.Play("Mini Simple Armature|Idle");
        isChasing = false;
    }
}
