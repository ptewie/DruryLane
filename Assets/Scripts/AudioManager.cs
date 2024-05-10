using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------------- AUDIO SOURCES -------------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("-------------- AUDIO CLIPS -------------------")]
    public AudioClip bgm;
    public AudioClip death;
    public AudioClip fail;
    public AudioClip select;

    private float initialPitch; // Store the initial pitch of the music

    private void Awake()
    {
        // Set the background music clip
        musicSource.clip = bgm;
        // Store the initial pitch of the music
        initialPitch = musicSource.pitch;
    }

    public void PlayBGM()
    {
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);

    }

    // Function to increase the pitch of the background music
    public void IncreasePitch(float amount)
    {
        // Increase the pitch by the specified amount
        musicSource.pitch += amount;
    }

    // Function to reset the pitch of the background music
    public void ResetPitch()
    {
        // Reset the pitch to its initial value
        musicSource.pitch = initialPitch;
    }
}
