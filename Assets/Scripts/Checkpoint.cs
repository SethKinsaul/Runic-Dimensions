using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            other.GetComponent<PlayerLives>().UpdateCheckpoint(transform);
            triggered = true;
        }
    }
}
