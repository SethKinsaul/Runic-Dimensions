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

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

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
}
