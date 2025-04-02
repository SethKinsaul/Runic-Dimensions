using UnityEngine;

public class FireballSpell : MonoBehaviour
{
    public GameObject fireballPrefab; // The prefab for the fireball
    public Transform firePoint;       // The point where the fireball spawns
    public float fireballSpeed = 10f; // Speed of the fireball
    public KeyCode castKey = KeyCode.F; // Key to cast the spell

    void Update()
    {
        // Check if the specified key is pressed
        if (Input.GetKeyDown(castKey))
        {
            CastFireball();
        }
    }

    void CastFireball()
    {
        // Instantiate the fireball at the FirePoint's position and rotation
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // Apply velocity to make it move forward
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * fireballSpeed;
        }

        // Optional: Destroy the fireball after a certain time to avoid clutter
        Destroy(fireball, 5f);
    }
}
