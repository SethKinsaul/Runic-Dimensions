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
        if (other.CompareTag("Skeleton"))
        {
            Debug.Log("Fireball hit Skeleton!");

            // Get the skeleton's parent GameObject
            Transform enemyParent = other.transform.parent;
            if (enemyParent != null)
            {
                // Instead of Destroy, deactivate the skeleton
                enemyParent.gameObject.SetActive(false);

                // Play death sound
                AudioManager.Instance.PlayDeathSound();
            }
            else
            {
                // Fallback (in case no parent)
                other.gameObject.SetActive(false);
            }

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // Destroy the fireball
        }
        if (other.CompareTag("Spider"))
        {
            // Destroy the enemy's parent GameObject (which should be the full Skeleton)
            Transform enemyParent = other.transform.parent;
            if (enemyParent != null)
            {
                // Instead of Destroy, deactivate the skeleton
                enemyParent.gameObject.SetActive(false);
                // ?? Play death sound through AudioManager
                AudioManager.Instance.PlayDeathSound();
            }
            else
            {
                // Fallback (in case no parent)
                other.gameObject.SetActive(false);
            }
        }
    }
}
