using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceHelper : MonoBehaviour 
{

    public float fadeInSpeed = 0.5f;
    public float fadeOutSpeed = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void PauseOrResume()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInEnum());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutEnum());
    }

    IEnumerator FadeInEnum()
    {
        while(audioSource.volume < 1.0f)
        {
            SetVolume(audioSource.volume + (fadeInSpeed * Time.deltaTime));
            yield return null;
        }
        SetVolume(1.0f);
    }

    IEnumerator FadeOutEnum()
    {
        while(audioSource.volume > 0.0f)
        {
            SetVolume(audioSource.volume - (fadeInSpeed * Time.deltaTime));
            yield return null;
        }
        SetVolume(0.0f);
        audioSource.Stop();
    }
}