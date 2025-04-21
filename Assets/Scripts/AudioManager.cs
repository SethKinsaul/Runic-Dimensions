using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;    // For background music
    public AudioSource deathSource;    // For player death sound
    public AudioSource fireSource;    // For player fire sound
    public AudioSource iceSource;    // For player ice sound
    public AudioSource dimensionTwistingSource;    // For player dimension twisting sound
    public AudioSource freezingSource;    // For player freezing sound

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathSound()
    {
        if (deathSource != null && !deathSource.isPlaying)
        {
            deathSource.Play();
        }
    }
    public void PlayFireSound()
    {
        if (fireSource != null)
        {
            fireSource.Play();
        }
    }
    public void PlayIceSound()
    {
        if (iceSource != null)
        {
            iceSource.Play();
        }
    }
    public void PlayDimensionTwistingSound()
    {
        if (dimensionTwistingSource != null)
        {
            dimensionTwistingSource.Play();
        }
    }
    public void PlayWaterFreezingSound()
    {
        if (freezingSource != null)
        {
            freezingSource.Play();
        }
    }
}
