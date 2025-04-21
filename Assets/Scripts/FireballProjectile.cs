using UnityEngine;
using UnityEngine.Audio;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 2f;
    public GameObject explosionEffect; // Optional VFX

    private Vector3 moveDirection;
    private AudioSource audioSource;

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifeTime);
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fireball hit enemy!");

        // Destroy the enemy's parent GameObject (which should be the full Skeleton)
        Transform enemyParent = other.transform.parent;
        if (enemyParent != null)
        {
            Destroy(enemyParent.gameObject);
            // ?? Play death sound through AudioManager
            AudioManager.Instance.PlayDeathSound();
        }
        else
        {
            // Fallback in case tag is on parent instead (rare, but safe check)
            Destroy(other.gameObject);
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Remove fireball on hit
    }
}
