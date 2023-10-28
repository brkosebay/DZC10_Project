using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController Instance; // Singleton instance
    public AudioClip introClip;
    public AudioClip loopClip;

    private AudioSource audioSource;
    private bool hasPlayedIntro = false;


    void Awake()
    {
        // Ensure there's only one instance of the BackgroundMusicController
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // Disable play on awake.

        // Play the intro clip once
        audioSource.clip = introClip;
        audioSource.Play();

    }


    void Update()
    {
        // Check if the intro clip has finished playing
        if (!hasPlayedIntro && !audioSource.isPlaying)
        {
            hasPlayedIntro = true;

            // Start playing the loop clip in a continuous loop
            audioSource.clip = loopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
