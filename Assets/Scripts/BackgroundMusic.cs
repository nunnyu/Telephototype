using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;
    private AudioClip[] tracks;
    private int currentTrackIndex = 0;

    public AudioSource source;
    public float fadeSpeed = 1f;

    private enum AudioState { Playing, FadingOut, FadingIn }
    private AudioState state = AudioState.Playing;

    private bool swapRequested = false;
    private float ogVolume;

    void Start()
    {
        tracks = new AudioClip[] { bgm1, bgm2, bgm3 };
        currentTrackIndex = 0;
        source = GetComponent<AudioSource>();
        source.clip = tracks[currentTrackIndex];
        source.Play();
        ogVolume = source.volume;

    }

    public void Swap()
    {
        if (state == AudioState.Playing)
        {
            swapRequested = true;
        }
    }

    void Update()
    {
        switch (state)
        {
            case AudioState.Playing:
                if (swapRequested)
                {
                    state = AudioState.FadingOut;
                    swapRequested = false;
                }
                break;

            case AudioState.FadingOut:
                source.volume = Mathf.MoveTowards(source.volume, 0f, Time.deltaTime * fadeSpeed);
                if (source.volume <= 0.01f)
                {
                    currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;
                    source.clip = tracks[currentTrackIndex];
                    source.Play();
                    state = AudioState.FadingIn;
                }
                break;

            case AudioState.FadingIn:
                source.volume = Mathf.MoveTowards(source.volume, ogVolume, Time.deltaTime * fadeSpeed);
                if (source.volume >= ogVolume - .01f)
                {
                    state = AudioState.Playing;
                }
                break;
        }
    }
}