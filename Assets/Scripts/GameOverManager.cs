using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Transform player;
    public Transform spawnPoint;

    [System.Serializable]
    public class ResettableObject
    {
        public GameObject gameObject;
        [HideInInspector] public Vector3 originalPosition;
    }

    // Reset
    public ResettableObject[] powerUps;
    public ResettableObject[] skeletons;
    //public GameObject Notifications;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        // Cache original positions
        foreach (var pu in powerUps)
        {
            if (pu != null && pu.gameObject != null)
                pu.originalPosition = pu.gameObject.transform.position;
        }

        foreach (var skelly in skeletons)
        {
            if (skelly != null && skelly.gameObject != null)
                skelly.originalPosition = skelly.gameObject.transform.position;
        }
    }

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);

        // Reset player
        player.position = spawnPoint.position;
        player.GetComponent<PlayerLives>().UpdateCheckpoint(spawnPoint);

        // Reset powerups
        foreach (var pu in powerUps)
        {
            if (pu != null && pu.gameObject != null)
            {
                pu.gameObject.SetActive(true);
                pu.gameObject.transform.position = pu.originalPosition;
                var powerUpScript = pu.gameObject.GetComponent<PowerUp>();
                if (powerUpScript != null && powerUpScript.spellNotificationText != null)
                {
                    powerUpScript.spellNotificationText.gameObject.SetActive(false);
                }
            }
        }

        // Reset skeletons
        foreach (var skelly in skeletons)
        {
            if (skelly != null && skelly.gameObject != null)
            {
                skelly.gameObject.SetActive(true);
                skelly.gameObject.transform.position = skelly.originalPosition;

                // Optionally reset skeleton state
                var skeletonScript = skelly.gameObject.GetComponent<Skeleton>();
                if (skeletonScript != null)
                {
                    skeletonScript.ResetState(); // Create this method in Skeleton.cs
                }
            }
        }
        // Reset spells stored in dictionary
        var spellManager = player.GetComponent<SpellManager>();
        if (spellManager != null)
        {
            if (spellManager.activeSpells.ContainsKey("DimensionTwistingSpell"))
                spellManager.activeSpells["DimensionTwistingSpell"] = false;
            if (spellManager.activeSpells.ContainsKey("IceSpell"))
                spellManager.activeSpells["IceSpell"] = false;
            if (spellManager.activeSpells.ContainsKey("FireballSpell"))
                spellManager.activeSpells["FireballSpell"] = false;
        }
        // Reset lives
        var lives = player.GetComponent<PlayerLives>();
        if (lives != null)
        {
            lives.ResetLives();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
