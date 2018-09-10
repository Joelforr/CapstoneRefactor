using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXController : MonoBehaviour {

    private AudioSource audioSource;

    private float currentTime;
    private float timer;

    private float delay;
    private bool active;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = .1f;
        audioSource.priority = 80;

    }
    private void Update()
    {
        if(currentTime <= 0)
        {
            currentTime -= Time.deltaTime;
        }

    }


    public bool IsPlaying()
    {
        return audioSource.isPlaying || active == true;
    }

    public void Stop()
    {
        active = false;
        audioSource.loop = false;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void Play(AudioClip clip )
    {
        audioSource.PlayOneShot(clip);
    }


    public void Loop(AudioClip clip)
    {
        audioSource.loop = true;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    public void PlayDelayed(AudioClip clip)
    {
        StartCoroutine(LoopDelayed(clip));
    }


    private IEnumerator LoopDelayed(AudioClip clip)
    {
        if (active == true) yield break;

        active = true;
        audioSource.clip = clip;

        while (active == true)
        {
            yield return new WaitForSeconds(delay);
            audioSource.PlayOneShot(clip);
            Debug.Log("playing run");
        }
    }
}
