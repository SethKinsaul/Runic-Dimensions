using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public Image[] lifeIcons; // Assign UI heart icons in Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameOverManager gameOverManager;
    public Transform[] checkpoints; // Assign in order of progression
    private Transform currentCheckpoint;
    private Vector3 originalSpawn;

    void Start()
    {
        currentLives = maxLives;
        originalSpawn = transform.position;
        currentCheckpoint = null;
        UpdateLifeUI();
    }

    public void TakeDamage()
    {
        currentLives--;
        AudioManager.Instance.PlayDeathSound();

        if (currentLives <= 0)
        {
            gameOverManager.TriggerGameOver();
        }
        else
        {
            Respawn();
        }

        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
            transform.position = currentCheckpoint.position;
        else
            transform.position = originalSpawn;
    }

    public void UpdateCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }
}
