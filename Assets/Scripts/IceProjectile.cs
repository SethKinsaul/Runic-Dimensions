using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float fallSpeed = 2f;
    public float lifeTime = 5f;

    private Vector3 moveDirection;
    public GameObject hitEffectPrefab; // Optional: effect on hit
    [SerializeField] private Transform colliderTransform; // Assign in Inspector
    public Material iceMaterial; // Assign in Inspector

    public void Initialize(Vector3 direction)
    {
        moveDirection = direction;
        Destroy(gameObject, lifeTime); // Destroy after a while
    }

    void Update()
    {
        // Simulate forward motion + falling
        Vector3 fallVector = Vector3.down * fallSpeed * Time.deltaTime;
        transform.position += (moveDirection * speed * Time.deltaTime) + fallVector;

        // Manually move the collider if needed
        if (colliderTransform != null)
        {
            colliderTransform.position = transform.position; // Match position exactly
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Skeleton"))
        {
            Debug.Log("Ice hit Skeleton!");
            AudioManager.Instance.PlayWaterFreezingSound();
            // Freeze the enemy if it has the component
            Skeleton skeleton = other.GetComponentInParent<Skeleton>();
            if (skeleton != null)
            {
                skeleton.Freeze(3f); // Freeze for 3 seconds
            }
        }
        else if (other.CompareTag("Spider"))
        {
            Debug.Log("Ice hit Spider!");
            AudioManager.Instance.PlayWaterFreezingSound();
            // Freeze the enemy if it has the component
            Spider spider = other.GetComponentInParent<Spider>();
            if (spider != null)
            {
                spider.Freeze(3f); // Freeze for 3 seconds
            }
        }
        else if (other.CompareTag("Water"))
        {
            Debug.Log("Ice hit water!");
            AudioManager.Instance.PlayWaterFreezingSound();
            // Try to get the collider and disable IsTrigger
            Collider col = other.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = false;
                Debug.Log("Water is now solid (isTrigger = false)");
            }

            // Change the material to ice
            Renderer rend = other.GetComponent<Renderer>();
            if (rend != null && iceMaterial != null)
            {
                rend.material = iceMaterial;
            }

            // Ensure it has a collider to be walked on
            Collider waterCollider = other.GetComponent<Collider>();
            if (waterCollider == null)
            {
                waterCollider = other.gameObject.AddComponent<BoxCollider>(); // Fallback
            }

            // Optional: set the layer to "Ground" if you use layer-based movement checks
            other.gameObject.layer = LayerMask.NameToLayer("Ground");
        }

        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the projectile after impact
    }
}