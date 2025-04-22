using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public GameObject winTextUI; // Assign a UI Text or Panel with "You Win!" message

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the goal!");

            // Hide this game object
            gameObject.SetActive(false);

            // Show the win text
            if (winTextUI != null)
            {
                winTextUI.SetActive(true);
            }
        }
    }
}
