using TMPro;
using UnityEngine;
using System.Collections;  // Ensure IEnumerator is recognized

public class PowerUp : MonoBehaviour
{
    public string spellName;  // Assign a unique spell name in the Inspector
    public AudioClip pickupSound;  // Assign an audio clip in the Inspector
    public TextMeshProUGUI spellNotificationText; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other is BoxCollider)  // Ensure only the player triggers it
        {
            Debug.Log($"{spellName} Power-Up Collected!");
            SpellManager.Instance.ActivateSpell(spellName);  // Activate spell

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);  // Play sound at power-up location
            }
            else
            {
                Debug.LogWarning($"No audio clip assigned for {spellName} power-up!");
            }

            // Show notification if this is a spell that has one
            if ((spellName == "DimensionTwistingSpell" || spellName == "IceSpell" || spellName == "FireballSpell") && spellNotificationText != null)
            {
                ShowSpellNotification(spellName);
            }

            gameObject.SetActive(false);  // Remove the power-up
        }
    }

    void ShowSpellNotification(string spell)
    {
        Debug.Log("Text Activated");

        switch (spell)
        {
            case "DimensionTwistingSpell":
                spellNotificationText.text = "Press 'T' to activate Dimension Twisting Spell";
                break;
            case "IceSpell":
                spellNotificationText.text = "Press 'G' to cast Ice Spell";
                break;
            case "FireballSpell":
                spellNotificationText.text = "Press 'F' to cast Fireball Spell";
                break;
            default:
                Debug.Log("Spell picked up!");
                break;
        }

        spellNotificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(10f)); // Hide after 10 seconds
    }

    IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spellNotificationText.gameObject.SetActive(false);
    }
}
