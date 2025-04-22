using UnityEngine;

public class SpiderFireballProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 1;
    private PlayerLives playerLives;
    public Transform player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        playerLives = player.GetComponent<PlayerLives>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by fireball!");
            // Insert damage logic here
            playerLives.TakeDamage();
        }
    }
}
